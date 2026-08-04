using System.Collections.Concurrent;

namespace StudentManager.Domain;

/// <summary>
/// Default repository backed by an in-memory dictionary — the same data
/// structure the legacy WinForms app used, made thread-safe for web hosting.
/// </summary>
public class InMemoryStudentRepository : IStudentRepository
{
    private readonly ConcurrentDictionary<string, Student> _students =
        new(StringComparer.OrdinalIgnoreCase);

    public bool Add(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        return _students.TryAdd(student.Id, student);
    }

    public Student? GetById(string id) =>
        string.IsNullOrWhiteSpace(id) ? null
        : _students.TryGetValue(id, out var student) ? student
        : null;

    public IReadOnlyList<Student> GetAll() =>
        _students.Values.OrderBy(s => s.Id, StringComparer.OrdinalIgnoreCase).ToList();

    public IReadOnlyList<Student> SearchByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Array.Empty<Student>();

        return _students.Values
            .Where(s => string.Equals(s.Name, name.Trim(), StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.Id, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public bool Update(string id, string name, bool enrolmentCompleted)
    {
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
            return false;

        if (!_students.TryGetValue(id, out var existing))
            return false;

        existing.Name = name.Trim();
        existing.EnrolmentCompleted = enrolmentCompleted;
        return true;
    }

    public bool Delete(string id) =>
        !string.IsNullOrWhiteSpace(id) && _students.TryRemove(id, out _);
}

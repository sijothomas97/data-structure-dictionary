using Microsoft.EntityFrameworkCore;
using StudentManager.Domain;

namespace StudentManager.Infrastructure;

/// <summary>
/// Database-backed implementation of <see cref="IStudentRepository"/> using
/// EF Core. Preserves the in-memory repository's semantics: case-insensitive
/// IDs and case-insensitive exact-name search. Comparisons use ToLower(),
/// which translates to LOWER() on both SQLite and PostgreSQL.
/// </summary>
public class EfStudentRepository(StudentDbContext db) : IStudentRepository
{
    public bool Add(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        var idLower = student.Id.ToLower();
        if (db.Students.Any(s => s.Id.ToLower() == idLower))
            return false;

        db.Students.Add(student);
        try
        {
            db.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            // Concurrent insert with the same primary key.
            db.Entry(student).State = EntityState.Detached;
            return false;
        }
    }

    public Student? GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        var idLower = id.Trim().ToLower();
        return db.Students.FirstOrDefault(s => s.Id.ToLower() == idLower);
    }

    public IReadOnlyList<Student> GetAll() =>
        db.Students.OrderBy(s => s.Id.ToLower()).ToList();

    public IReadOnlyList<Student> SearchByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Array.Empty<Student>();

        var nameLower = name.Trim().ToLower();
        return db.Students
            .Where(s => s.Name.ToLower() == nameLower)
            .OrderBy(s => s.Id.ToLower())
            .ToList();
    }

    public bool Update(string id, string name, bool enrolmentCompleted)
    {
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
            return false;

        var existing = GetById(id);
        if (existing is null)
            return false;

        existing.Name = name.Trim();
        existing.EnrolmentCompleted = enrolmentCompleted;
        db.SaveChanges();
        return true;
    }

    public bool Delete(string id)
    {
        var existing = GetById(id);
        if (existing is null)
            return false;

        db.Students.Remove(existing);
        db.SaveChanges();
        return true;
    }
}

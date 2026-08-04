namespace StudentManager.Domain;

/// <summary>
/// A student in the enrolment system. Ported from the legacy WinForms
/// <c>TaskB.Student</c> class (studentID / StudentName / StudentEnrolStatus).
/// </summary>
public class Student
{
    public required string Id { get; init; }
    public required string Name { get; set; }
    public bool EnrolmentCompleted { get; set; }

    /// <summary>Creates a validated <see cref="Student"/>.</summary>
    /// <exception cref="ArgumentException">Id or Name is null/whitespace.</exception>
    public static Student Create(string id, string name, bool enrolmentCompleted = false)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Student ID must not be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Student name must not be empty.", nameof(name));

        return new Student
        {
            Id = id.Trim(),
            Name = name.Trim(),
            EnrolmentCompleted = enrolmentCompleted
        };
    }
}

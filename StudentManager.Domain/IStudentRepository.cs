namespace StudentManager.Domain;

/// <summary>
/// Abstraction over student storage. The default implementation is the
/// in-memory dictionary the original app used; a database-backed
/// implementation can be swapped in later without touching callers.
/// </summary>
public interface IStudentRepository
{
    /// <summary>Adds a student. Returns false if a student with the same ID already exists.</summary>
    bool Add(Student student);

    /// <summary>Returns the student with the given ID, or null if not found.</summary>
    Student? GetById(string id);

    /// <summary>Returns all students, ordered by ID.</summary>
    IReadOnlyList<Student> GetAll();

    /// <summary>Case-insensitive exact-name search (matches the legacy "Details" behaviour).</summary>
    IReadOnlyList<Student> SearchByName(string name);

    /// <summary>Updates name and enrolment status of an existing student. Returns false if not found.</summary>
    bool Update(string id, string name, bool enrolmentCompleted);

    /// <summary>Deletes the student with the given ID. Returns false if not found.</summary>
    bool Delete(string id);
}

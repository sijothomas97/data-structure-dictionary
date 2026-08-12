namespace StudentManager.Api;

public record CreateStudentRequest(string Id, string Name, bool EnrolmentCompleted = false);

public record UpdateStudentRequest(string Name, bool EnrolmentCompleted);

public record StudentResponse(string Id, string Name, bool EnrolmentCompleted);

public record EnrolStatusResponse(string Id, bool EnrolmentCompleted);

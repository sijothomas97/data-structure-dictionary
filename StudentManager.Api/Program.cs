using Microsoft.EntityFrameworkCore;
using StudentManager.Api;
using StudentManager.Domain;
using StudentManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- Persistence: SQLite by default, PostgreSQL via configuration ---
// Select with Database:Provider = "Sqlite" | "Postgres" (env: Database__Provider)
// and ConnectionStrings:Default (env: ConnectionStrings__Default).
var dbProvider = builder.Configuration["Database:Provider"] ?? "Sqlite";
var usePostgres = dbProvider.Equals("Postgres", StringComparison.OrdinalIgnoreCase)
               || dbProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase)
               || dbProvider.Equals("Npgsql", StringComparison.OrdinalIgnoreCase);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? (usePostgres
        ? "Host=localhost;Database=students;Username=students;Password=students"
        : "Data Source=students.db");

builder.Services.AddDbContext<StudentDbContext>(options =>
{
    if (usePostgres)
        options.UseNpgsql(connectionString);
    else
        options.UseSqlite(connectionString);
});
builder.Services.AddScoped<IStudentRepository, EfStudentRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Student Manager API",
        Version = "v1",
        Description = "CRUD, search and enrolment-status endpoints for the student enrolment manager."
    });
});

var app = builder.Build();

// Apply EF Core migrations on startup. Retries cover the docker-compose case
// where PostgreSQL may still be starting when the API boots.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StudentDbContext>();
    var attemptsLeft = 10;
    while (true)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex) when (--attemptsLeft > 0)
        {
            app.Logger.LogWarning("Database not ready ({Message}); retrying in 2s ({AttemptsLeft} attempts left)...",
                ex.Message, attemptsLeft);
            Thread.Sleep(2000);
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();

// Static web frontend (wwwroot/index.html).
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
   .WithTags("Health");

var students = app.MapGroup("/api/students").WithTags("Students");

// List all
students.MapGet("/", (IStudentRepository repo) =>
        Results.Ok(repo.GetAll().Select(ToResponse)))
    .WithName("ListStudents")
    .Produces<IEnumerable<StudentResponse>>(StatusCodes.Status200OK);

// Search by name (before /{id} for clarity; route templates are distinct anyway)
students.MapGet("/search", (string name, IStudentRepository repo) =>
        string.IsNullOrWhiteSpace(name)
            ? Results.BadRequest(new { error = "Query parameter 'name' is required." })
            : Results.Ok(repo.SearchByName(name).Select(ToResponse)))
    .WithName("SearchStudentsByName")
    .Produces<IEnumerable<StudentResponse>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

// Get one
students.MapGet("/{id}", (string id, IStudentRepository repo) =>
        repo.GetById(id) is { } student
            ? Results.Ok(ToResponse(student))
            : Results.NotFound())
    .WithName("GetStudent")
    .Produces<StudentResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

// Enrolment status
students.MapGet("/{id}/enrol-status", (string id, IStudentRepository repo) =>
        repo.GetById(id) is { } student
            ? Results.Ok(new EnrolStatusResponse(student.Id, student.EnrolmentCompleted))
            : Results.NotFound())
    .WithName("GetEnrolStatus")
    .Produces<EnrolStatusResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

// Create
students.MapPost("/", (CreateStudentRequest request, IStudentRepository repo) =>
    {
        Student student;
        try
        {
            student = Student.Create(request.Id, request.Name, request.EnrolmentCompleted);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }

        return repo.Add(student)
            ? Results.Created($"/api/students/{student.Id}", ToResponse(student))
            : Results.Conflict(new { error = $"A student with ID '{student.Id}' already exists." });
    })
    .WithName("CreateStudent")
    .Produces<StudentResponse>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status409Conflict);

// Update (name + enrolment status)
students.MapPut("/{id}", (string id, UpdateStudentRequest request, IStudentRepository repo) =>
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Results.BadRequest(new { error = "Student name must not be empty." });

        if (!repo.Update(id, request.Name, request.EnrolmentCompleted))
            return Results.NotFound();

        var updated = repo.GetById(id)!;
        return Results.Ok(ToResponse(updated));
    })
    .WithName("UpdateStudent")
    .Produces<StudentResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound);

// Delete
students.MapDelete("/{id}", (string id, IStudentRepository repo) =>
        repo.Delete(id) ? Results.NoContent() : Results.NotFound())
    .WithName("DeleteStudent")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();

static StudentResponse ToResponse(Student s) => new(s.Id, s.Name, s.EnrolmentCompleted);

/// <summary>Exposed for WebApplicationFactory-based integration tests.</summary>
public partial class Program { }

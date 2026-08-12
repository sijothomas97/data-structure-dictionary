using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudentManager.Domain;
using StudentManager.Infrastructure;
using Xunit;

namespace StudentManager.Tests.Infrastructure;

/// <summary>
/// Tests the EF Core repository against an in-memory SQLite database,
/// asserting it preserves the InMemoryStudentRepository semantics
/// (case-insensitive IDs and name search).
/// </summary>
public sealed class EfStudentRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly StudentDbContext _db;
    private readonly EfStudentRepository _repo;

    public EfStudentRepositoryTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<StudentDbContext>()
            .UseSqlite(_connection)
            .Options;
        _db = new StudentDbContext(options);
        _db.Database.EnsureCreated();
        _repo = new EfStudentRepository(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public void Add_ThenGetById_RoundTrips()
    {
        _repo.Add(Student.Create("S001", "Ada Lovelace", true)).Should().BeTrue();

        var found = _repo.GetById("S001");
        found.Should().NotBeNull();
        found!.Name.Should().Be("Ada Lovelace");
        found.EnrolmentCompleted.Should().BeTrue();
    }

    [Fact]
    public void Add_DuplicateId_DifferentCase_ReturnsFalse()
    {
        _repo.Add(Student.Create("S001", "Ada")).Should().BeTrue();
        _repo.Add(Student.Create("s001", "Bob")).Should().BeFalse();
        _db.Students.Count().Should().Be(1);
    }

    [Fact]
    public void GetById_IsCaseInsensitive_AndTrims()
    {
        _repo.Add(Student.Create("S001", "Ada"));

        _repo.GetById("s001").Should().NotBeNull();
        _repo.GetById("  S001  ").Should().NotBeNull();
        _repo.GetById("S999").Should().BeNull();
        _repo.GetById("  ").Should().BeNull();
    }

    [Fact]
    public void GetAll_ReturnsAllOrderedById()
    {
        _repo.Add(Student.Create("b2", "Bob"));
        _repo.Add(Student.Create("A1", "Ada"));
        _repo.Add(Student.Create("c3", "Cyd"));

        _repo.GetAll().Select(s => s.Id).Should().ContainInOrder("A1", "b2", "c3");
    }

    [Fact]
    public void SearchByName_ExactMatch_CaseInsensitive()
    {
        _repo.Add(Student.Create("S001", "Grace Hopper"));
        _repo.Add(Student.Create("S002", "grace hopper", true));
        _repo.Add(Student.Create("S003", "Grace"));

        var results = _repo.SearchByName("GRACE HOPPER");
        results.Select(s => s.Id).Should().BeEquivalentTo(new[] { "S001", "S002" });

        _repo.SearchByName("nobody").Should().BeEmpty();
        _repo.SearchByName("   ").Should().BeEmpty();
    }

    [Fact]
    public void Update_ExistingStudent_PersistsChanges()
    {
        _repo.Add(Student.Create("S001", "Before", false));

        _repo.Update("s001", "  After  ", true).Should().BeTrue();

        var updated = _repo.GetById("S001")!;
        updated.Name.Should().Be("After");
        updated.EnrolmentCompleted.Should().BeTrue();
    }

    [Fact]
    public void Update_Unknown_ReturnsFalse()
    {
        _repo.Update("nope", "Name", true).Should().BeFalse();
        _repo.Update("", "Name", true).Should().BeFalse();
        _repo.Update("id", " ", true).Should().BeFalse();
    }

    [Fact]
    public void Delete_RemovesStudent_CaseInsensitive()
    {
        _repo.Add(Student.Create("S001", "Ada"));

        _repo.Delete("s001").Should().BeTrue();
        _repo.GetById("S001").Should().BeNull();
        _repo.Delete("S001").Should().BeFalse();
    }
}

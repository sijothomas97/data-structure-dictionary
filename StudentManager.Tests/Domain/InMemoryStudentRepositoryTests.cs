using FluentAssertions;
using StudentManager.Domain;
using Xunit;

namespace StudentManager.Tests.Domain;

public class InMemoryStudentRepositoryTests
{
    private readonly InMemoryStudentRepository _repo = new();

    private static Student NewStudent(string id = "S001", string name = "Ada", bool enrolled = false)
        => Student.Create(id, name, enrolled);

    [Fact]
    public void Add_NewStudent_ReturnsTrue_AndIsRetrievable()
    {
        _repo.Add(NewStudent()).Should().BeTrue();

        var found = _repo.GetById("S001");
        found.Should().NotBeNull();
        found!.Name.Should().Be("Ada");
    }

    [Fact]
    public void Add_DuplicateId_ReturnsFalse()
    {
        _repo.Add(NewStudent()).Should().BeTrue();
        _repo.Add(NewStudent(name: "Someone Else")).Should().BeFalse();
    }

    [Fact]
    public void Add_DuplicateId_DifferentCase_ReturnsFalse()
    {
        _repo.Add(NewStudent("s001")).Should().BeTrue();
        _repo.Add(NewStudent("S001")).Should().BeFalse();
    }

    [Fact]
    public void GetById_Unknown_ReturnsNull()
    {
        _repo.GetById("nope").Should().BeNull();
        _repo.GetById("").Should().BeNull();
    }

    [Fact]
    public void GetAll_ReturnsAllStudents_OrderedById()
    {
        _repo.Add(NewStudent("S002", "Bob"));
        _repo.Add(NewStudent("S001", "Ada"));
        _repo.Add(NewStudent("S003", "Cara"));

        _repo.GetAll().Select(s => s.Id).Should().ContainInOrder("S001", "S002", "S003");
    }

    [Fact]
    public void SearchByName_ExactMatch_CaseInsensitive()
    {
        _repo.Add(NewStudent("S001", "Ada Lovelace"));
        _repo.Add(NewStudent("S002", "ada lovelace", enrolled: true));
        _repo.Add(NewStudent("S003", "Grace Hopper"));

        var results = _repo.SearchByName("ADA LOVELACE");

        results.Should().HaveCount(2);
        results.Select(s => s.Id).Should().ContainInOrder("S001", "S002");
    }

    [Fact]
    public void SearchByName_NoMatch_ReturnsEmpty()
    {
        _repo.Add(NewStudent("S001", "Ada"));
        _repo.SearchByName("Zed").Should().BeEmpty();
        _repo.SearchByName("").Should().BeEmpty();
    }

    [Fact]
    public void Update_ExistingStudent_ChangesNameAndStatus()
    {
        _repo.Add(NewStudent("S001", "Ada", enrolled: false));

        _repo.Update("S001", "Ada L.", enrolmentCompleted: true).Should().BeTrue();

        var updated = _repo.GetById("S001")!;
        updated.Name.Should().Be("Ada L.");
        updated.EnrolmentCompleted.Should().BeTrue();
    }

    [Fact]
    public void Update_Unknown_ReturnsFalse()
    {
        _repo.Update("nope", "Name", true).Should().BeFalse();
    }

    [Fact]
    public void Delete_ExistingStudent_RemovesIt()
    {
        _repo.Add(NewStudent());
        _repo.Delete("S001").Should().BeTrue();
        _repo.GetById("S001").Should().BeNull();
    }

    [Fact]
    public void Delete_Unknown_ReturnsFalse()
    {
        _repo.Delete("nope").Should().BeFalse();
    }
}

using FluentAssertions;
using StudentManager.Domain;
using Xunit;

namespace StudentManager.Tests.Domain;

public class StudentTests
{
    [Fact]
    public void Create_WithValidValues_TrimsAndAssigns()
    {
        var student = Student.Create("  S001 ", "  Ada Lovelace ", enrolmentCompleted: true);

        student.Id.Should().Be("S001");
        student.Name.Should().Be("Ada Lovelace");
        student.EnrolmentCompleted.Should().BeTrue();
    }

    [Fact]
    public void Create_DefaultsEnrolmentToNotCompleted()
    {
        Student.Create("S001", "Ada").EnrolmentCompleted.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidId_Throws(string? id)
    {
        var act = () => Student.Create(id!, "Ada");
        act.Should().Throw<ArgumentException>().WithParameterName("id");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_Throws(string? name)
    {
        var act = () => Student.Create("S001", name!);
        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }
}

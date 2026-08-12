using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using StudentManager.Api;
using Xunit;

namespace StudentManager.Tests.Api;

public class StudentApiTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    // Each test class instance (i.e. each test) gets its own SQLite database
    // file, so tests are fully isolated and re-runs start clean.
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly string _dbPath;

    public StudentApiTests(WebApplicationFactory<Program> factory)
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"students-apitest-{Guid.NewGuid():N}.db");
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("Database:Provider", "Sqlite");
            builder.UseSetting("ConnectionStrings:Default", $"Data Source={_dbPath}");
        });
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        foreach (var suffix in new[] { "", "-wal", "-shm" })
        {
            try { File.Delete(_dbPath + suffix); } catch { /* best effort */ }
        }
    }

    private static CreateStudentRequest NewRequest(string id, string name = "Ada", bool enrolled = false)
        => new(id, name, enrolled);

    [Fact]
    public async Task Health_ReturnsHealthy()
    {
        var response = await _client.GetAsync("/health");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Swagger_Json_IsServed()
    {
        var response = await _client.GetAsync("/swagger/v1/swagger.json");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Student Manager API");
    }

    [Fact]
    public async Task Post_CreatesStudent_Returns201WithLocation()
    {
        var response = await _client.PostAsJsonAsync("/api/students", NewRequest("api-post-1", "Ada", true));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().Be("/api/students/api-post-1");

        var created = await response.Content.ReadFromJsonAsync<StudentResponse>();
        created.Should().Be(new StudentResponse("api-post-1", "Ada", true));
    }

    [Theory]
    [InlineData("", "Ada")]
    [InlineData("  ", "Ada")]
    [InlineData("S001", "")]
    [InlineData("S001", "   ")]
    public async Task Post_WithMissingFields_Returns400(string id, string name)
    {
        var response = await _client.PostAsJsonAsync("/api/students", NewRequest(id, name));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_DuplicateId_Returns409()
    {
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-dup-1"))).EnsureSuccessStatusCode();

        var response = await _client.PostAsJsonAsync("/api/students", NewRequest("api-dup-1"));
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Get_ExistingStudent_ReturnsIt()
    {
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-get-1", "Grace"))).EnsureSuccessStatusCode();

        var student = await _client.GetFromJsonAsync<StudentResponse>("/api/students/api-get-1");
        student.Should().Be(new StudentResponse("api-get-1", "Grace", false));
    }

    [Fact]
    public async Task Get_Unknown_Returns404()
    {
        var response = await _client.GetAsync("/api/students/does-not-exist");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_IncludesCreatedStudents()
    {
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-list-1", "Ada"))).EnsureSuccessStatusCode();
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-list-2", "Bob"))).EnsureSuccessStatusCode();

        var all = await _client.GetFromJsonAsync<List<StudentResponse>>("/api/students");
        all.Should().NotBeNull();
        all!.Select(s => s.Id).Should().Contain(new[] { "api-list-1", "api-list-2" });
    }

    [Fact]
    public async Task Search_ByName_ReturnsMatches_CaseInsensitive()
    {
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-search-1", "Findable Person"))).EnsureSuccessStatusCode();
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-search-2", "findable person", true))).EnsureSuccessStatusCode();

        var results = await _client.GetFromJsonAsync<List<StudentResponse>>(
            "/api/students/search?name=FINDABLE%20PERSON");

        results.Should().NotBeNull();
        results!.Select(s => s.Id).Should().BeEquivalentTo(new[] { "api-search-1", "api-search-2" });
    }

    [Fact]
    public async Task Search_WithoutName_Returns400()
    {
        var response = await _client.GetAsync("/api/students/search");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task EnrolStatus_ReturnsStatus()
    {
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-status-1", "Ada", true))).EnsureSuccessStatusCode();

        var status = await _client.GetFromJsonAsync<EnrolStatusResponse>("/api/students/api-status-1/enrol-status");
        status.Should().Be(new EnrolStatusResponse("api-status-1", true));
    }

    [Fact]
    public async Task EnrolStatus_Unknown_Returns404()
    {
        var response = await _client.GetAsync("/api/students/does-not-exist/enrol-status");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_UpdatesNameAndStatus()
    {
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-put-1", "Before", false))).EnsureSuccessStatusCode();

        var response = await _client.PutAsJsonAsync("/api/students/api-put-1",
            new UpdateStudentRequest("After", true));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<StudentResponse>();
        updated.Should().Be(new StudentResponse("api-put-1", "After", true));
    }

    [Fact]
    public async Task Put_Unknown_Returns404()
    {
        var response = await _client.PutAsJsonAsync("/api/students/does-not-exist",
            new UpdateStudentRequest("Name", true));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_EmptyName_Returns400()
    {
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-put-2"))).EnsureSuccessStatusCode();

        var response = await _client.PutAsJsonAsync("/api/students/api-put-2",
            new UpdateStudentRequest("  ", true));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_RemovesStudent()
    {
        (await _client.PostAsJsonAsync("/api/students", NewRequest("api-del-1"))).EnsureSuccessStatusCode();

        (await _client.DeleteAsync("/api/students/api-del-1")).StatusCode
            .Should().Be(HttpStatusCode.NoContent);
        (await _client.GetAsync("/api/students/api-del-1")).StatusCode
            .Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Unknown_Returns404()
    {
        var response = await _client.DeleteAsync("/api/students/does-not-exist");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

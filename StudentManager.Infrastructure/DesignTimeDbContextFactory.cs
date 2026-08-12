using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentManager.Infrastructure;

/// <summary>
/// Used by the `dotnet ef` CLI to create the context at design time
/// (e.g. `dotnet ef migrations add X --project StudentManager.Infrastructure`).
/// The connection string is never opened for `migrations add`.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StudentDbContext>
{
    public StudentDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<StudentDbContext>()
            .UseSqlite("Data Source=design-time.db")
            .Options;
        return new StudentDbContext(options);
    }
}

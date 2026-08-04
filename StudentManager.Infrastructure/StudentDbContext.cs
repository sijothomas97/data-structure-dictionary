using Microsoft.EntityFrameworkCore;
using StudentManager.Domain;

namespace StudentManager.Infrastructure;

/// <summary>
/// EF Core context for the student store. Provider-agnostic: configured with
/// SQLite by default or PostgreSQL via configuration (see Program.cs).
/// </summary>
public class StudentDbContext(DbContextOptions<StudentDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).HasMaxLength(64);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.Property(s => s.EnrolmentCompleted).IsRequired();
        });
    }
}

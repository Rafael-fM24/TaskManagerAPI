using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<TaskNote> TaskNotes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(builder =>
        {
            builder.HasKey(t => t.Id);

            builder
                .HasMany(t => t.Notes)
                .WithOne()
                .HasForeignKey("TaskItemId")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Navigation(t => t.Notes)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        base.OnModelCreating(modelBuilder);
    }
}
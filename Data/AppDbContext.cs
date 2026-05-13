using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<UserModel> Users {get; set;}

    public DbSet<TaskModel> Tasks {get; set;}

    public DbSet<AssignmentModel> Assignments {get; set;}

}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WorkoutTracker.Models
{
    public class WorkoutContext : IdentityDbContext
    { 
        public WorkoutContext(DbContextOptions<WorkoutContext> options) : base(options) { } 

        public DbSet<Workout> Workouts { get; set; } 

        public DbSet<Exercise> Exercises { get; set; }
    } 
}

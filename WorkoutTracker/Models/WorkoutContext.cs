using Microsoft.EntityFrameworkCore;

namespace WorkoutTracker.Models
{
    public class WorkoutContext : DbContext
    { 
        public WorkoutContext(DbContextOptions<WorkoutContext> options) : base(options) { } 

        public DbSet<Workout> Workouts { get; set; }
    }
}

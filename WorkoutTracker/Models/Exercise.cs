using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class Exercise 
    { 
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Sets { get; set; }

        [Required]
        public int Reps { get; set; }

        [Required]
        public double Weight { get; set; }

        public int WorkoutId { get; set; } 

        public Workout? Workout { get; set; }
    }
}

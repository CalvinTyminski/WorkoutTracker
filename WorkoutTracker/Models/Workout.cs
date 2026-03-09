using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class Workout 
    { 
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter a name for workout")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } 

        public int DurationMinutes { get; set; }

        public List<Exercise> Exercises { get; set; } = new List<Exercise>(); 

        public string UserId { get; set; }
    }
}

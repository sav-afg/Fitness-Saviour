namespace WebsiteFirstDraft.Data.Models
{
    public class ExerciseType
    {
       
        public int Id { get; set; }

       
        public string ExerciseTypes { get; set; } = string.Empty;

        
        public string ExerciseNames { get; set; } = string.Empty;

        public double CaloriesBurnedPerMinute { get; set; }

        public string IntensityLevel { get; set; } = string.Empty;
    }

}

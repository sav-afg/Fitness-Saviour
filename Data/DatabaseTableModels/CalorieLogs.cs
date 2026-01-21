using System.ComponentModel.DataAnnotations;
using WebsiteFirstDraft.Components.Pages.Diet_Food;

namespace WebsiteFirstDraft.Data.DatabaseTableModels
{
    // Represents a log entry for calorie tracking. New table on SQL Server. Required for graphs to work
    public class CalorieLogs
    {
        // Primary key for the CalorieLogs table
        [Key]
        public int CalorieLog_Id { get; set; }

        // Foreign key referencing the user
        [Required]
        public int User_id { get; set; } 

        [Required]
        public DateTime Log_Date { get; set; } = DateTime.Now;

        // Nullable integer for calories consumed
        public int? Calories_Consumed { get; set; }

        
        public int? Calories_Burned { get; set; }

        
        public int Net_Calories {  get; set; }

        // Nullable integers for calories from macros
        public int? Calories_From_Carbs { get; set; }
        public int? Calories_From_Protein { get; set; }
        public int? Calories_From_Fats { get; set; }

        // Macro tracking in grams
        public int Carbs_Consumed { get; set; }
        public int Protein_Consumed { get; set; }
        public int Fat_Consumed { get; set; }

        //// Navigation Properties

        //public required User User { get; set; }
        //public FoodType? FoodType { get; set; }

        //public ExerciseType? ExerciseType { get; set; }
    }
}

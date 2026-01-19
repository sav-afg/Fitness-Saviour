using System.ComponentModel.DataAnnotations;

namespace WebsiteFirstDraft.Data.DatabaseTableModels
{
    // Represents a log entry for tracking user weight over time
    public class WeightLog
    {
        [Key]
        public int WeightLogId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        public double Weight { get; set; }

        // Navigation property
        public required User User { get; set; }
    }
}


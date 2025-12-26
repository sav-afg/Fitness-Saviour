namespace WebsiteFirstDraft.Data.Models
{
    // Stores UI preferences chosen by the user
    // This allows global access across all pages
    public class UISettings
    {
        public string ExperienceLevel { get; set; } = "Beginner";
        public string PrimaryGoal { get; set; } = "General";
        public bool HighContrast { get; set; }
        public bool DyslexicFont { get; set; }
        public bool ReduceAnimations { get; set; }
        public bool ShowAdvancedMetrics { get; set; }
        public bool EnableGamification { get; set; }
        public bool LargerTextSize { get; set; }
        public bool Calories { get; set; }
        public bool Macros { get; set; }
        public bool Exercise { get; set; }
        public bool WeightTrends{ get; set; }
        public bool ProgressData { get; set; }
        public bool MinimalInterface { get; set; }
    }
}

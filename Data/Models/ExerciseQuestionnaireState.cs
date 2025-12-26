namespace WebsiteFirstDraft.Data.Models
{
    public class ExerciseQuestionnaireState
    {
        public string FitnessGoal { get; set; } = "";
        public bool JointPain { get; set; }
        public bool BackIssues { get; set; }
        public bool CardioLimitations { get; set; }

        public string ExperienceLevel { get; set; } = "";
        public string Intensity { get; set; }
        public string LocationPreference { get; set; } = "";

        public byte FrequencyPerWeekValue { get; set; }

    }
}

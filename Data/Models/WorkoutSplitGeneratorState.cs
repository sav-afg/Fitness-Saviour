namespace WebsiteFirstDraft.Data.Models
{
    public class WorkoutSplitGeneratorState
    {
        public string Experience { get; set; } = "";
        public string MuscleGroups { get; set; }

        public byte Preference { get; set; } 
        public byte FrequencyPerWeekValue { get; set; }

    }
}

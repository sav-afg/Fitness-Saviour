namespace WebsiteFirstDraft.Data.Models
{
    public class DietQuestionnaireState
    {
        public string PrimaryGoal { get; set; } = "";
        public bool Vegetarian { get; set; }
        public bool Vegan { get; set; }
        public bool Halal { get; set; }

        public string ExperienceLevel { get; set; } = "";
        public byte MealNumber { get; set; }
        public string StructurePreference { get; set; } = "";
    }

}

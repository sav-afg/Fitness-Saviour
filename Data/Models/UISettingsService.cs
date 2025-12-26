namespace WebsiteFirstDraft.Data.Models
{
    // Singleton service which will store UI settings across entire website
    // This ensures personalisation affects all pages
    public class UISettingsService
    {
        public UISettings Settings { get; set; } = new UISettings();
    }

}

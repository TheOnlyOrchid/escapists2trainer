namespace EscapistsTrainer
{
    // interface for modules
    public interface ITrainerModule
    {
        string Id { get; }
        string DisplayName { get; }
        string Description { get; }
        string[] CategoryPath { get; }
        bool SupportsToggle { get; }
        bool IsEnabled { get; set; }
        bool HasSettings { get; }
        void DrawMain();
        void DrawSettings();
        ModuleWindow[] GetWindows();
    }
}

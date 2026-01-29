namespace EscapistsTrainer
{
    public sealed class TrainerUiState
    {
        public ITrainerModule SelectedModule { get; set; }
        public ModulePanel ActivePanel { get; set; }
    }

    public enum ModulePanel
    {
        Main,
        Settings
    }
}

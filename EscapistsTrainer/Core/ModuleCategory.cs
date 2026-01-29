using System.Collections.Generic;

namespace EscapistsTrainer
{
    public sealed class ModuleCategory
    {
        public ModuleCategory(string name)
        {
            Name = name;
            Subcategories = new List<ModuleCategory>();
            Modules = new List<ITrainerModule>();
            IsExpanded = false;
        }

        public string Name { get; private set; }
        public List<ModuleCategory> Subcategories { get; private set; }
        public List<ITrainerModule> Modules { get; private set; }
        public bool IsExpanded { get; set; }
    }
}

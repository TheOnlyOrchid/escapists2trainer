using System;
using System.Collections.Generic;

namespace EscapistsTrainer
{
    public sealed class ModuleRegistry
    {
        private readonly List<ModuleCategory> _rootCategories = new List<ModuleCategory>();

        public ModuleRegistry()
        {
            Keybinds = new KeybindManager();
        }

        public IList<ModuleCategory> RootCategories { get { return _rootCategories; } }
        public KeybindManager Keybinds { get; private set; }

        public void RegisterModule(ITrainerModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }

            ModuleCategory category = GetOrCreateCategory(module.CategoryPath);
            category.Modules.Add(module);
        }

        private ModuleCategory GetOrCreateCategory(string[] path)
        {
            if (path == null || path.Length == 0)
            {
                return GetOrCreateCategory(new string[] { "General" });
            }

            List<ModuleCategory> currentLevel = _rootCategories;
            ModuleCategory current = null;

            for (int i = 0; i < path.Length; i++)
            {
                string name = path[i] ?? "General";
                current = FindOrCreate(currentLevel, name);
                currentLevel = current.Subcategories;
            }

            return current;
        }

        private ModuleCategory FindOrCreate(List<ModuleCategory> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (string.Equals(list[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return list[i];
                }
            }

            ModuleCategory category = new ModuleCategory(name);
            list.Add(category);
            return category;
        }
    }
}

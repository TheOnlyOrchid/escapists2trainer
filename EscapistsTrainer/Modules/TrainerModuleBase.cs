using System.Collections.Generic;
using UnityEngine;

namespace EscapistsTrainer
{
    public abstract class TrainerModuleBase : ITrainerModule
    {
        private readonly List<ModuleSetting> _settings = new List<ModuleSetting>();
        private ModuleWindow[] _windows;

        public abstract string Id { get; }
        public abstract string DisplayName { get; }
        public abstract string Description { get; }
        public abstract string[] CategoryPath { get; }
        public virtual bool SupportsToggle { get { return true; } }
        public virtual bool IsEnabled { get; set; }
        public virtual bool HasSettings { get { return _settings.Count > 0; } }

        public virtual void DrawMain()
        {
            GUILayout.Label("This module has no GUI :(", GUI.skin.label);
        }

        public virtual void DrawSettings()
        {
            if (_settings.Count == 0)
            {
                GUILayout.Label("No settings for this module.", GUI.skin.label);
                return;
            }

            for (int i = 0; i < _settings.Count; i++)
            {
                _settings[i].Draw();
                GUILayout.Space(4f);
            }
        }

        public virtual ModuleWindow[] GetWindows()
        {
            return _windows;
        }

        protected void AddSetting(ModuleSetting setting)
        {
            if (setting != null)
            {
                _settings.Add(setting);
            }
        }

        protected void SetWindows(ModuleWindow[] windows)
        {
            _windows = windows;
        }
    }
}

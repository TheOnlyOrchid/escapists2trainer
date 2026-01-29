using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace EscapistsTrainer
{
    public sealed class TimeFreezeModule : TrainerModuleBase
    {
        private readonly Keybind _toggleKeybind;
        private static bool _isFrozen;
        internal static bool IsFrozen
        {
            get { return _isFrozen; }
            set { _isFrozen = value; }
        }

        public TimeFreezeModule(KeybindManager keybinds)
        {
            _toggleKeybind = new Keybind("Toggle Time Freeze", KeyCode.F5, false, false, false);
            keybinds.Register(_toggleKeybind, ToggleEnabled);

            AddSetting(new ToggleSetting("Show status message", "Display a message when time freeze is toggled.", true));
        }

        public override string Id { get { return "time-freeze"; } }
        public override string DisplayName { get { return "Time Freeze"; } }
        public override string Description { get { return "Pauses or resumes in-game time."; } }
        public override string[] CategoryPath { get { return new string[] { "World" }; } }

        public override bool IsEnabled
        {
            get { return IsFrozen; }
            set { IsFrozen = value; }
        }

        public override void DrawMain()
        {
            GUILayout.Label("Time Freeze", GUI.skin.label);
            GUILayout.Label("Toggle the module to freeze time.", GUI.skin.label);
            GUILayout.Label("Keybind: " + _toggleKeybind.ToDisplayString(), GUI.skin.label);
        }

        private void ToggleEnabled()
        {
            IsEnabled = !IsEnabled;
        }
    }

    [HarmonyPatch]
    internal static class Patch_TimeFreeze_ProcessTime
    {
        static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("RoutineManager");
            if (type == null)
            {
                return null;
            }

            return AccessTools.Method(type, "ProcessTime");
        }

        [HarmonyPrefix]
        static bool Prefix()
        {
            return !TimeFreezeModule.IsFrozen;
        }
    }
}

using System;
using System.Globalization;
using HarmonyLib;
using UnityEngine;

namespace EscapistsTrainer
{
    public sealed class SetTimeModule : TrainerModuleBase
    {
        private string _timeInput = "12:00";
        private string _statusMessage;

        public override string Id { get { return "set-time"; } }
        public override string DisplayName { get { return "Set Time"; } }
        public override string Description { get { return "Sets the in-game time to a specific hour and minute."; } }
        public override string[] CategoryPath { get { return new string[] { "World", "Time" }; } }
        public override bool SupportsToggle { get { return false; } }

        public override void DrawMain()
        {
            GUILayout.Label("Set Time", GUI.skin.label);
            GUILayout.Label("Enter time as hh:mm and press Set.", GUI.skin.label);

            GUILayout.BeginHorizontal();
            _timeInput = GUILayout.TextField(_timeInput, 5);
            if (GUILayout.Button("Set", GUILayout.Width(60f)))
            {
                _statusMessage = TrySetTime(_timeInput);
            }
            GUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(_statusMessage))
            {
                GUILayout.Label(_statusMessage, GUI.skin.label);
            }
        }

        private string TrySetTime(string input)
        {
            if (!TryParseTime(input, out int hour, out int minutes))
            {
                return "Invalid time. Use hh:mm (00:00 - 23:59).";
            }

            var routineManagerType = AccessTools.TypeByName("RoutineManager");
            if (routineManagerType == null)
            {
                return "RoutineManager type not found.";
            }

            var routineManager = UnityEngine.Object.FindObjectOfType(routineManagerType);
            if (routineManager == null)
            {
                return "RoutineManager instance not found.";
            }

            var setTime = AccessTools.Method(routineManagerType, "SetTime", new[] { typeof(int), typeof(int), typeof(bool) });
            if (setTime == null)
            {
                setTime = AccessTools.Method(routineManagerType, "SetTime");
            }

            if (setTime == null)
            {
                return "SetTime method not found.";
            }

            try
            {
                setTime.Invoke(routineManager, new object[] { hour, minutes, false });
            }
            catch (Exception ex)
            {
                return "Failed to set time: " + ex.GetType().Name;
            }

            return "Time set to " + hour.ToString("00", CultureInfo.InvariantCulture) + ":" + minutes.ToString("00", CultureInfo.InvariantCulture) + ".";
        }

        private static bool TryParseTime(string input, out int hour, out int minutes)
        {
            hour = 0;
            minutes = 0;

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            string[] parts = input.Trim().Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out hour))
            {
                return false;
            }

            if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out minutes))
            {
                return false;
            }

            if (hour < 0 || hour > 23)
            {
                return false;
            }

            if (minutes < 0 || minutes > 59)
            {
                return false;
            }

            return true;
        }
    }
}

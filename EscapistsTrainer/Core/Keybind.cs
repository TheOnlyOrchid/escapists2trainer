using UnityEngine;

namespace EscapistsTrainer
{
    public sealed class Keybind
    {
        public Keybind(string name, KeyCode key, bool ctrl, bool alt, bool shift)
        {
            Name = name;
            Key = key;
            Ctrl = ctrl;
            Alt = alt;
            Shift = shift;
        }

        public string Name { get; private set; }
        public KeyCode Key { get; private set; }
        public bool Ctrl { get; private set; }
        public bool Alt { get; private set; }
        public bool Shift { get; private set; }

        public bool IsDown()
        {
            if (!Input.GetKeyDown(Key))
            {
                return false;
            }

            if (Ctrl && !IsCtrlHeld())
            {
                return false;
            }

            if (Alt && !IsAltHeld())
            {
                return false;
            }

            if (Shift && !IsShiftHeld())
            {
                return false;
            }

            return true;
        }

        public string ToDisplayString()
        {
            string label = "";

            if (Ctrl)
            {
                label += "Ctrl + ";
            }

            if (Alt)
            {
                label += "Alt + ";
            }

            if (Shift)
            {
                label += "Shift + ";
            }

            label += Key.ToString();
            return label;
        }

        private static bool IsCtrlHeld()
        {
            return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        }

        private static bool IsAltHeld()
        {
            return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        }

        private static bool IsShiftHeld()
        {
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }
    }
}

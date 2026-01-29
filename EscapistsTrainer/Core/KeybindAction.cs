using System;

namespace EscapistsTrainer
{
    public sealed class KeybindAction
    {
        public KeybindAction(Keybind keybind, Action action)
        {
            Keybind = keybind;
            Action = action;
        }

        public Keybind Keybind { get; private set; }
        public Action Action { get; private set; }
    }
}

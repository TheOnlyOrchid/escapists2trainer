using System;
using System.Collections.Generic;

namespace EscapistsTrainer
{
    public sealed class KeybindManager
    {
        private readonly List<KeybindAction> _actions = new List<KeybindAction>();

        public void Register(Keybind keybind, Action action)
        {
            if (keybind == null)
            {
                throw new ArgumentNullException("keybind");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            _actions.Add(new KeybindAction(keybind, action));
        }

        public void Update()
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                if (_actions[i].Keybind.IsDown())
                {
                    _actions[i].Action();
                }
            }
        }

        public KeybindAction[] GetAll()
        {
            return _actions.ToArray();
        }
    }
}

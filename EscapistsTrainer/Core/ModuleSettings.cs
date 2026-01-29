using UnityEngine;

namespace EscapistsTrainer
{
    public abstract class ModuleSetting
    {
        protected ModuleSetting(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public abstract void Draw();
    }

    public sealed class ToggleSetting : ModuleSetting
    {
        public ToggleSetting(string name, string description, bool value)
            : base(name, description)
        {
            Value = value;
        }

        public bool Value { get; set; }

        public override void Draw()
        {
            Value = GUILayout.Toggle(Value, Name, GUI.skin.toggle);

            if (!string.IsNullOrEmpty(Description))
            {
                GUILayout.Label(Description, GUI.skin.label);
            }
        }
    }

    public sealed class SliderSetting : ModuleSetting
    {
        public SliderSetting(string name, string description, float value, float min, float max)
            : base(name, description)
        {
            Value = value;
            Min = min;
            Max = max;
        }

        public float Value { get; set; }
        public float Min { get; private set; }
        public float Max { get; private set; }

        public override void Draw()
        {
            GUILayout.Label(Name, GUI.skin.label);
            Value = GUILayout.HorizontalSlider(Value, Min, Max);

            if (!string.IsNullOrEmpty(Description))
            {
                GUILayout.Label(Description, GUI.skin.label);
            }
        }
    }

    public sealed class ChoiceSetting : ModuleSetting
    {
        public ChoiceSetting(string name, string description, string[] options, int selectedIndex)
            : base(name, description)
        {
            Options = options;
            SelectedIndex = selectedIndex;
        }

        public string[] Options { get; private set; }
        public int SelectedIndex { get; set; }

        public override void Draw()
        {
            GUILayout.Label(Name, GUI.skin.label);

            if (Options != null && Options.Length > 0)
            {
                SelectedIndex = GUILayout.SelectionGrid(SelectedIndex, Options, 1, GUI.skin.button);
            }

            if (!string.IsNullOrEmpty(Description))
            {
                GUILayout.Label(Description, GUI.skin.label);
            }
        }
    }
}

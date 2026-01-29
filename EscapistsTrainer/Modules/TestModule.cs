using UnityEngine;

namespace EscapistsTrainer
{
    public sealed class TestModule : TrainerModuleBase
    {
        private readonly string _id;
        private readonly string _name;
        private readonly string _description;
        private readonly string[] _path;

        public TestModule(string name, string description, string[] categoryPath)
        {
            _name = name;
            _description = description;
            _path = categoryPath;
            _id = name.Replace(' ', '-').ToLowerInvariant();

            AddSetting(new SliderSetting("Example slider", "Test test test", 0.5f, 0f, 1f));
            AddSetting(new ChoiceSetting("Mode", "Switch between test modes", new string[] { "Test1", "Test2", "Test3" }, 1));
        }

        public override string Id { get { return _id; } }
        public override string DisplayName { get { return _name; } }
        public override string Description { get { return _description; } }
        public override string[] CategoryPath { get { return _path; } }

        public override void DrawMain()
        {
            GUILayout.Label("This is a placeholder module.", GUI.skin.label);
            GUILayout.Label("Add UI widgets here when the module is ready.", GUI.skin.label);
        }
    }
}

using UnityEngine;

namespace EscapistsTrainer
{
    public sealed class ModuleWindow
    {
        public ModuleWindow(int id, string title, Rect rect, GUI.WindowFunction draw)
        {
            Id = id;
            Title = title;
            Rect = rect;
            Draw = draw;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public Rect Rect { get; set; }
        public GUI.WindowFunction Draw { get; private set; }
    }
}

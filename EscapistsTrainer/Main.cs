using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace EscapistsTrainer
{
    [BepInPlugin("orchid.escapists2.trainer", "Escapists 2 Trainer", "0.1.0")]
    public sealed class TrainerPlugin : BaseUnityPlugin
    {
        private readonly Keybind _toggleUi = new Keybind("Toggle UI", KeyCode.F1, false, false, false);
        private ModuleRegistry _registry;
        private TrainerUiState _uiState;
        private Rect _windowRect = new Rect(120f, 80f, 980f, 640f);
        private Vector2 _navScroll;
        private Vector2 _mainScroll;
        private bool _showUi = true;

        private void Awake()
        {
            Logger.LogInfo("Escapists 2 Trainer loading...");

            _registry = new ModuleRegistry();
            _uiState = new TrainerUiState();

            var harmony = new Harmony("orchid.escapists2.trainer");
            
            // this actually applies the patches
            harmony.PatchAll(typeof(TrainerPlugin).Assembly);

            RegisterModules();
        }
        
        private void Update()
        {
            // toggles UI if the ui key is pressed
            if (_toggleUi.IsDown())
            {
                _showUi = !_showUi;
            }

            _registry.Keybinds.Update();
        }

        private void OnGUI()
        {
            if (!_showUi)
            {
                return;
            }

            _windowRect = GUILayout.Window(20401, _windowRect, DrawWindow, "Escapists 2 Trainer");
        }

        private void DrawWindow(int id)
        {
            GUILayout.BeginHorizontal();

            DrawLeftNav();
            DrawMainPanel();

            GUILayout.EndHorizontal();
            GUI.DragWindow(new Rect(0f, 0f, _windowRect.width, 22f));

            DrawModuleWindows();
        }

        private void DrawLeftNav()
        {
            GUILayout.BeginVertical(GUILayout.Width(260f));
            _navScroll = GUILayout.BeginScrollView(_navScroll, GUI.skin.box);

            foreach (ModuleCategory category in _registry.RootCategories)
            {
                DrawCategory(category, 0);
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void DrawCategory(ModuleCategory category, int indent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(indent * 12f);

            string caret = category.IsExpanded ? "▼" : "▶";
            bool expanded = GUILayout.Toggle(category.IsExpanded, caret + " " + category.Name, GUI.skin.button);
            if (expanded != category.IsExpanded)
            {
                category.IsExpanded = expanded;
            }

            GUILayout.EndHorizontal();

            if (!category.IsExpanded)
            {
                return;
            }

            foreach (ModuleCategory subCategory in category.Subcategories)
            {
                DrawCategory(subCategory, indent + 1);
            }

            foreach (ITrainerModule module in category.Modules)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space((indent + 1) * 12f);
                bool selected = _uiState.SelectedModule == module;
                GUIStyle style = selected ? GUI.skin.box : GUI.skin.button;

                if (GUILayout.Button(module.DisplayName, style))
                {
                    _uiState.SelectedModule = module;
                    _uiState.ActivePanel = ModulePanel.Main;
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawMainPanel()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            _mainScroll = GUILayout.BeginScrollView(_mainScroll);

            if (_uiState.SelectedModule != null)
            {
                DrawModuleHeader(_uiState.SelectedModule);

                if (_uiState.SelectedModule.HasSettings)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Toggle(_uiState.ActivePanel == ModulePanel.Main, "Main", GUI.skin.button))
                    {
                        _uiState.ActivePanel = ModulePanel.Main;
                    }

                    if (GUILayout.Toggle(_uiState.ActivePanel == ModulePanel.Settings, "Settings", GUI.skin.button))
                    {
                        _uiState.ActivePanel = ModulePanel.Settings;
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(6f);

                if (_uiState.ActivePanel == ModulePanel.Main)
                {
                    _uiState.SelectedModule.DrawMain();
                }
                else
                {
                    _uiState.SelectedModule.DrawSettings();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void DrawModuleHeader(ITrainerModule module)
        {
            GUILayout.Label(module.DisplayName, GUI.skin.label);

            if (!string.IsNullOrEmpty(module.Description))
            {
                GUILayout.Label(module.Description, GUI.skin.label);
            }

            if (module.SupportsToggle)
            {
                bool enabled = GUILayout.Toggle(module.IsEnabled, "Enabled", GUI.skin.toggle);
                if (enabled != module.IsEnabled)
                {
                    module.IsEnabled = enabled;
                }
            }
        }

        private void DrawModuleWindows()
        {
            if (_uiState.SelectedModule == null)
            {
                return;
            }

            ModuleWindow[] windows = _uiState.SelectedModule.GetWindows();
            if (windows == null || windows.Length == 0)
            {
                return;
            }

            for (int i = 0; i < windows.Length; i++)
            {
                ModuleWindow window = windows[i];
                window.Rect = GUILayout.Window(window.Id, window.Rect, window.Draw, window.Title);
            }
        }
        
        // where registered modules live.
        private void RegisterModules()
        {
            var timeFreeze = new TimeFreezeModule(_registry.Keybinds);
            _registry.RegisterModule(timeFreeze);

            var setTime = new SetTimeModule();
            _registry.RegisterModule(setTime);

            var placeholder = new TestModule("TestModule", "WARNING : does absolutely nothing ;).", new string[] { "Utilities", "Testing" });
            _registry.RegisterModule(placeholder);
        }
    }
}

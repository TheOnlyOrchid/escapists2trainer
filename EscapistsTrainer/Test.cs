using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace EscapistsTrainer
{
    [BepInPlugin("orchid.escapists2.timefreeze", "Time Freeze", "1.0.2")]
    public class TimeFreezePlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Time Freeze loading...");

            try
            {
                Harmony.DEBUG = true;
                var h = new Harmony("orchid.escapists2.timefreeze");
                h.PatchAll(typeof(TimeFreezePlugin).Assembly);
                Logger.LogInfo("Time Freeze loaded.");
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch]
    public static class Patch_ProcessTime
    {
        private static bool _logged;

        static MethodBase TargetMethod()
        {
            var t = AccessTools.TypeByName("RoutineManager");
            if (t == null)
            {
                Debug.LogError("[TimeFreeze] TypeByName('RoutineManager') returned null");
                return null;
            }

            var m = AccessTools.Method(t, "ProcessTime"); // private void ProcessTime()
            Debug.Log("[TimeFreeze] TargetMethod resolved: " + (m != null ? m.ToString() : "null"));
            return m;
        }

        [HarmonyPrefix]
        static bool Prefix(object __instance)
        {
            if (!_logged)
            {
                _logged = true;
                Debug.Log("[TimeFreeze] ProcessTime prefix HIT (time is being updated here)");
            }

            return false;
        }
    }
}
using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using WickerREST;

namespace WickerToolbox.Commands
{
    internal class RevealMap
    {
        [HarmonyPatch(typeof(FOWSystem), nameof(FOWSystem.IsExplored))]
        class Patch_FOWSystem_IsExplored
        {
            static bool Prefix(Vector3 pos, ref bool __result)
            {
                if (ActiveConfig.isRevealed)
                {
                    __result = true; // Set the result to true
                    return false; // Skip the original method
                }
                return true; // Continue with the original method
            }
        }

        [HarmonyPatch(typeof(FOWSystem), nameof(FOWSystem.IsVisible), new Type[] { typeof(Vector3) })]
        class Patch_FOWSystem_IsVisible
        {
            static bool Prefix(Vector3 pos, ref bool __result)
            {
                if (ActiveConfig.isRevealed)
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FOWSystem), nameof(FOWSystem.IsVisible), new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(int), typeof(int) })]
        class Patch_FOWSystem_IsVisible2
        {
            static bool Prefix(int sx, int sy, int fx, int fy, float outer, int sightHeight, int variance, ref bool __result)
            {
                if (ActiveConfig.isRevealed)
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        [CommandHandler("revealMap", "World")]
        public static void RevealMapHttp()
        {
            Processing.RevealMap();
        }

        static class Processing
        {
            public static void RevealMap()
            {
                if (!Common.IsInGame())
                {
                    WickerNetwork.Instance.LogResponse("Must be in-game to reveal map!");
                    return;
                }

                if (GameManager.Instance != null && GameManager.gameFullyInitialized)
                {
                    GameManager.Instance.cameraManager.fogOfWarEffect.mFog.enabled = false;
                    ActiveConfig.isRevealed = true;

                    var relicResources = UnityEngine.Object.FindObjectsOfType<RelicExtractionResource>();
                    foreach (var relic in relicResources)
                    {
                        relic.availableForExtraction = true;
                    }
                    WickerNetwork.Instance.LogResponse("Revealing map...");
                }
            }
        }
    }
}

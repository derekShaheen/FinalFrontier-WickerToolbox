using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using WickerREST;

namespace WickerToolbox.Commands
{
    internal class PlaceablePatch
    {
        [HarmonyPatch(typeof(Placeable), nameof(Placeable.IsPlacementValid))]
        class Patch_Placeable_IsPlacementValid
        {
            static bool Prefix(ref bool __result)
            {
                if (ActiveConfig.placeAnywhere)
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PlacementValidityHelper), nameof(PlacementValidityHelper.CanPathToPoint))]
        class Patch_PlacementValidityHelper_CanPathToPoint
        {
            static bool Prefix(ref bool __result)
            {
                if (ActiveConfig.placeAnywhere)
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(WagonShop), nameof(WagonShop.CanPathToWagon))]
        class Patch_WagonShop_CanPathToWagon
        {
            static bool Prefix(ref bool __result)
            {
                if (ActiveConfig.placeAnywhere)
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        [CommandHandler("ToggleBuildAnywhere", "World", "This command allows you to place buildings anywhere, even on invalid positions.")]
        public static void TogglePlaceAnywhereHttp()
        {
            Processing.PlaceAnywhere();
        }

        static class Processing
        {
            public static void PlaceAnywhere()
            {
                //if (!Common.IsInGame())
                //{
                //    WickerNetwork.Instance.LogResponse("Must be in-game to reveal map!");
                //    return;
                //}

                //if (GameManager.Instance != null && GameManager.gameFullyInitialized)
                //{
                //    GameManager.Instance.cameraManager.fogOfWarEffect.mFog.enabled = false;
                //    ActiveConfig.isRevealed = true;

                //    var relicResources = UnityEngine.Object.FindObjectsOfType<RelicExtractionResource>();
                //    foreach (var relic in relicResources)
                //    {
                //        relic.availableForExtraction = true;
                //    }
                //    WickerNetwork.Instance.LogResponse("Revealing map...");
                //}
                ActiveConfig.placeAnywhere = !ActiveConfig.placeAnywhere;

                WickerNetwork.Instance.LogResponse("Place Anywhere: " + ActiveConfig.placeAnywhere);
            }
        }
    }
}

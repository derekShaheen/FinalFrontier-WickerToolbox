//using HarmonyLib;
//using Il2Cpp;
//using MelonLoader;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WickerREST;

//namespace WickerToolbox.Commands
//{
//    internal class RNGController
//    {
//        [CommandHandler("RNGControl", "World", "Allows you to control the output of the random number generator.")]
//        public static void ToggleRNGControl()
//        {
//            Processing.RNGControl();
//        }

//        static class Processing
//        {
//            public static void RNGControl()
//            {
//                ActiveConfig.RNGControl = !ActiveConfig.RNGControl;

//                WickerNetwork.Instance.LogResponse(ActiveConfig.RNGControl ? "Random number generator control enabled." : "Random number generator control disabled.");
//            }
//        }

//        [HarmonyPatch(typeof(RNG))]
//        public class RNGPatch
//        {
//            [HarmonyPostfix]
//            [HarmonyPatch("Generate")]
//            public static void GeneratePostfix(ref uint __result)
//            {
//                if (ActiveConfig.RNGControl)
//                {
//                    MelonLogger.Msg("Original Generate result: " + __result);
//                    __result = 1;
//                }
//            }

//            [HarmonyPostfix]
//            [HarmonyPatch("GenerateFloat")]
//            public static void GenerateFloatPostfix(ref float __result)
//            {
//                if (ActiveConfig.RNGControl)
//                {
//                    MelonLogger.Msg("Original GenerateFloat result: " + __result);
//                    __result = 1.0f;
//                }
//            }

//            [HarmonyPostfix]
//            [HarmonyPatch("Range", new Type[] { typeof(float), typeof(float) })]
//            public static void RangeFloatPostfix(float min, float max, ref float __result)
//            {
//                if (ActiveConfig.RNGControl)
//                {
//                    MelonLogger.Msg($"Original Range(float, float) result: {__result} (min: {min}, max: {max})");
//                    __result = 1.0f;
//                }
//            }

//            [HarmonyPostfix]
//            [HarmonyPatch("Range", new Type[] { typeof(int), typeof(int) })]
//            public static void RangeIntPostfix(int min, int max, ref int __result)
//            {
//                if (ActiveConfig.RNGControl)
//                {
//                    MelonLogger.Msg($"Original Range(int, int) result: {__result} (min: {min}, max: {max})");
//                    __result = 1;
//                }
//            }
//        }
//    }
//}

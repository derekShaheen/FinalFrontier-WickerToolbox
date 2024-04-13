using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WickerREST;
using static Il2CppLibTerrain2.Terrain2Scene;

namespace WickerToolbox.Commands
{
    internal class CameraControl
    {
        [CommandHandler("ApplyCustomCamera"
            , "Camera"
            , "Apply custom camera settings based on applied fields."
            , "CustomCameraAutoComplete")]
        public static void ApplyCustomCameraHttp(float minDistance = 30f
                                        , float maxDistance = 300f
                                        , float minAngle = 10f
                                        , float maxAngle = 90f
                                        , bool renderFog = false)
        {
            Processing.CustomCamera(minDistance, maxDistance, minAngle, maxAngle, renderFog);
        }

        [CommandHandler("RestoreDefaultCamera"
                       , "Camera"
                       , "Restore default camera settings.")]
        public static void RestoreDefaultCameraHttp()
        {
            Processing.RestoreDefaultValues();
        }

        public static Dictionary<string, string[]> CustomCameraAutoComplete()
        {
            Dictionary<string, string[]> responseOptions = new Dictionary<string, string[]>();
            responseOptions["renderFog"] = new[] { "False", "True" };

            return responseOptions;
        }

        static class Processing
        {
            static bool customCamera = false;

            static float pminDistance = 0f;
            static float pmaxDistance = 0f;
            static float pminAngle = 0f;
            static float pmaxAngle = 0f;

            public static void CustomCamera(float minDistance = 30f
                                        , float maxDistance = 300f
                                        , float minAngle = 10f
                                        , float maxAngle = 90f
                                        , bool renderFog = false)
            {
                if (!Common.IsInGame())
                {
                    WickerNetwork.Instance.LogResponse("You must be in-game to use this command.");
                    return;
                }

                var camMan = GameManager.Instance.cameraManager;

                if (pminDistance == 0f) // Not yet captured defaults
                {
                    pminDistance = camMan.minDistanceFromTarget;
                    pmaxDistance = camMan.maxDistanceFromTarget;
                    pminAngle = camMan.minAngle;
                    pmaxAngle = camMan.maxAngle;
                }

                camMan.minAngle = minAngle;
                camMan.maxAngle = maxAngle;
                camMan.minDistanceFromTarget = minDistance;
                camMan.maxDistanceFromTarget = maxDistance;
                RenderSettings.fog = renderFog;
                customCamera = true;

                WickerNetwork.Instance.LogResponse("Custom camera settings applied.");
            }

            public static void RestoreDefaultValues()
            {
                if (!Common.IsInGame())
                {
                    WickerNetwork.Instance.LogResponse("You must be in-game to use this command.");
                    return;
                }

                if (!customCamera || pminAngle == 0)
                {
                    WickerNetwork.Instance.LogResponse("Default settings restored.");
                    return;
                }

                var camMan = GameManager.Instance.cameraManager;

                camMan.minAngle = pminAngle;
                camMan.maxAngle = pmaxAngle;
                camMan.minDistanceFromTarget = pminDistance;
                camMan.maxDistanceFromTarget = pmaxDistance;
                RenderSettings.fog = true;
                customCamera = false;

                WickerNetwork.Instance.LogResponse("Default settings restored.");
            }
        }
    }
}

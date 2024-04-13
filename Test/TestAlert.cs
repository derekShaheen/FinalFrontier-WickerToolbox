//using Il2Cpp;
//using Il2CppSystem;
//using UnityEngine;
//using WickerREST;

//namespace WickerToolbox.Test
//{
//    internal class TestAlert
//    {
//        [CommandHandler("TestAlert", "Alert", "Test alert.")]
//        public static void TestAlertHttp(string InputStr = "Test Alert")
//        {
//            Processing.TestAlert(InputStr);
//        }

//        static class Processing
//        {

//            public static void TestAlert(string InputStr)
//            {
//                WickerNetwork.Instance.LogResponse("Test Alert Start");
//                var rayHit = GameManager.Instance.terrainManager.GetTerrainWorldPointUnderCursor(out Vector3 point);
//                if (!rayHit)
//                {
//                    WickerNetwork.Instance.LogResponse("No cursor position found.");
//                    return;
//                }

//                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                cube.transform.position = point;
//                // Find UIEventOverview gameobject
//                //GameManager.Instance.uiManager.ShowMiscInfoText(Color.red, InputStr);
//                //GameManager.Instance.uiManager.windowManager.debugAlertWindow.Init(InputStr, GameManager.Instance.gameObject, null, "Test", false);
//                GameManager.Instance.uiManager.modalWindowManager.ShowModalWindow(
//                    new ModalWindowData("Test Title", "Test Window", "TYes", "TNo", null, null, null));
//                var hintSettings = new UIHintMessageSettings();
//                hintSettings.message = InputStr;
//                hintSettings.displayTime = 5f;
//                var hint = new UIHintMessageEntry(HintType.Arborist, hintSettings, null);
//                hint.message = InputStr;
//                hint.displayTime = 5f;
//                hint.priority = 99;
//                GameManager.Instance.hintManager.AddMessageEntry(hint);
//                GameManager.Instance.hintManager.OnCriteriaMetChanged(hint);
//                WickerNetwork.Instance.LogResponse("Test Alert End");
//            }
//        }
//    }
//}

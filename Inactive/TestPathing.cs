//using Il2Cpp;
//using Il2CppAIPathfinding;
//using UnityEngine;
//using WickerREST;

//namespace WickerToolbox.Commands
//{
//    internal class TestPathing
//    {
//        //[CommandHandler("TestPath", "Pathing", "Enables pathing debug.")]
//        public static void TestPath()
//        {
//            //if(GameManager.Instance == null)
//            //{
//            //    WickerNetwork.Instance.LogResponse("GameManager is null!");
//            //    return;
//            //}

//            //if(GameManager.Instance.inputManager.selectedObject == null)
//            //{
//            //    WickerNetwork.Instance.LogResponse("No object selected!");
//            //    return;
//            //}

//            //AIPathfinder aiPathfinder = GameManager.Instance.aiPathfinder;
//            //var nodes = aiPathfinder.GetPathCheckGridNodeForObject(GameManager.Instance.inputManager.selectedObject
//            //            , AIGridGraph.FloodFillType.WallsBlock, true, out bool IsOccupantAtNode);
//            //Villager villager = GameManager.Instance.inputManager.selectedObject.GetComponent<Villager>();
            
//            //if (villager == null)
//            //{
//            //    WickerNetwork.Instance.LogResponse("Villager not found!");
//            //}

//            //IPlaceOfWork placeOfWork = villager.placeOfWork;
//            //if (placeOfWork == null)
//            //{
//            //    WickerNetwork.Instance.LogResponse("Place of work not found!");
//            //}

//            //GameObject placeOfWorkGO = placeOfWork.Cast<IRegistersForWork>().gameObject;

//            //var nodesDest = aiPathfinder.GetPathCheckGridNodeForObject(placeOfWorkGO
//            //            , AIGridGraph.FloodFillType.WallsBlock, true, out bool IsOccupantAtNodeOut);

//            AIGridGraph.Instance.displayGridGizmos = true;
//            WickerNetwork.Instance.LogResponse("Pathing debug enabled!");
//        }

//        //Create a command to set debugPathStartObj in aiPathFinder to selected object
//        //[CommandHandler("SetPathStart", "Pathing", "Sets the start object for pathing debug.")]
//        public static void SetPathStart()
//        {
//            if(GameManager.Instance == null)
//            {
//                WickerNetwork.Instance.LogResponse("GameManager is null!");
//                return;
//            }
//            AIPathfinder aiPathfinder = GameManager.Instance.aiPathfinder;
//            aiPathfinder.debugPathStartObj = GameManager.Instance.inputManager.selectedObject;
//            WickerNetwork.Instance.LogResponse("Pathing debug start object set!");
//        }

//        //Now do end object
//        //[CommandHandler("SetPathEnd", "Pathing", "Sets the end object for pathing debug.")]
//        public static void SetPathEnd()
//        {
//            if(GameManager.Instance == null)
//            {
//                WickerNetwork.Instance.LogResponse("GameManager is null!");
//                return;
//            }
//            AIPathfinder aiPathfinder = GameManager.Instance.aiPathfinder;
//            aiPathfinder.debugPathEndObj = GameManager.Instance.inputManager.selectedObject;
//            WickerNetwork.Instance.LogResponse("Pathing debug end object set!");
//        }
//    }
//}

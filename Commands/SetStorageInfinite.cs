using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WickerREST;

namespace WickerToolbox.Commands
{
    internal class SetStorageInfinite
    {
        [GameVariable("IsSelectedInvInfinite")]
        public static string GetIsSelectedInvInfinite()
        {
            return Processing.IsSelectedStorageInf().ToString();
        }

        [CommandHandler("ToggleStorageInfinite", "Items", "Select a building in game, this will toggle it's storage being infinite. Note there is nothing on screen to indicate this. It effectively freezes the inventory.")]
        public static void SetSelectedStorageInfiniteHttp()
        {
            Processing.ToggleInfiniteStorage();
        }

        static class Processing
        {
            public static bool IsSelectedStorageInf()
            {
                if (!Common.IsInGame())
                {
                    return false;
                }

                GameObject selectedBuilding = GameManager.Instance.inputManager.selectedObject;
                if (selectedBuilding == null)
                {
                    return false;
                }

                ReservableItemStorage storage = selectedBuilding.GetComponent<ReservableItemStorage>();
                if (storage == null)
                {
                    return false;
                }

                return storage.infiniteItems;
            }

            public static void ToggleInfiniteStorage()
            {
                if (!Common.IsInGame())
                {
                    WickerNetwork.Instance.LogResponse("Must be in-game to add items!");
                    return;
                }

                GameObject selectedBuilding = GameManager.Instance.inputManager.selectedObject;
                if (selectedBuilding == null)
                {
                    WickerNetwork.Instance.LogResponse("Must select a building to add items!");
                    return;
                }

                ReservableItemStorage storage = selectedBuilding.GetComponent<ReservableItemStorage>();
                if (storage == null)
                {
                    WickerNetwork.Instance.LogResponse("Selected object does not have a storage!");
                    return;
                }

                storage.infiniteItems = !storage.infiniteItems;

                WickerNetwork.Instance.LogResponse($"Infinite storage for {selectedBuilding.name} is now {storage.infiniteItems}. {storage.transform.position}");
            }
        }
    }
}

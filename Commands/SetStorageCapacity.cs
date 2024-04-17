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
    internal class SetStorageCapacity
    {
        [CommandHandler("SetStorageCapacity", "Items", "Select a building in game, this will allow you to set the storage capacity of the selection.")]
        public static void SetStorageCapacityHttp(int newCapacity)
        {
            Processing.SetStorageCapacity(newCapacity);
        }

        static class Processing
        {
            public static void SetStorageCapacity(int newCapacity)
            {
                if (!Common.IsInGame())
                {
                    WickerNetwork.Instance.LogResponse(message: "Must be in-game to set storage capacity!");
                    return;
                }

                GameObject selectedObj = GameManager.Instance.inputManager.selectedObject;
                if (selectedObj == null)
                {
                    WickerNetwork.Instance.LogResponse("Must select a building to set storage capacity!");
                    return;
                }

                ReservableItemStorage storage = selectedObj.GetComponent<ReservableItemStorage>();
                if (storage == null)
                {
                    WickerNetwork.Instance.LogResponse("Selected object does not have a storage!");
                    return;
                }
                bool set = false;
                int capacity = 0;
                ItemStorage itemStorage = storage.itemStorage;
                if (itemStorage != null)
                {
                    capacity = (int)itemStorage.carryCapacity;

                    itemStorage.carryCapacity = newCapacity;
                    set = true;
                }

                StorageBuilding storageBuilding = selectedObj.GetComponent<StorageBuilding>();
                if (storageBuilding != null)
                {
                   capacity = storageBuilding.storageItemCountCapacity;
                    storageBuilding.storageItemCountCapacity = newCapacity;
                    set = true;
                }

                if (set)
                {
                    WickerNetwork.Instance.LogResponse($"Set storage capacity to {newCapacity} from {capacity} for {selectedObj.name}.");
                }
                else
                {
                    WickerNetwork.Instance.LogResponse("Selected object does not have a storage!");
                }
            }
        }
    }
}

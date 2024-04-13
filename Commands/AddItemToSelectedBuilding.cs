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
    internal class AddItemToSelectedBuilding
    {

        [CommandHandler("AddItemToSelectedBuilding", "Items", "Select a building in game, then use this to add item to it's storage.", "AddItemAutoComplete")]
        public static void AddItemHttp(string itemID = "GoldIngot", string count = "1")
        {
            Processing.AddItem(itemID, count);
        }

        public static Dictionary<string, string[]> AddItemAutoComplete()
        {
            Dictionary<string, string[]> outOptionsDict = new Dictionary<string, string[]>();
            //List<string> itemOptions = new List<string>();
            //foreach (var item in Item.itemDict)
            //{
            //    if (item.value.item.itemID.Equals("Work"))
            //    {
            //        continue;
            //    }
            //    itemOptions.Add(item.value.item.itemID.ToString());
            //}
            List<string> outOptions = new List<string>(Enum.GetNames(typeof(ItemID)));
            outOptions.Remove("Work");
            outOptions.Remove("MAX");
            outOptions.Remove("None");
            outOptions.Remove("Unassigned");
            outOptionsDict["itemID"] = outOptions.ToArray();
            outOptionsDict["itemID"] = outOptionsDict["itemID"].OrderBy(x => x).ToArray();

            return outOptionsDict;
        }

        static class Processing
        {
            private static string cachedAssemblyName = string.Empty;

            public static void AddItem(string itemID, string count = "1")
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

                Building building = selectedBuilding.GetComponent<Building>();
                if (building == null)
                {
                    WickerNetwork.Instance.LogResponse("Selected object is not a building!");
                    return;
                }

                // Initialize itemType to null
                Type itemType = null;

                if (!string.IsNullOrEmpty(cachedAssemblyName))
                {
                    var cachedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == cachedAssemblyName);
                    if (cachedAssembly != null)
                    {
                        itemType = cachedAssembly.GetType("Item" + itemID);
                    }
                }

                // If itemType is still null, search through all loaded assemblies
                if (itemType == null)
                {
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        itemType = assembly.GetType("Il2CPP.Item" + itemID, false, true);
                        if (itemType != null)
                        {
                            // Cache the assembly name for future searches
                            cachedAssemblyName = assembly.GetName().Name;
                            break;
                        }
                    }
                }

                if (itemType == null)
                {
                    WickerNetwork.Instance.LogResponse($"Item '{itemID}' not found.");
                    return;
                }

                object itemInstance = Activator.CreateInstance(itemType);

                if (itemInstance == null || !(itemInstance is Item))
                {
                    WickerNetwork.Instance.LogResponse($"Failed to create of '{itemType}'.");
                    return;
                }

                Item item = (Item)itemInstance;
                uint itemCount;
                if (!uint.TryParse(count, out itemCount))
                {
                    WickerNetwork.Instance.LogResponse($"Invalid item count '{count}'.");
                    return;
                }

                //Limit item count to 9999 and send response if higher
                if (itemCount > 9999)
                {
                    WickerNetwork.Instance.LogResponse($"Item count '{itemCount}' is too high. Limit is 9999.");
                    return;
                }

                ItemBundle itemBundle = new ItemBundle(item, itemCount, 100U);
                building.storage.AddItems(itemBundle);

                WickerNetwork.Instance.LogResponse($"Added {count} of {itemID} to {selectedBuilding.name}.");
            }
        }
    }
}

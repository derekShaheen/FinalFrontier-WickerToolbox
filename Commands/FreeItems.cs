using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WickerREST;

namespace WickerToolbox.Commands
{
    internal class FreeItems
    {

        // Dictionary to store original prices
        private static Dictionary<string, uint> originalPrices = new Dictionary<string, uint>();
        private static bool itemsAreFree = false;

        [CommandHandler("FreeItems", "Market", "Set the price of items at <i>new</i> traders to 0. Select again to restore prices.")]
        public static void ToggleItemsFree()
        {
            // Toggle state
            itemsAreFree = !itemsAreFree;

            foreach (var item in Item.itemDict)
            {
                if (itemsAreFree)
                {
                    // If making items free, store original price and set to 0
                    if (!originalPrices.ContainsKey(item.key))
                    {
                        originalPrices[item.key] = item.value.item.basePrice;
                    }
                    item.value.item.basePrice = 0;
                }
                else
                {
                    // If restoring prices, restore from stored original prices
                    if (originalPrices.ContainsKey(item.key))
                    {
                        item.value.item.basePrice = originalPrices[item.key];
                    }
                }
            }

            if (itemsAreFree)
            {
                WickerNetwork.Instance.LogResponse("Set all items base price to 0. Affects new traders.");
            }
            else
            {
                WickerNetwork.Instance.LogResponse("Restored all items to their original base price. Affects new traders.");
            }
        }
    }
}

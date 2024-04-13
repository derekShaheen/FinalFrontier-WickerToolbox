using Il2Cpp;
using WickerREST;

namespace WickerToolbox.Commands
{
    internal class ListAllItems
    {
        //[CommandHandler("ListAllItems", "Utilities")]
        public static void ListAllItemsHttp()
        {
            // Iterate through all items, append name and description to response
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"");
            foreach (var item in Item.itemDict)
            {
                sb.AppendLine($"Item: '{item.value.item.itemID}'");
            }
            WickerNetwork.Instance.LogResponse(sb.ToString());
        }
    }
}

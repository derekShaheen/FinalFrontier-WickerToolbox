using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WickerREST;

namespace WickerToolbox.Commands
{
    internal class ReRollTradeModifiers
    {

        [CommandHandler("RerollTradeModifiers", "Market", "If you have an active trader, you can re-roll the trade modifiers. Close and re-open the market to see the change.")]
        public static void RerollTradeModifiersHTTP()
        {
            if (!Common.IsInGame())
            {
                WickerNetwork.Instance.LogResponse("Must be in-game to reroll trade modifiers!");
                return;
            }
            try
            {
                GameManager.Instance.tradeManager.RollPriceModifiers();
            }
            catch (System.Exception e)
            {
                WickerNetwork.Instance.LogResponse($"Error: {e.Message}");
                return;
            }
            WickerNetwork.Instance.LogResponse("Trade modifiers rerolled.");
        }
    }
}

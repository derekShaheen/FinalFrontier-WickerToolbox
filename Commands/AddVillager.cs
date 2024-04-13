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
    internal class AddVillager
    {
        [CommandHandler("AddVillager", "Village", "Begin immigration of villagers.")]
        public static void AddVillagerHttp(string Count = "1")
        {
            if (!Common.IsInGame())
            {
                WickerNetwork.Instance.LogResponse("Must be in-game to add villagers!");
                return;
            }
            var cheatManager = UnityEngine.Object.FindObjectOfType<CheatManager>();
            if (cheatManager == null)
            {
                cheatManager = GameObject.Instantiate(new GameObject()).AddComponent<CheatManager>();
            }

            cheatManager.AddVillagers(int.Parse(Count));
            cheatManager.StartImmigration();
            WickerNetwork.Instance.LogResponse($"Immigrating {Count} villagers");
        }
    }
}

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
    internal class SetMaxWorkers
    {
        [CommandHandler("SetMaxWorkersForBuilding", "Village", "Allows you to set the max number of workers for a selected building. NOTE: This only stays for the session and will be reset upon load.")]
        public static void SetMaxWorkersHttp(string Count = "1")
        {
            if (!Common.IsInGame())
            {
                WickerNetwork.Instance.LogResponse("Must be in-game to set max workers!");
                return;
            }
            try
            {
                Processing.SetMaxWorkers(int.Parse(Count));
            }
            catch (Exception e)
            {
                WickerNetwork.Instance.LogResponse($"Error: {e.Message}");
                return;
            }
        }

        static class Processing
        {

            public static void SetMaxWorkers(int maxWorkers)
            {
                if (!Common.IsInGame())
                {
                    WickerNetwork.Instance.LogResponse("Must be in-game to set max workers!");
                    return;
                }

                GameObject selectedBuilding = GameManager.Instance.inputManager.selectedObject;
                if (selectedBuilding == null)
                {
                    WickerNetwork.Instance.LogResponse("Must select a building to set max workers!");
                    return;
                }

                if (maxWorkers > 26)
                {
                    maxWorkers = 26;
                    WickerNetwork.Instance.LogResponse("Max workers that can be set is 26! Setting request to 26.");
                }

                Building building = selectedBuilding.GetComponent<Building>();
                if (building != null)
                {
                    int tempMaxWorkers = selectedBuilding.GetComponent<Building>().maxWorkers;
                    selectedBuilding.GetComponent<Building>().maxWorkers = maxWorkers;
                    WickerNetwork.Instance.LogResponse($"Set max workers for {selectedBuilding.name} to {maxWorkers}.");
                    return;
                }

                Cropfield Crops = selectedBuilding.GetComponent<Cropfield>();
                if (Crops != null)
                {
                    int tempMaxWorkers = Crops.GetComponent<Cropfield>().maxWorkers;
                    Crops.GetComponent<Cropfield>().maxWorkers = maxWorkers;
                    WickerNetwork.Instance.LogResponse($"Set max workers for {selectedBuilding.name} to {maxWorkers}.");
                    return;
                }

                Resource ResoucePlot = selectedBuilding.GetComponent<Resource>();
                if (ResoucePlot != null)
                {
                    int tempMaxWorkers = ResoucePlot.GetComponent<Resource>().maxWorkers;
                    ResoucePlot.GetComponent<Resource>().maxWorkers = maxWorkers;
                    WickerNetwork.Instance.LogResponse($"Set max workers for {selectedBuilding.name} to {maxWorkers}.");
                    return;
                }

                WickerNetwork.Instance.LogResponse($"Could not find a component to set max worker. Object: {selectedBuilding.name}");
            }
        }
    }
}

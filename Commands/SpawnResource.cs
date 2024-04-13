using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WickerToolbox;
using WickerREST;
using UnityEngine;

namespace WickerToolbox.Commands
{
    internal class SpawnResource
    {
        // Define a delegate for the SpawnResource action
        public delegate void SpawnResourceAction(string resource, string isDeep);

        // Store the last command parameters for hotkey execution
        public static SpawnResourceAction? lastSpawnResourceAction;
        public static string? lastResource;
        public static string? lastIsDeep;

        [CommandHandler("SpawnResource", "World", "Spawns a resource under the cursor with hotkey.", "SpawnResourceAutoComplete")]
        public static void RegisterSpawnResourceCommand(string resource = "Gold", string isDeep = "False")
        {
            ToolboxREST.UnsetActions();
            // Store the parameters and the action to be called on hotkey press
            lastResource = resource;
            lastIsDeep = isDeep;
            lastSpawnResourceAction = SpawnResourceHttp;
            WickerNetwork.Instance.LogResponse($"Settings applied. Press CTRL+Home (default) in-game to spawn under cursor." +
                $"\nResource: {resource}" +
                $"\nIsDeep: {isDeep}");
        }

        public static void SpawnResourceHttp(string resource = "Gold", string isDeep = "False")
        {
            bool isValidMineral = Enum.TryParse<MineralSite.MineralType>(resource, true, out var mineralType);

            // Check if the resource is one of the special cases or a valid mineral type
            if (!isValidMineral && !resource.Equals("Clay", StringComparison.InvariantCultureIgnoreCase)
                && !resource.Equals("Stone", StringComparison.InvariantCultureIgnoreCase)
                && !resource.Equals("Sand", StringComparison.InvariantCultureIgnoreCase))
            {
                WickerNetwork.Instance.LogResponse($"Invalid resource type '{resource}'.");
                return;
            }

            // Validate isDeep boolean
            if (!bool.TryParse(isDeep, out var isDeepBool))
            {
                WickerNetwork.Instance.LogResponse($"Invalid isDeep value '{isDeep}'.");
                return;
            }

            var rayHit = GameManager.Instance.terrainManager.GetTerrainWorldPointUnderCursor(out Vector3 point);
            if (!rayHit)
            {
                WickerNetwork.Instance.LogResponse("No cursor position found.");
                return;
            }

            WickerNetwork.Instance.LogResponse(Processing.SpawnMineralSite(point, resource, isDeepBool));
        }

        public static Dictionary<string, string[]> SpawnResourceAutoComplete()
        {
            List<string> resourceOptions = new List<string>(Enum.GetNames(typeof(MineralSite.MineralType)));
            resourceOptions.Add("Sand");
            resourceOptions.Add("Clay");
            resourceOptions.Add("Stone");
            resourceOptions.Remove("Max");
            Dictionary<string, string[]> responseOptions = new Dictionary<string, string[]>();
            responseOptions["resource"] = resourceOptions.ToArray();

            // Sort the response options by value
            responseOptions["resource"] = responseOptions["resource"].OrderBy(x => x).ToArray();

            responseOptions["isDeep"] = new[] { "False", "True" };

            return responseOptions;
        }

        static class Processing
        {

            public static string SpawnMineralSite(Vector3 location, string resourceName, bool isDeep)
            {
                if (!Common.IsInGame())
                {
                    return "Must be in-game to spawn mineral sites!";
                }

                float radius = 20f; // Assuming default radius
                System.Random rand = new System.Random();
                int mineralCount = isDeep ? int.MaxValue : rand.Next(4000, 6500);
                if (Enum.TryParse<MineralSite.MineralType>(resourceName, true, out var mineralType))
                {
                    // Handle MineralSite types
                    int newId = GameManager.Instance.mineralManager.mineralSites.Count + 1;
                    MineralSite newMineralSite = new MineralSite(newId, mineralType, location, radius, mineralCount);
                    GameManager.Instance.mineralManager.mineralSites.Add(newMineralSite);
                    GameManager.Instance.mineralManager.CreateMineralSitePrefab(mineralType, location, radius);
                    MineralSitePrefabData prefabData = new MineralSitePrefabData(mineralType, location, 20f);
                    GameManager.Instance.mineralManager.mineralSitePrefabData.Add(prefabData);
                    if (isDeep)
                    {
                        Il2CppSystem.Collections.Generic.List<MineralSite> newSitesList = new Il2CppSystem.Collections.Generic.List<MineralSite>();
                        newSitesList.Add(newMineralSite);
                        GameManager.Instance.mineralManager.CreateDeepMineralDeposits(1, newSitesList, true);
                    }
                }
                else
                {
                    // Handle non-MineralSite types (Sand, Clay, Stone)
                    GameObject sitePrefab;
                    switch (resourceName.ToLower())
                    {
                        case "sand":
                            sitePrefab = GameManager.Instance.mineralManager.CreateSandSitePrefab(location, radius, mineralCount);
                            var sandSite = new SandSite(sitePrefab, location, radius, mineralCount);
                            if (isDeep) sitePrefab.GetComponent<SandPitResource>().storage.itemStorage.infiniteItems = true;
                            GameManager.Instance.mineralManager.sandSites.Add(sandSite);
                            break;
                        case "clay":
                            sitePrefab = GameManager.Instance.mineralManager.CreateClaySitePrefab(location, radius, mineralCount);
                            var claySite = new ClaySite(sitePrefab, location, radius, mineralCount);
                            if (isDeep) sitePrefab.GetComponent<ClayPitResource>().storage.itemStorage.infiniteItems = true;
                            GameManager.Instance.mineralManager.claySites.Add(claySite);
                            break;
                        case "stone":
                            sitePrefab = GameManager.Instance.mineralManager.CreateStoneSitePrefab(location, radius, mineralCount);
                            var stoneSite = new StoneSite(sitePrefab, location, radius, mineralCount);
                            if (isDeep) sitePrefab.GetComponent<StonePitResource>().storage.itemStorage.infiniteItems = true;
                            GameManager.Instance.mineralManager.stoneSites.Add(stoneSite);
                            break;
                        default:
                            return $"Invalid resource type '{resourceName}'.";
                            //break;
                    }
                }
                return $"Spawned {(isDeep ? "Deep " : "")}{resourceName} at {location}";
            }
        }
    }
}

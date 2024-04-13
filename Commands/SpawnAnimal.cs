using Il2Cpp;
using UnityEngine;
using WickerREST;

namespace WickerToolbox.Commands
{
    internal class SpawnAnimal
    {
        // Define a delegate for the SpawnAnimal action
        public delegate void SpawnAnimalAction(string animal, string count);

        public static SpawnAnimalAction? lastSpawnAnimalAction;
        public static string? lastAnimal;
        public static string? lastCount;


        //[CommandHandler("SpawnAnimal", "Utilities", "Spawns a animal under the cursor with hotkey.", "SpawnAnimalAutoComplete")]
        public static void RegisterSpawnAnimalCommand(string animal = "Deer", string count = "5")
        {
            // Store the parameters and the action to be called on hotkey press
            lastAnimal = animal;
            lastCount = count;
            lastSpawnAnimalAction = SpawnAnimalHttp;
            WickerNetwork.Instance.LogResponse($"Settings applied. Press CTRL+Insert (default) in-game to spawn under cursor." +
                $"\nAnimal: {animal}" +
                $"\nCount: {count}");
        }

        public static void SpawnAnimalHttp(string animal = "Deer", string count = "5")
        {
            // Check if the resource is one of the special cases or a valid mineral type
            if (!animal.Equals("Deer", StringComparison.InvariantCultureIgnoreCase)
                && !animal.Equals("Bear", StringComparison.InvariantCultureIgnoreCase)
                && !animal.Equals("Boar", StringComparison.InvariantCultureIgnoreCase)
                && !animal.Equals("Wolf", StringComparison.InvariantCultureIgnoreCase))
            {
                WickerNetwork.Instance.LogResponse($"Invalid animal type '{animal}'.");
                return;
            }

            // Validate count
            if (!int.TryParse(count, out var countInt))
            {
                WickerNetwork.Instance.LogResponse($"Invalid count value '{count}'.");
                return;
            }

            // Verify count is positive
            if (countInt <= 0)
            {
                WickerNetwork.Instance.LogResponse("Count must be greater than 0.");
                return;
            }

            var rayHit = GameManager.Instance.terrainManager.GetTerrainWorldPointUnderCursor(out Vector3 point);
            if (!rayHit)
            {
                WickerNetwork.Instance.LogResponse("No cursor position found.");
                return;
            }

            WickerNetwork.Instance.LogResponse(Processing.SpawnAnimal(point, animal, countInt));
        }

        public static Dictionary<string, string[]> SpawnAnimalAutoComplete()
        {
            List<string> outOptions = new List<string>(Enum.GetNames(typeof(AnimalGroupDefinition.AnimalType)));
            outOptions.Remove("_Count"); // Remove the _Count enum value (not a valid animal type)
            outOptions.Remove("None"); // Remove the None enum value (not a valid animal type)

            Dictionary<string, string[]> outOptionsDict = new Dictionary<string, string[]>();
            outOptionsDict["animal"] = outOptions.ToArray();

            // Sort the response options by value
            outOptionsDict["animal"] = outOptionsDict["animal"].OrderBy(x => x).ToArray();
            return outOptionsDict;
        }

        static class Processing
        {

            public static string SpawnAnimal(Vector3 location, string animal, int count)
            {
                if (!Common.IsInGame())
                {
                    return "Must be in-game to spawn animals!";
                }

                switch (animal.ToLower())
                {
                    case "deer":
                        GameManager.Instance.animalManager.DebugSpawnDeerAtPoint(count, location);
                        break;
                    case "bear":
                        GameManager.Instance.animalManager.DebugSpawnBearsAtPoint(count, location);
                        break;
                    case "boar":
                        GameManager.Instance.animalManager.DebugSpawnBoarsAtPoint(count, location);
                        break;
                    case "wolf":
                        GameManager.Instance.animalManager.DebugSpawnWolvesAtPoint(count, location);
                        break;
                    default:
                        return $"Invalid animal type '{animal}'.";
                        //break;
                }
                return $"Spawned {count} {animal} at {location}";
            }
        }
    }
}

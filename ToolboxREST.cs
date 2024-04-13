using Il2Cpp;
using MelonLoader;
using MelonLoader.Utils;
using System.Collections;
using UnityEngine;
using WickerREST;
using WickerToolbox.Commands;

namespace WickerToolbox
{
    public static class BuildInfo
    {
        public const string Name = "WickerToolbox";
        public const string Description = "Toolbox for Farthest Frontier using REST";
        public const string Author = "Skrip";
        public const string Version = "1.0.6";
        public const string DownloadLink = "";
    }

    public static class ActiveConfig
    {
        public static bool isRevealed = false;
        public static bool isPlaying = false;
        public static bool placeAnywhere = false;
        public static bool RNGControl = false;
    }

    public class ToolboxREST : MelonMod
    {
        //public static int HighestUsage = 0;

        //private Dictionary<string, List<Vector3>> lastPositions = new Dictionary<string, List<Vector3>>();

        //private List<Node> nodes = new List<Node>();
        //public float nodeCreationThreshold = 6f; // Distance in units to create a new node
        //bool isTracking = false;


        private MelonPreferences_Category? modCategory;
        private MelonPreferences_Entry<KeyCode>? activationKey;

        //Singleton
        public static ToolboxREST? Instance;

        public static ToolboxREST GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new ToolboxREST();
                }
                return Instance;
            }
            set
            {
                Instance = value;
            }
        }

        public override void OnInitializeMelon()
        {
            Instance = this;
            HarmonyInstance.PatchAll();
            modCategory = MelonPreferences.CreateCategory("Toolbox");
            modCategory.SetFilePath(Path.Combine(MelonEnvironment.UserDataDirectory, "Toolbox.cfg"));
            activationKey = modCategory.CreateEntry("ActivationKey", KeyCode.Home, description: "Key for function activation");

            if (activationKey == null)
            {
                LoggerInstance.Msg("Please set a key for the function activation in the mod settings.");
            }
            else
            {
                activationKey.Value = KeyCode.Home;
            }

            LoggerInstance.Msg($"Activation key is CTRL + {activationKey?.Value}");
            MelonLoader.MelonCoroutines.Start(WaitForHotkeyPress());
        }

        private IEnumerator WaitForHotkeyPress()
        {
            while (true)
            {
                if (UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(KeyCode.RightControl))
                {
                    // Check if the hotkey is pressed
                    if (UnityEngine.Input.GetKeyDown(activationKey.Value))
                    {
                        if (SpawnResource.lastSpawnResourceAction != null)
                        {
                            SpawnResource.lastSpawnResourceAction.Invoke(SpawnResource.lastResource, SpawnResource.lastIsDeep);
                        }
                        if (SpawnAnimal.lastSpawnAnimalAction != null)
                        {
                            SpawnAnimal.lastSpawnAnimalAction.Invoke(SpawnAnimal.lastAnimal, SpawnAnimal.lastCount);
                        }

                        //isTracking = !isTracking;
                        //ToggleVisualization();
                        //LoggerInstance.Msg($"Set {isTracking}");
                    }
                }
                yield return null;
            }
        }

        //public void ToggleVisualization()
        //{
        //    CameraManager camMan = GameManager.Instance.cameraManager;
        //    if (isTracking)
        //    {
        //        camMan.minFieldOfView = 22f;
        //        camMan.maxFieldOfView = 65f;
        //        camMan.minAngle = 10f;
        //        camMan.maxAngle = 90f;
        //        camMan.minDistanceFromTarget = 30f;
        //        camMan.maxDistanceFromTarget = 300f;
        //        camMan.shadowDistMin = 200f;
        //        camMan.shadowDistMax = 450f;
        //        RenderSettings.fog = false;
        //    }
        //    else
        //    {
        //        foreach (Node node in nodes)
        //        {
        //            GameObject.Destroy(node.Cube);
        //        }
        //        nodes.Clear();
        //        HighestUsage = 1;
        //    }
        //}

        public override void OnUpdate()
        {
            //if (isTracking)
            //{
            //    for (int i = nodes.Count - 1; i >= 0; i--)
            //    {
            //        nodes[i].DecreaseUsage();
            //        if (nodes[i].Usage <= 0)
            //        {
            //            // Remove the node if it's no longer used
            //            GameObject.Destroy(nodes[i].Cube); // Assuming cube is named or you have a reference to it
            //            nodes.RemoveAt(i);
            //        }
            //    }

            //    Il2CppSystem.Collections.Generic.List<Villager> villagers = GameManager.Instance.resourceManager.villagers;
            //    foreach (Villager villager in villagers)
            //    {
            //        if (villager == null || villager.gameObject == null)
            //        {
            //            continue;
            //        }

            //        string villagerName = villager.gameObject.name;
            //        Vector3 currentPos = villager.gameObject.transform.position;

            //        if (!lastPositions.ContainsKey(villagerName))
            //        {
            //            lastPositions[villagerName] = new List<Vector3> { currentPos };
            //        }
            //        else
            //        {
            //            Vector3 lastPos = lastPositions[villagerName].LastOrDefault(); // Ensure System.Linq is used
            //            float distanceMoved = Vector3.Distance(currentPos, lastPos);

            //            if (distanceMoved >= nodeCreationThreshold)
            //            {
            //                lastPositions[villagerName].Add(currentPos);
            //                CreateOrUpdateNode(villager);
            //            }
            //        }
            //    }
            //}
        }

        //    [CommandHandler("SaveNodesHtmlToFile", "Test", "Saves the current nodes to an HTML file. A:\\\\Games\\\\Steam\\\\steamapps\\\\common\\\\Farthest Frontier\\\\output.html")]
        //    public static void SaveNodesHtmlToFile(string path)
        //    {
        //        if(path.Equals(string.Empty))
        //        {
        //            path = "A:\\Games\\Steam\\steamapps\\common\\Farthest Frontier\\output.html";
        //        }
        //        MelonLogger.Msg($"Saving nodes to HTML file... {ToolboxREST.GetInstance.GetNodes().Count}");
        //        string htmlContent = ToolboxREST.GetInstance.GenerateNodesHtml();

        //        if(File.Exists(path))
        //        {
        //            File.Delete(path);
        //        }

        //        System.IO.File.WriteAllText(path, htmlContent);
        //        WickerNetwork.Instance.LogResponse($"Nodes saved to <a src=\"{path}\">{path}</a>");
        //    }

        //    public string GenerateNodesHtml()
        //    {
        //        var htmlBuilder = new System.Text.StringBuilder();

        //        if (nodes.Count == 0) return "";

        //        // Find minimum x and z to normalize positions
        //        float minX = nodes.Min(node => node.Position.x);
        //        float minZ = nodes.Min(node => node.Position.z);
        //        float maxX = nodes.Max(node => node.Position.x); // Find maximum x to estimate the container width

        //        htmlBuilder.AppendLine("<!DOCTYPE html>");
        //        htmlBuilder.AppendLine("<html lang='en'>");
        //        htmlBuilder.AppendLine("<head>");
        //        htmlBuilder.AppendLine("<meta charset='UTF-8'>");
        //        htmlBuilder.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
        //        htmlBuilder.AppendLine("<title>Node Visualization</title>");
        //        htmlBuilder.AppendLine("<style>");
        //        htmlBuilder.AppendLine("body { background-color: black; margin: 0; height: 100vh; display: flex; justify-content: center; align-items: center; }");
        //        htmlBuilder.AppendLine("#container { position: relative; transform: scaleX(-1) translateX(-30%); width: 100%; }"); // Adjust translateX if needed
        //        htmlBuilder.AppendLine(".node { width: 20px; height: 20px; position: absolute; box-shadow: 0 0 10px rgba(255,255,255,0.5); }"); // Nodes style
        //        htmlBuilder.AppendLine("</style>");
        //        htmlBuilder.AppendLine("</head>");
        //        htmlBuilder.AppendLine("<body>");
        //        htmlBuilder.AppendLine("<div id='container'>"); // Container div

        //        foreach (var node in GetNodes())
        //        {
        //            float normalizedUsage = node.Usage / ((float)ToolboxREST.HighestUsage / 5);

        //            // If usage is zero, set color to gray, otherwise interpolate between red and green
        //            string color;
        //            if (node.Usage > 0)
        //            {
        //                int r = (int)(255 * (1 - normalizedUsage));
        //                int g = (int)(255 * normalizedUsage);
        //                color = $"rgb({r},{g},0)";
        //            } else
        //            {
        //                // Set color to gray
        //                color = "gray";
        //            }

        //            // Adjust node positions by subtracting the minimum values
        //            int left = (int)((node.Position.x - minX) * 5); // Adjust scaling factor as needed
        //            int top = (int)((node.Position.z - minZ) * 5); // Adjust scaling factor as needed

        //            htmlBuilder.AppendLine($"<div class='node' style='left: {left}px; top: {top}px; background-color: {color};'></div>");
        //        }

        //        htmlBuilder.AppendLine("</div>"); // Close container div
        //        htmlBuilder.AppendLine("</body>");
        //        htmlBuilder.AppendLine("</html>");

        //        return htmlBuilder.ToString();
        //    }


        public static void UnsetActions()
        {
            SpawnResource.lastSpawnResourceAction = null;
            SpawnAnimal.lastSpawnAnimalAction = null;
        }

        //    private void CreateOrUpdateNode(Villager villager)
        //    {
        //        // Get the current position and round it to the nearest integer
        //        Vector3 currentPosition = villager.gameObject.transform.position;
        //        Vector3 roundedPosition = new Vector3(Mathf.Round(currentPosition.x), Mathf.Round(currentPosition.y), Mathf.Round(currentPosition.z));

        //        // Check if there's an existing node for this rounded position
        //        Node existingNode = nodes.Find(node => Vector3.Distance(node.Position, roundedPosition) < nodeCreationThreshold);

        //        if (existingNode != null)
        //        {
        //            existingNode.CountVillager(villager);
        //            //LoggerInstance.Msg($"Added villager to existing node: {roundedPosition}");
        //        }
        //        else
        //        {
        //            Node newNode = new Node(roundedPosition);
        //            newNode.CountVillager(villager);
        //            nodes.Add(newNode);
        //            //LoggerInstance.Msg($"Created new node: {roundedPosition}");
        //        }
        //    }


        //    public List<Node> GetNodes()
        //    {
        //        return nodes; // Return the current list of nodes.
        //    }
        //}

        //public class Node
        //{
        //    public Vector3 Position { get; private set; }
        //    public GameObject Cube { get; private set; }
        //    public List<Villager> Villagers { get; private set; }
        //    public int Usage { get; set; } // Track the usage of this node

        //    public Node(Vector3 position)
        //    {
        //        Position = position + new Vector3(0, 0.25f, 0); // Center the cube vertically

        //        Cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //        Cube.transform.position = Position;
        //        Cube.transform.localScale = new Vector3(3, 0.5f, 3); // Scale the cube
        //                                                             // Set the material smoothness to 0
        //        var renderer = Cube.GetComponent<Renderer>();
        //        renderer.material.SetFloat("_Glossiness", 0f);  //
        //        renderer.material.SetFloat("_Metallic", 0f);    // 
        //        Cube.GetComponent<Collider>().enabled = false;

        //        Usage = 2000; // Initial usage
        //        UpdateGlobalHighestUsage();
        //        UpdateNodeColor(); // Initialize color based on usage
        //    }

        //    public void CountVillager(Villager villager)
        //    {
        //        //if (!Villagers.Contains(villager))
        //        //{
        //            Usage += 500; // Increase usage when a new villager is added
        //            UpdateGlobalHighestUsage();
        //            UpdateNodeColor(); // Update color whenever usage changes
        //        //}
        //    }

        //    public void DecreaseUsage()
        //    {
        //        if (Usage > 0)
        //        {
        //            Usage -= 1; // Decrease usage over time
        //            UpdateNodeColor(); // Update color whenever usage changes
        //        }
        //    }

        //    private void UpdateGlobalHighestUsage()
        //    {
        //        if (Usage > ToolboxREST.HighestUsage)
        //        {
        //            ToolboxREST.HighestUsage = Usage; // Update the global highest usage
        //        }
        //    }

        //    private void UpdateNodeColor()
        //    {
        //        if (ToolboxREST.HighestUsage == 0) return; // Avoid division by zero

        //        float normalizedUsage = (float)Usage / (ToolboxREST.HighestUsage / 5); // Normalize usage against the highest value
        //        Color nodeColor = Color.Lerp(Color.red, Color.green, normalizedUsage); // Lerp color from red to green

        //        if(Usage <= 0)
        //        {
        //            nodeColor = Color.grey;
        //        }

        //        Cube.GetComponent<Renderer>().material.color = nodeColor; // Apply the color to the cube
        //    }
    }

}
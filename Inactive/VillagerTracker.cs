//using Il2Cpp;
//using UnityEngine;
//using WickerREST;

//namespace WickerToolbox.Commands
//{
//    public class VillagerTrackerManager
//    {
//        private static GameObject tracker = null;

//        [CommandHandler("ToggleVillagerTracker", "Village", "Toggles the villager tracker visualization.")]
//        public static void ToggleVillagerTracker()
//        {
//            // Check if the VillagerTracker object exists
//            if (tracker == null)
//            {
//                // Create the VillagerTracker GameObject
//                //tracker = GameManager.Instance.gameObject;

//                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

//                // Add and setup the VillagerTracker component
//                VillagerTrackerMono.VillagerTracker villagerTracker = cube.AddComponent<VillagerTrackerMono.VillagerTracker>(); // Add the VillagerTracker component if it doesn't exist

//                // Add and setup the NodeVisualizer component
//                NodeVisualizer nodeVisualizer = cube.AddComponent<NodeVisualizer>();
//                nodeVisualizer.villagerTracker = villagerTracker; // Assign the VillagerTracker reference

//                // Log the creation
//                WickerNetwork.Instance.LogResponse("Villager tracker created!");
//            }
//            else
//            {
//                // The VillagerTracker exists, toggle the visualization
//                NodeVisualizer visualizer = tracker.GetComponent<NodeVisualizer>();
//                if (visualizer != null)
//                {
//                    visualizer.ToggleVisualization();
//                    WickerNetwork.Instance.LogResponse("Visual toggled");
//                }
//                else
//                {
//                    // Log an error or take appropriate action if NodeVisualizer is not found
//                    WickerNetwork.Instance.LogResponse("Error: NodeVisualizer component not found.");
//                }
//            }
//        }
//    }

//    public class NodeVisualizer : MonoBehaviour
//    {
//        public VillagerTrackerMono.VillagerTracker villagerTracker; // Assign in the inspector
//        private LineRenderer lineRenderer;

//        void Awake()
//        {
//            lineRenderer = gameObject.GetComponent<LineRenderer>();
//            if (lineRenderer == null) // Ensure there is a LineRenderer component
//            {
//                lineRenderer = gameObject.AddComponent<LineRenderer>();
//            }
//            lineRenderer.enabled = false; // Start with the LineRenderer disabled
//        }

//        public void ToggleVisualization()
//        {
//            lineRenderer.enabled = !lineRenderer.enabled; // Toggle visibility

//            if (lineRenderer.enabled)
//            {
//                UpdateVisualization();
//            }
//        }

//        void UpdateVisualization()
//        {
//            List<Node> nodes = villagerTracker.GetNodes();
//            if (nodes == null || nodes.Count == 0)
//            {
//                lineRenderer.positionCount = 0;
//                return;
//            }

//            // Configure LineRenderer
//            lineRenderer.positionCount = nodes.Count;
//            lineRenderer.startWidth = 0.05f;
//            lineRenderer.endWidth = 0.05f;

//            for (int i = 0; i < nodes.Count; i++)
//            {
//                lineRenderer.SetPosition(i, nodes[i].Position);
//            }

//            // Optionally loop back to the start to close the shape
//            lineRenderer.loop = true; // You can set this based on your requirements
//        }
//    }

//    public class Node
//    {
//        public Vector3 Position { get; private set; }
//        public List<Villager> Villagers { get; private set; }

//        public Node(Vector3 position)
//        {
//            Position = position;
//            Villagers = new List<Villager>();
//        }

//        public void AddVillager(Villager villager)
//        {
//            if (!Villagers.Contains(villager))
//            {
//                Villagers.Add(villager);
//            }
//        }
//    }

//}

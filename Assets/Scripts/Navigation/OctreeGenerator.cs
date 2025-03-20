using System.Linq;
using UnityEngine;

namespace Octrees
{
    public class OctreeGenerator : MonoBehaviour
    {
        private GameObject[] obstacles; // References to the objects that are going to be inside the bounds
        [SerializeField] private float minNodeSize = 1f; // Controls how small the node can be
        public Octree octree;

        public readonly Graph waypoints = new();

        private void Awake()
        {
            GameObject[] foundObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            obstacles = foundObstacles.Where(obstacle => obstacle.GetComponent<Collider>() != null).ToArray();
            if (obstacles.Length != foundObstacles.Length)
            {
                Debug.LogWarning("Some obstacles were skipped because they don't have a Collider component.");
            }

            octree = new Octree(obstacles, minNodeSize, waypoints); // Creates new octree object at the start
        }

        // Draws the octree grid to visualize it
        private void OnDrawGizmos()
        {
            // Show this only when the application is playing
            if (!Application.isPlaying) return;

            // Draws the bounds of the octree
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(octree.bounds.center, octree.bounds.size);

            octree.root.DrawNode(); // Draws the bounds of the nodes
            //octree.graph.DrawGraph();
        }
    }
}

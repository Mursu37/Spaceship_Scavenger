using UnityEngine;

namespace Octrees
{
    // Handles all of the objects that exist inside the octree
    public class OctreeObject
    {
        private Bounds bounds; // Each object will have their own bounds

        // Assign bounds for each object
        public OctreeObject(GameObject obj)
        {
            bounds = obj.GetComponent<Collider>().bounds;
        }

        // Checks if this bounds intesects with other bounds
        public bool Intersects(Bounds boundsToCheck) => bounds.Intersects(boundsToCheck);
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Octrees
{
    public class OctreeNode
    {
        public List<OctreeObject> objects = new(); // List of objects that exist inside the node

        private static int nextId; // Static counter to assign unique ID numbers for each node
        public readonly int id; // Unique ID for this specific node

        public Bounds bounds; // Represents the bounding box of the node
        private Bounds[] childBounds = new Bounds[8]; // Stores the bounds of the eight potential child nodes
        public OctreeNode[] children; // Array of child nodes

        public bool isLeaf => children == null; // Indicates whether the node is a leaf node (i.e., it has no children)

        private float minNodeSize; // The minimum size a node can shrink to before subdivision stops

        // Constructor to initialize the OctreeNode
        public OctreeNode(Bounds bounds, float minNodeSize)
        {
            id = nextId++; // Assign a unique ID to the node

            // Set the bounds and minimum size
            this.bounds = bounds;
            this.minNodeSize = minNodeSize;

            // Calculate the size and offset for child nodes
            Vector3 newSize = bounds.size * 0.5f; // Child nodes will be half the size of the current node
            Vector3 centerOffset = bounds.size * 0.25f; // Offset to position child centers correctly
            Vector3 parentCenter = bounds.center;

            // Calculate bounds for each child node
            for (int i = 0; i < 8; i++)
            {
                Vector3 childCenter = parentCenter;

                // Adjust center position for each child node based on its index
                childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1);
                childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
                childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1);

                childBounds[i] = new Bounds(childCenter, newSize); // Create the bounds for the child node
            }
        }

        // Public method to divide the node and add a GameObject
        public void Divide(GameObject obj) => Divide(new OctreeObject(obj));

        // Recursive function to subdivide the node and distribute an object into child nodes
        private void Divide(OctreeObject octreeObject)
        {
            // If the node size is less than or equal to the minimum size, stop dividing
            if (bounds.size.x <= minNodeSize)
            {
                AddObject(octreeObject);
                return;
            }

            children ??= new OctreeNode[8]; // Initialize child nodes if not already created

            bool intersectedChild = false; // Track if the object intersects with any child node

            // Iterate through all child bounds to determine where the object fits
            for (int i = 0; i < 8; i++)
            {
                children[i] ??= new OctreeNode(childBounds[i], minNodeSize); // Create the child node if it doesn't exist

                // If the object intersects with the child bounds, further divide the child
                if (octreeObject.Intersects(childBounds[i]))
                {
                    children[i].Divide(octreeObject);
                    intersectedChild = true;
                }
            }

            // If the object does not intersect any child node, add it to the current node
            if (!intersectedChild)
            {
                AddObject(octreeObject);
            }
        }

        // Adds an object directly to the current node
        private void AddObject(OctreeObject octreeObject) => objects.Add(octreeObject);

        // Debug function to visualize the node and its contents using Gizmos
        public void DrawNode()
        {
            // Draw the bounds of the current node as a green wireframe cube
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            // For each object in the node, draw it as a red solid cube if it intersects the bounds
            //foreach (OctreeObject obj in objects)
            //{
            //    if (obj.Intersects(bounds))
            //    {
            //        Gizmos.color = Color.red;
            //        Gizmos.DrawCube(bounds.center, bounds.size);
            //    }
            //}

            // Recursively draw all child nodes, if they exist
            if (children != null)
            {
                foreach (OctreeNode child in children)
                {
                    if (child != null) child.DrawNode();
                }
            }
        }
    }
}

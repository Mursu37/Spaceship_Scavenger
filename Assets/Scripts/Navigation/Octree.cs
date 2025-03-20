using System.Collections.Generic;
using UnityEngine;

namespace Octrees
{
    public class Octree
    {
        public OctreeNode root; // Root of the octree
        public Bounds bounds; // Used to check what's inside the octree
        public Graph graph;

        private List<OctreeNode> emptyLeaves = new(); // List of empty leaves

        // Every world object that are going to be contained inside the octree and minimum node size are set as parameters
        public Octree(GameObject[] worldObjects, float minNodeSize, Graph graph)
        {
            this.graph = graph;

            CalculateBounds(worldObjects);
            CreateTree(worldObjects, minNodeSize);

            GetEmptyLeaves(root);
            GetEdges();
            Debug.Log(graph.edges.Count);
        }

        public OctreeNode FindClosestNode(Vector3 position) => FindClosestNode(root, position);

        public OctreeNode FindClosestNode(OctreeNode node, Vector3 position)
        {
            OctreeNode found = null;
            for (int i = 0; i < node.children.Length; i++)
            {
                if (node.children[i].bounds.Contains(position))
                {
                    if (node.children[i].isLeaf)
                    {
                        found = node.children[i];
                        break;
                    }
                    found = FindClosestNode(node.children[i], position);
                }
            }
            return found;
        }

        private void GetEdges()
        {
            foreach (OctreeNode leaf in emptyLeaves)
            {
                foreach (OctreeNode otherLeaf in emptyLeaves)
                {
                    if (leaf.bounds.Intersects(otherLeaf.bounds))
                    {
                        graph.AddEdge(leaf, otherLeaf);
                    }
                }
            }
        }

        // Creates the tree
        private void CreateTree(GameObject[] worldObjects, float minNodeSize)
        {
            root = new OctreeNode(bounds, minNodeSize);

            // Iterates trough all of the world objects and divides the nodes that intersects with the object
            foreach (var obj in worldObjects)
            {
                root.Divide(obj);
            }
        }

        // Gets nodes that can be passed trough
        private void GetEmptyLeaves(OctreeNode node)
        {
            if (node.isLeaf && node.objects.Count == 0)
            {
                emptyLeaves.Add(node);
                graph.AddNode(node);
                return;
            }

            if (node.children == null) return;

            foreach (OctreeNode child in node.children)
            {
                GetEmptyLeaves(child);
            }

            for (int i = 0; i < node.children.Length; i++)
            {
                for (int j = i + 1; j < node.children.Length; j++)
                {
                    if (i == j) continue;
                    graph.AddEdge(node.children[i], node.children[j]);
                }
            }
        }

        // Calculates bounds that contains all of the world objects
        private void CalculateBounds(GameObject[] worldObjects)
        {
            // Iterates all of the objects and adds their colliders into the bounds
            foreach (GameObject obj in worldObjects)
            {
                bounds.Encapsulate(obj.GetComponent<Collider>().bounds);
            }

            // Makes the bounds into perfect box
            Vector3 size = Vector3.one * Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * 0.7f;
            bounds.SetMinMax(bounds.center - size, bounds.center + size); 
        }
    }
}
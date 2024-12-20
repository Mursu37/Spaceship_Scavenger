using System.Linq;
using UnityEngine;

namespace Octrees
{
    public class Mover: MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float accuracy = 1f;
        [SerializeField] private float turnSpeed = 5f;
        [SerializeField] private Transform targetDestination;

        private int currentWaypoint;
        private OctreeNode currentNode;
        private Vector3 destination;
        private OctreeNode currentTargetNode;

        [SerializeField] private OctreeGenerator octreeGenerator;
        private Graph graph;

        Vector3 direction;
        private bool canMove = false;

        private void Start()
        {
            graph = octreeGenerator.waypoints;
            currentNode = GetClosestNode(transform.position);
            currentTargetNode = GetClosestNode(targetDestination.position);

            if (targetDestination != null)
            {
                GetDestination(targetDestination.position);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                GetDestination(targetDestination.position);
            }

            if (currentTargetNode != GetClosestNode(targetDestination.position))
            {
                Debug.Log("Target destination changed.");

                currentTargetNode = GetClosestNode(targetDestination.position);
                GetDestination(targetDestination.position);
            }

            if (graph == null) return;

            if (graph.GetPathLength() == 0 || currentWaypoint >= graph.GetPathLength())
            {
                //GetRandomDestination();
                return;
            }

            if (Vector3.Distance(graph.GetPathNode(currentWaypoint).bounds.center, transform.position) < accuracy)
            {
                currentWaypoint++;
                Debug.Log($"Waypoint {currentWaypoint} reached.");
            }

            if (currentWaypoint < graph.GetPathLength())
            {
                currentNode = graph.GetPathNode(currentWaypoint);
                destination = currentNode.bounds.center;

                direction = destination - transform.position;
                direction.Normalize();

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
                //canMove = true;
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            else
            {
                canMove = false;
            }
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                GetComponent<Rigidbody>().AddForce(direction * speed);
            }
        }

        private OctreeNode GetClosestNode(Vector3 position)
        {
            return octreeGenerator.octree.FindClosestNode(position);
        }

        private void GetDestination(Vector3 destination)
        {
            OctreeNode destinationNode = octreeGenerator.octree.FindClosestNode(destination);

            if (graph.AStar(currentNode, destinationNode))
            {
                currentWaypoint = 0;
            }
        }

        private void GetRandomDestination()
        {
            OctreeNode destinationNode;

            do
            {
                destinationNode = graph.nodes.ElementAt(Random.Range(0, graph.nodes.Count)).Key;
            }
            while (!graph.AStar(currentNode, destinationNode));

            currentWaypoint = 0;
        }

        private void OnDrawGizmos()
        {
            if (graph == null || graph.GetPathLength() == 0) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(graph.GetPathNode(0).bounds.center, 0.7f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(graph.GetPathNode(graph.GetPathLength() - 1).bounds.center, 0.7f);

            Gizmos.color = Color.green;
            for (int i = 0; i < graph.GetPathLength(); i++)
            {
                Gizmos.DrawWireSphere(graph.GetPathNode(i).bounds.center, 0.5f);
                if (i < graph.GetPathLength() - 1)
                {
                    Vector3 start = graph.GetPathNode(i).bounds.center;
                    Vector3 end = graph.GetPathNode(i + 1).bounds.center;
                    Gizmos.DrawLine(start, end);
                }
            }
        }
    }
}

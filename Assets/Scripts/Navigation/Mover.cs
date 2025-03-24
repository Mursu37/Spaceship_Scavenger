using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Octrees
{
    public class Mover: MonoBehaviour
    {
        [SerializeField] private float acceleration = 0.05f;
        [SerializeField] private float accuracy = 1f;
        [SerializeField] private float turnAcceleration = 5f;
        [SerializeField] private float turnSpeed = 5f;
        [SerializeField] private Transform targetDestination;

        private int currentWaypoint;
        private OctreeNode targetNode;
        private Vector3 destination;
        private OctreeNode currentTargetNode;
        private OctreeNode currentNode;

        [SerializeField] private OctreeGenerator octreeGenerator;
        private Graph graph;

        Vector3 direction;
        private bool canMove = false;
        private bool isOutOfBounds = false;

        private Rigidbody rb;
        private OctreeNode previousNode;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            graph = octreeGenerator.waypoints;
            targetNode = GetClosestNode(transform.position);
            currentNode = targetNode;
            previousNode = currentNode;
            currentTargetNode = GetClosestNode(targetDestination.position);

            if (targetDestination != null)
            {
                GetDestination(targetDestination.position);
            }
        }

        private void Update()
        {
            if (currentTargetNode != GetClosestNode(targetDestination.position))
            {
                previousNode = currentNode;
                currentTargetNode = GetClosestNode(targetDestination.position);
                GetDestination(targetDestination.position);
            }

            if (currentNode != GetClosestNode(transform.position) && targetNode != GetClosestNode(transform.position))
            {
                previousNode = currentNode;
                currentNode = GetClosestNode(transform.position);
                targetNode = GetClosestNode(transform.position);
                GetDestination(targetDestination.position);
            }

            if (graph == null) return;

            if (graph.GetPathLength() == 0 || currentWaypoint >= graph.GetPathLength())
            {
                GetRandomNeighbor();
                return;
            }

            if (Vector3.Distance(graph.GetPathNode(currentWaypoint).bounds.center, transform.position) < accuracy)
            {
                currentWaypoint++;
                currentNode = GetClosestNode(transform.position);
                Debug.Log($"Waypoint {currentWaypoint} reached.");
            }

            if (currentWaypoint < graph.GetPathLength())
            {
                targetNode = graph.GetPathNode(currentWaypoint);
                destination = targetNode.bounds.center;

                direction = destination - transform.position;
                direction.Normalize();

                
                canMove = true;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
                //transform.Translate(0, 0, speed * Time.deltaTime);
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
                Vector3 desiredDirection = direction.normalized;
                Vector3 currentForward = transform.forward;

                Vector3 rotationDirection = Vector3.Cross(currentForward, desiredDirection);

                //rb.AddTorque(rotationDirection * turnAcceleration, ForceMode.VelocityChange);

                rb.AddForce(currentForward * acceleration, ForceMode.VelocityChange);
            }

            if (!graph.nodes.ContainsKey(targetNode))
            {
                rb.AddForce(previousNode.bounds.center * acceleration, ForceMode.VelocityChange);
            }
        }

        private OctreeNode GetClosestNode(Vector3 position)
        {
            return octreeGenerator.octree.FindClosestNode(position);
        }

        private void GetDestination(Vector3 destination)
        {
            Debug.Log("Finding new path.");

            OctreeNode destinationNode = octreeGenerator.octree.FindClosestNode(destination);

            if (graph.AStar(targetNode, destinationNode))
            {
                currentWaypoint = 0;
            }
            else
            {
                if (!graph.nodes.ContainsKey(destinationNode))
                {
                    GetRandomNeighbor();
                }
            }
        }

        private void GetRandomNeighbor()
        {
            List<OctreeNode> neighbors = graph.GetNeighbors(targetNode);

            OctreeNode destinationNode = neighbors[Random.Range(0, neighbors.Count)];

            if (graph.AStar(targetNode, destinationNode))
            {
                currentWaypoint = 0;
            }
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

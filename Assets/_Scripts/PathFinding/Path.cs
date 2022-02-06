using System;
using System.Collections.Generic;
using _Scripts.PathFinding.Interface;
using _Scripts.WorldGen;
using UnityEngine;

namespace _Scripts.PathFinding
{
    /**
     * This is using Dijkstra's Algorithm but a better approach would be to transform tha code to A*.
     *
     * # Bugs:
     *      - Bug exists when the offset between tiles is too great, neighbours arent found for some of the nodes.
     */
    [Serializable]
    public class Path : IPathFinder
    {
        private List<Node> _availableNodes;

        public Path()
        { 
            // Blank.
        }

        public void SetAvailableTiles(List<Node> availableNodes)
        {
            _availableNodes = availableNodes;
        }

        // Returns a path from our startingPosition, to Our ending position if it exists.
        public List<Node> FindPath(Node startingNode, Node endingNode)
        {
            List<Node> finalPath = new List<Node>();
            List<Node> unexplored = new List<Node>();

            // Add the starting and endingNodes to our unexplored List.
            unexplored.Add(startingNode);
            unexplored.Add(endingNode);

            // Loop through all of our nodes and find the neighbours. Then add that node to our unexplored list.
            foreach (var node in _availableNodes)
            {
                unexplored.Add(FindNeighbourNodes(node));
            }

            // Set the startingNode to be 0;
            startingNode.SetWeight(0);

            // Return the endingNode
            Node currentNode = FindPath(startingNode, endingNode, unexplored);

            // Loop through until our currentNode is null.
            while (currentNode != null)
            {
                // Add the currentNode to our FinalPath
                finalPath.Add(currentNode);
                Node current = currentNode;
                // Set the currentNode to be that of our parentNode so we can recursively loop through all parentNodes and find the path.
                currentNode = current.GetParentNode();
            }

            // Reverse the path as it is returned from end to start.
            finalPath.Reverse();

            // Path is now completed.
            return finalPath;
        }

        private Node FindPath(Node startingNode, Node endingNode, List<Node> unexplored)
        {
            // Loop until all of our unexplored nodes are explored.
            while (unexplored.Count != 0)
            {
                // Sort our list to have the lightest nodes first;
                unexplored.Sort((x, z) => x.GetWeight().CompareTo(z.GetWeight()));

                // Pull out the lightest node from the unexplored list.
                Node node = unexplored[0];
                
                // No need to loop through all nodes if the endNode has been reached.
                if(node ==  endingNode)
                    break;

                // Remove the currentNode as no longer unexplored.
                unexplored.Remove(node);

                // Loop through the currentNodes neighbours.
                foreach (var neighNode in node.GetNeighbourNodes())
                {
                    Node neighbourNode = neighNode;

                    if (unexplored.Contains(neighNode))
                    {
                        // Get the startingNodes Vector based on Coords;
                        Vector3 sNode = new Vector3(startingNode.GetCoords().GetX(), 0,
                            startingNode.GetCoords().GetZ());

                        // Get the currentNeighbours Vector based on Coords;
                        Vector3 cNode = new Vector3(neighbourNode.GetCoords().GetX(), 0,
                            neighbourNode.GetCoords().GetZ());

                        // Check the distance and weight of currentNeighbour compared to the currentNode;
                        float distance = Vector3.Distance(sNode, cNode);
                        distance = node.GetWeight() + distance;

                        // Checks the if the currentNeighbours Distance is less than the currentNodes weight
                        if (distance < neighbourNode.GetWeight())
                        {
                            // If distance is less, re-assign the currentNeighbours weight to be the calculated distance,
                            // And assign the neighbours parentNode to be the currentNode;
                            neighbourNode.SetWeight(distance);
                            neighbourNode.SetParent(node);
                        }
                    }
                }
            }

            return endingNode;
        }

        public Node FindNeighbourNodes(Node toFind)
        {
            Node currentNode = toFind;

            // Look through all of our available Nodes;
            foreach (var current in _availableNodes)
            {
                // Find the node below (Down)
                if (current.GetCoords().GetZ()
                        .Equals(currentNode.GetCoords().GetZ() - Mathf.FloorToInt(WorldGenerator.TileOffset)) &&
                    current.GetCoords().GetX() == currentNode.GetCoords().GetX())
                {
                    currentNode.AddNeighbourNode(current);
                }
                // Find the node Above (Up)
                else if (current.GetCoords().GetZ()
                             .Equals(currentNode.GetCoords().GetZ() + Mathf.FloorToInt(WorldGenerator.TileOffset)) &&
                         current.GetCoords().GetX() == currentNode.GetCoords().GetX())
                {
                    currentNode.AddNeighbourNode(current);
                }

                // Find the node to our left (Left)
                else if (current.GetCoords().GetX()
                             .Equals(currentNode.GetCoords().GetX() - Mathf.FloorToInt(WorldGenerator.TileOffset)) &&
                         current.GetCoords().GetZ() == currentNode.GetCoords().GetZ())
                {
                    currentNode.AddNeighbourNode(current);
                }
                // Find the node to our right (Right)
                else if (current.GetCoords().GetX()
                             .Equals(currentNode.GetCoords().GetX() + Mathf.FloorToInt(WorldGenerator.TileOffset)) &&
                         current.GetCoords().GetZ() == currentNode.GetCoords().GetZ())
                {
                    currentNode.AddNeighbourNode(current);
                }
            }
            
            // This is for debugging.
            if (currentNode.GetNeighbourNodes().Count <= 0)
                Debug.LogError(
                    $"Something Went Wrong... No Neighbours Detected For: {currentNode.GetCoords().ToString()}");

            // Return the currentNode with all of its neighbours.
            return currentNode;
        }
    }
}
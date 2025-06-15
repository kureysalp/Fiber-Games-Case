using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using FiberCase.Grid_System;

namespace FiberCase.Path_Finding
{
    public class PathFinding : MonoBehaviour
    {
        private GridManager _grid;

        private void Awake()
        {
            _grid = GetComponent<GridManager>();
        }
        
        public async Task<List<Node>> FindPathAsync(Node startNode, Node targetNode)
        {
            return await Task.Run(() =>
            {
                var openSet = new SortedSet<Node>();
                var closedSet = new HashSet<Node>();

                foreach (var node in _grid.Grid)
                    node.ResetPathfindingData();

                startNode.SetGCost(0);
                startNode.SetHCost(GetDistance(startNode, targetNode));
                startNode.SetParent(null);

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    var currentNode = openSet.Min;
                    openSet.Remove(currentNode);
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        return RetracePath(startNode, targetNode);
                    }

                    foreach (var neighbour in _grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.Walkable || neighbour.IsOccupied || closedSet.Contains(neighbour))
                            continue;

                        var tentativeGCost = currentNode.GCost + 10;

                        if (tentativeGCost < neighbour.GCost || !openSet.Contains(neighbour))
                        {
                            if (openSet.Contains(neighbour))
                                openSet.Remove(neighbour);

                            neighbour.SetGCost(tentativeGCost);
                            neighbour.SetHCost(GetDistance(neighbour, targetNode));
                            neighbour.SetParent(currentNode);

                            openSet.Add(neighbour);
                        }
                    }
                }

                return null;
            });
        }

        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            if(!startNode.IsOccupied)
                path.Add(startNode);
            path.Reverse();
            return path;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            var distanceX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
            var distanceY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

            return 10 * (distanceX + distanceY);
        }
    }
}
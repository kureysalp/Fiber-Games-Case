using UnityEngine;
using System.Collections.Generic;
using FiberCase.Grid_System;

namespace FiberCase.Path_Finding
{
    public class PathFinding : MonoBehaviour
    {
        public Vector2Int seeker;
        public Vector2Int target;

        GridManager _grid;

        void Awake()
        {
            _grid = GetComponent<GridManager>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FindPath(_grid.GetNodeFromGridPosition(seeker), _grid.GetNodeFromGridPosition(target));
            }
        }

        void FindPath(Node startPos, Node targetPos)
        {
            //var startNode = _grid.GetNodeFromWorldPosition(startPos);
            //var targetNode = _grid.GetNodeFromWorldPosition(targetPos);

            var startNode = startPos;
            var targetNode = targetPos;
            
            var openSet = new SortedSet<Node>();
            var closedSet = new HashSet<Node>();
            
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
                    RetracePath(startNode, targetNode);
                    return;
                }
                
                foreach (var neighbour in _grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
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
        }

        private void RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            _grid.SetPath(path);
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            var distanceX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
            var distanceY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

            return 10* (distanceX + distanceY);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FiberCase.Path_Finding;
using UnityEngine;

namespace FiberCase.Grid_System
{
    public class GridManager : MonoBehaviour
    {
        public Node[,] Grid { get; private set; }
        
        [SerializeField] private int _nodeSize;
        [SerializeField] private int _columns;
        [SerializeField] private int _rows;
        
        [SerializeField] private bool _drawGizmos;

        private Vector3 _bottomLeftPosition;

        private int _nodeRadius;

        public List<Node> Path { get; private set; }

        [SerializeField] private Transform _startNodePosition;
        public Node StartNode { get; private set; }
        
        private PathFinding _pathFinding;

        private void Awake()
        {
            _pathFinding = GetComponent<PathFinding>();
        }

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            _nodeRadius = _nodeSize / 2;

            Grid = new Node[_columns, _rows];

            _bottomLeftPosition = transform.position - Vector3.right * _columns * _nodeSize / 2 - Vector3.forward * _rows * _nodeSize / 2;

            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    var nodeWorldPosition = _bottomLeftPosition + Vector3.right * (_nodeSize * x + _nodeRadius) +
                                            Vector3.forward *
                                            (_nodeSize * y + _nodeRadius);
                    var nodeGridPosition = new Vector2Int(x, y);

                    Grid[x, y] = new Node(true, nodeGridPosition, nodeWorldPosition);
                }
            }

            StartNode = GetNodeFromWorldPosition(_startNodePosition.position);
        }

        public async Task<List<Node>> FindPathAsync(Vector3 targetPosition)
        {
            var targetNode = GetNodeFromWorldPosition(targetPosition);
            Debug.Log("touched node: "+ targetNode);
            if (targetNode == null) return null;
            
            return await _pathFinding.FindPathAsync(StartNode, targetNode);
        }

        public Node[] GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            int[,] directions =
            {
                { 0, 1 },   
                { 1, 0 },   
                { 0, -1 },  
                { -1, 0 }   
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                var checkX = node.Position.x + directions[i, 0];
                var checkY = node.Position.y + directions[i, 1];

                if (checkX >= 0 && checkX < _columns && checkY >= 0 && checkY < _rows)
                {
                    neighbours.Add(Grid[checkX, checkY]);
                }
            }
            return neighbours.ToArray();
        }

        public Node GetNodeFromWorldPosition(Vector3 position)
        {
            var xPercent = Mathf.Clamp01((position.x - _bottomLeftPosition.x) / _columns * _nodeSize);
            var yPercent = Mathf.Clamp01((position.y - _bottomLeftPosition.y) / _rows * _nodeSize);

            var xPosition = Mathf.RoundToInt((_columns - 1) * xPercent);
            var yPosition = Mathf.RoundToInt((_rows - 1) * yPercent);

            return Grid[xPosition, yPosition];
        }

        public Node GetNodeFromGridPosition(Vector2Int position)
        {
            if (position.x < 0 || position.x >= _columns || position.y < 0 || position.y >= _rows) return null;
            
            return Grid[position.x, position.y];
        }

        public void SetPath(List<Node> path)
        {
            Path = path;
        }

        void OnDrawGizmos()
        {
            if (!_drawGizmos) return;
            
            Gizmos.DrawWireCube(transform.position, new Vector3(_columns, .1f, _rows) * _nodeSize);

            if (Grid != null)
            {
                foreach (Node node in Grid)
                {
                    Gizmos.color = (node.Walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(node.WorldPosition, new Vector3(_nodeSize - 0.1f, .1f, _nodeSize - 0.1f));
                }
            }
            
            if (Path != null)
            {
                foreach (Node n in Path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (_nodeSize - 0.1f));
                }
            }

        }
    }
}
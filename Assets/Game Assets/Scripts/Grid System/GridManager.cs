using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FiberCase.Event;
using FiberCase.Path_Finding;
using UnityEngine;
using UnityEngine.Serialization;

namespace FiberCase.Grid_System
{
    public class GridManager : MonoBehaviour
    {
        public Node[,] Grid { get; private set; }
        
        [SerializeField] private float _nodeSize;
        [SerializeField] private int _columns;
        [SerializeField] private int _rows;
        
        [SerializeField] private GameObject _nodePrefab;
        
        [SerializeField] private bool _drawGizmos;
        

        private Vector3 _bottomLeftPosition;

        private float _nodeRadius;

        public List<Node> Path { get; private set; }

        [SerializeField] private Transform _firstStartNodePosition;
        [SerializeField] private Transform _secondStartNodePosition;
        public Node FirstStartNode { get; private set; }
        public Node SecondStartNode { get; private set; }
        
        private PathFinding _pathFinding;

        [SerializeField] private LayerMask _blockLayer;

        private void Awake()
        {
            _pathFinding = GetComponent<PathFinding>();
        }

        private void Start()
        {
            CreateGrid();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<ReadyForPlayerInputEvent>(CheckStartNodesAvailability);
            EventBus.Subscribe<PlayAgainEvent>(ResetAllNodes);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<ReadyForPlayerInputEvent>(CheckStartNodesAvailability);
            EventBus.Unsubscribe<PlayAgainEvent>(ResetAllNodes);
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

                    var isWalkable = !CheckIfBlocked(nodeWorldPosition);
                    Grid[x, y] = new Node(isWalkable, nodeGridPosition, nodeWorldPosition);
                    
                    if(isWalkable)
                        Instantiate(_nodePrefab, nodeWorldPosition, Quaternion.identity, transform);
                }
            }

            FirstStartNode = GetNodeFromWorldPosition(_firstStartNodePosition.position);
            SecondStartNode = GetNodeFromWorldPosition(_secondStartNodePosition.position);
        }

        public async Task<List<Node>> FindPathAsync(Vector3 targetPosition)
        {
            var targetNode = GetNodeFromWorldPosition(targetPosition);
            if (targetNode == null) return null;

            var startNode = FirstStartNode;
            if(FirstStartNode.IsOccupied)
                startNode = SecondStartNode;

            Path = await _pathFinding.FindPathAsync(startNode, targetNode);
            return Path;
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

        private bool CheckIfBlocked(Vector3 position)
        {
            return Physics.CheckSphere(position, _nodeRadius / 2f, _blockLayer);
        }
        
        public Node GetNodeFromWorldPosition(Vector3 worldPosition)
        {
            var gridWidth = _columns * _nodeSize;
            var gridHeight = _rows * _nodeSize;

            var left = transform.position.x - gridWidth / 2f;
            var bottom = transform.position.z - gridHeight / 2f;

            var x = Mathf.FloorToInt((worldPosition.x - left) / _nodeSize);
            var y = Mathf.FloorToInt((worldPosition.z - bottom) / _nodeSize);

            x = Mathf.Clamp(x, 0, _columns - 1);
            y = Mathf.Clamp(y, 0, _rows - 1);

            return Grid[x, y];
        }
        
        public Node GetNodeFromGridPosition(Vector2Int position)
        {
            if (position.x < 0 || position.x >= _columns || position.y < 0 || position.y >= _rows) return null;
            
            return Grid[position.x, position.y];
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
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(n.WorldPosition, new Vector3(_nodeSize - 0.1f, .1f, _nodeSize - 0.1f));
                }
            }
        }

        private void CheckStartNodesAvailability(ReadyForPlayerInputEvent readyEvent)
        {
            var bothIsOccupied = FirstStartNode.IsOccupied && SecondStartNode.IsOccupied;

            if (bothIsOccupied)
                EventBus.Raise(new GameLostEvent());
        }

        private void ResetAllNodes(PlayAgainEvent playAgainEvent)
        {
            foreach (var node in Grid)
            {
                if(node.Walkable)
                    node.ResetNode();
            }
        }
    }
}
using System;
using FiberCase.Gameplay;
using UnityEngine;

namespace FiberCase.Grid_System
{
    public class Node : IComparable<Node>
    {
        public bool Walkable { get; set; }
        public bool IsOccupied { get; private set; }
        public Vector2Int Position { get; }
        public Vector3 WorldPosition { get; set; }
        
        public int GCost { get; private set; }
        public int HCost { get; private set; }
        public int FCost => GCost + HCost;
        
        public Node Parent { get; set; }
        
        public CoinHolder CoinHolder { get; private set; }

        public void SetGCost(int cost)
        {
            GCost = cost;
        }

        public void SetHCost(int cost)
        {
            HCost = cost;
        }

        public void SetParent(Node parent)
        {
            Parent = parent;
        }

        public Node(bool walkable, Vector2Int position, Vector3 worldPosition)
        {
            Walkable = walkable;
            Position = position;
            WorldPosition = worldPosition;
        }

        public void SetWalkable(bool walkable)
        {
            Walkable = walkable;    
        }

        public void CoinStackOnThisNode(CoinHolder coinHolder)
        {
            CoinHolder = coinHolder;
            IsOccupied = true;
        }

        public void RemoveCoinHolder()
        {
            CoinHolder = null;
            IsOccupied = false;
        }
        
        public int CompareTo(Node other)
        {
            var compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
                compare = HCost.CompareTo(other.HCost);
            if (compare == 0)
                compare = GetHashCode().CompareTo(other.GetHashCode());

            return compare;
        }

        public void ResetPathfindingData()
        {
            GCost = 0;
            HCost = 0;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not Node other) return false;
            return Position.x == other.Position.x && Position.y == other.Position.y;
        }
        
        public override int GetHashCode()
        {
            return Position.x * 1000 + Position.y;
        }

    }
    
}
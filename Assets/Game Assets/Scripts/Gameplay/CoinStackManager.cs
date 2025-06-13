using System;
using System.Collections.Generic;
using FiberCase.Game_State;
using FiberCase.Grid_System;
using UnityEngine;

namespace FiberCase.Gameplay
{
    public class CoinStackManager : MonoBehaviour
    {
        private CoinStackManagerStateMachine _stackManagerStateMachine;
        public CoinStackManagerIdleState IdleState { get; set; }
        public CoinStackManagerCoinSpawningState CoinSpawningState { get; set; }
        public CoinStackManagerMovingState MovingState { get; set; }
        public CoinStackManagerCoinSortingState SortingState { get; set; }

        private CoinStack _currentCoinStack;
        
        [SerializeField] private GridManager  _gridManager;
        public GridManager GridManager => _gridManager;
        
        [SerializeField] private PlayerInput _playerInput;
        public PlayerInput PlayerInput => _playerInput;

        private List<Node> _currentStackPath;
        
        private void Awake()
        {
            _stackManagerStateMachine = new CoinStackManagerStateMachine();
            
            IdleState = new CoinStackManagerIdleState(this, _stackManagerStateMachine);
            CoinSpawningState = new CoinStackManagerCoinSpawningState(this, _stackManagerStateMachine);
            MovingState = new CoinStackManagerMovingState(this, _stackManagerStateMachine);
            SortingState = new CoinStackManagerCoinSortingState(this, _stackManagerStateMachine);
        }

        private void Start()
        {
            _stackManagerStateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            _stackManagerStateMachine.CurrentState.UpdateState();
        }

        public void SetStackMovePath(List<Node> path)
        {
            _currentStackPath = path;
        }
    }
}
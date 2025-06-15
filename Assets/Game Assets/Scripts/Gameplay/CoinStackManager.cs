using System;
using System.Collections.Generic;
using FiberCase.Game_State;
using FiberCase.Grid_System;
using FiberCase.Scriptable_Objects;
using FiberCase.Systems;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace FiberCase.Gameplay
{
    public class CoinStackManager : MonoBehaviour
    {
        private CoinStackManagerStateMachine _stackManagerStateMachine;
        public CoinStackManagerIdleState IdleState { get; set; }
        public CoinStackManagerCoinSpawningState CoinSpawningState { get; set; }
        public CoinStackManagerMovingState MovingState { get; set; }
        public CoinStackManagerCoinSortingState SortingState { get; set; }

        public CoinHolder CurrentCoinHolder { get; private set; }
        public CoinHolder CoinHolderOnQueue { get; private set; }


        [SerializeField] private GridManager  _gridManager;
        public GridManager GridManager => _gridManager;
        
        [SerializeField] private PlayerInput _playerInput;
        public PlayerInput PlayerInput => _playerInput;

        public List<Node> CurrentStackPath { get; private set; }

        [SerializeField] private Transform _coinHolderHoldPosition;
        public Transform CoinHolderHoldPosition => _coinHolderHoldPosition;

        [SerializeField] private Transform _coinHolderQueuePosition;
        public Transform CoinHolderQueuePosition => _coinHolderQueuePosition;


        [SerializeField] private float _moveSpeed;
        public float MoveSpeed => _moveSpeed;


        [SerializeField] private CoinStack[] _coinStackCollection; 
        public CoinStack[] CoinStackCollection => _coinStackCollection;
        
        [SerializeField] private CoinColorCoding _coinColorCoding;



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
            InitializeFirstCoinStack();
            _stackManagerStateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            _stackManagerStateMachine.CurrentState.UpdateState();
            //_ = _stackManagerStateMachine.CurrentState.UpdateStateAsync();
        }

        public void SetStackMovePath(List<Node> path)
        {
            CurrentStackPath = path;
        }

        private void InitializeFirstCoinStack()
        {
            var coinHolderForCurrent = CreateCoinHolder();
            var coinHolderForQueue = CreateCoinHolder();
            
            SetCoinHolderOnQueue(coinHolderForCurrent);
            DeployCoinHolderFromQueue();
            SetCoinHolderOnQueue(coinHolderForQueue);
            
            coinHolderForCurrent.transform.position = _coinHolderHoldPosition.position;
        }
        
        public CoinHolder CreateCoinHolder()
        {
            var newCoinHolder = Poolable.Get<CoinHolder>();

            var randomCoinStack =
                CoinStackCollection[Random.Range(0, CoinStackCollection.Length)];
            newCoinHolder.CreateStack(randomCoinStack, _coinColorCoding);
            
            return newCoinHolder;
            
        }
        
        
        public void SetCoinHolderOnQueue(CoinHolder coinHolder)
        {
            CoinHolderOnQueue = coinHolder;
            CoinHolderOnQueue.transform.position = _coinHolderQueuePosition.position;
        }

        public void DeployCoinHolderFromQueue()
        {
            CurrentCoinHolder = CoinHolderOnQueue;
            CoinHolderOnQueue = null;
        }
        
    }
}
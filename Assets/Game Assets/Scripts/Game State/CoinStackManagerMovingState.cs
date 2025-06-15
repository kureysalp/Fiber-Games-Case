using FiberCase.Gameplay;
using UnityEngine;

namespace FiberCase.Game_State
{
    public class CoinStackManagerMovingState : CoinStackManagerState
    {
        private int _waypointIndex;
        public CoinStackManagerMovingState(CoinStackManager coinStackManager, CoinStackManagerStateMachine coinStackManagerStateMachine) : base(coinStackManager, coinStackManagerStateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            _waypointIndex = 0;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            if (_waypointIndex == CoinStackManager.CurrentStackPath.Count)
            {
                CoinStackManagerStateMachine.ChangeState(CoinStackManager.SortingState);
                return;
            }

            var endPosition = CoinStackManager.CurrentStackPath[_waypointIndex].WorldPosition;
            CoinStackManager.CurrentCoinHolder.MovePosition(endPosition, CoinStackManager.MoveSpeed);

            if ((CoinStackManager.CurrentCoinHolder.transform.position - endPosition).sqrMagnitude < 0.01f)
            {
                _waypointIndex++;
                if(_waypointIndex == CoinStackManager.CurrentStackPath.Count)
                {
                    var nodeOnCoinHolder = CoinStackManager.GridManager.GetNodeFromWorldPosition(CoinStackManager.CurrentCoinHolder.transform
                            .position);
                    nodeOnCoinHolder.CoinStackOnThisNode(CoinStackManager.CurrentCoinHolder);
                    CoinStackManager.CurrentCoinHolder.SetNode(nodeOnCoinHolder);
                }
            }
        }
    }
}
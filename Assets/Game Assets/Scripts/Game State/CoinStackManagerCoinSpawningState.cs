using FiberCase.Gameplay;
using FiberCase.Systems;
using UnityEngine;

namespace FiberCase.Game_State
{
    public class CoinStackManagerCoinSpawningState : CoinStackManagerState
    {
        public CoinStackManagerCoinSpawningState(CoinStackManager coinStackManager, CoinStackManagerStateMachine coinStackManagerStateMachine) : base(coinStackManager, coinStackManagerStateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            CoinStackManager.DeployCoinHolderFromQueue();
            
            CoinStackManager.SetCoinHolderOnQueue(CoinStackManager.CreateCoinHolder());
        }

       

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            var targetPosition = CoinStackManager.CoinHolderHoldPosition.position;
            
            CoinStackManager.CurrentCoinHolder.MovePosition(targetPosition, CoinStackManager.MoveSpeed);
            
            if((CoinStackManager.CurrentCoinHolder.transform.position- targetPosition).sqrMagnitude < 0.01f)
                CoinStackManagerStateMachine.ChangeState(CoinStackManager.IdleState);
        }
    }
}
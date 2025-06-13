using FiberCase.Gameplay;
using UnityEngine;

namespace FiberCase.Game_State
{
    public class CoinStackManagerMovingState : CoinStackManagerState
    {
        public CoinStackManagerMovingState(CoinStackManager coinStackManager, CoinStackManagerStateMachine coinStackManagerStateMachine) : base(coinStackManager, coinStackManagerStateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            Debug.Log("entered moving state");
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }
    }
}
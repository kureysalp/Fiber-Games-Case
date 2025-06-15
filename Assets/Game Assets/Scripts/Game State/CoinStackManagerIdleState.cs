using FiberCase.Gameplay;
using UnityEngine;

namespace FiberCase.Game_State
{
    public class CoinStackManagerIdleState : CoinStackManagerState
    {
        public CoinStackManagerIdleState(CoinStackManager coinStackManager, CoinStackManagerStateMachine coinStackManagerStateMachine) : base(coinStackManager, coinStackManagerStateMachine)
        {
            
        }

        public override void EnterState()
        {
            base.EnterState();
            
            // TODO: Hide input is busy icon. 
        }

        public override void ExitState()
        {
            base.ExitState();
            
            // TODO: Show input is busy icon.
        }

        public override async void UpdateState()
        {
            base.UpdateState();

            if (!CoinStackManager.PlayerInput.HasInput) return;

            var path = await CoinStackManager.GridManager.FindPathAsync(CoinStackManager.PlayerInput.InputMovePosition);
            if (path == null) return;
            
            CoinStackManager.SetStackMovePath(path);
            CoinStackManagerStateMachine.ChangeState(CoinStackManager.MovingState);
        }
    }
}
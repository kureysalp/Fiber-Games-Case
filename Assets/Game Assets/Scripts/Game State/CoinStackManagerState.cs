using FiberCase.Gameplay;

namespace FiberCase.Game_State
{
    public class CoinStackManagerState
    {
        protected CoinStackManager CoinStackManager;
        protected CoinStackManagerStateMachine CoinStackManagerStateMachine;

        public CoinStackManagerState(CoinStackManager coinStackManager, CoinStackManagerStateMachine coinStackManagerStateMachine)
        {
            CoinStackManager = coinStackManager;
            CoinStackManagerStateMachine = coinStackManagerStateMachine;
        }
        
        public virtual void EnterState() {}
        public virtual void ExitState() {}
        public virtual void UpdateState() {}
    }
}
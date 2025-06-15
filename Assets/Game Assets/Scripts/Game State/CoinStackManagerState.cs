using System.Threading.Tasks;
using FiberCase.Gameplay;
using UnityEngine;

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

        public virtual void EnterState()
        {
            Debug.Log("entered state "+ this);
        }
        public virtual void ExitState() {}
        public virtual void UpdateState() {}
        public virtual Task UpdateStateAsync() => Task.CompletedTask;

    }
}
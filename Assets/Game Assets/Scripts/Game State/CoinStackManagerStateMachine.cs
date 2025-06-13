namespace FiberCase.Game_State
{
    public class CoinStackManagerStateMachine
    {
        public CoinStackManagerState CurrentState { get; private set; }

        public void Initialize(CoinStackManagerState initialState)
        {
            CurrentState = initialState;
            CurrentState.EnterState();
        }

        public void ChangeState(CoinStackManagerState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}
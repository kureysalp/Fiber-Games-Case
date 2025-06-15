
namespace FiberCase.Event
{
    public struct CoinsPoppedEvent : IEvent
    {
        public int CoinAmount;
        public int CoinValue;
    }
    
    public struct ReadyForPlayerInputEvent : IEvent {}
    
    public struct GameWonEvent : IEvent {}
    public struct GameLostEvent : IEvent {}
    public struct PlayAgainEvent : IEvent {}
}
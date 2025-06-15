
namespace FiberCase.Event
{
    public struct CoinsPoppedEvent : IEvent
    {
        public int CoinAmount;
        public int CoinValue;
    }
    
}
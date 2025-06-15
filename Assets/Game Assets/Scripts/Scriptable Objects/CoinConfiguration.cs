using UnityEngine;
using UnityEngine.Serialization;

namespace FiberCase.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "SO_Coin_Configuration", menuName = "Scriptable Objects/Coin Configuration")]
    public class CoinConfiguration : ScriptableObject
    {
        [SerializeField] private float _coinMoveDuration;
        [SerializeField] private float _coinMoveHeight;
        [SerializeField] private float _coinStackHeight;
        [SerializeField] private int _coinPopAmount;

        public float CoinMoveDuration => _coinMoveDuration;
        public float CoinMoveHeight => _coinMoveHeight;
        public float CoinStackHeight => _coinStackHeight;
        public int CoinPopAmount => _coinPopAmount;

    }
}
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FiberCase.Gameplay
{
    [Serializable]
    public struct CoinStackPair
    {
        [SerializeField] private int _coinValue;
        [SerializeField] private int _coinCount;

        public int CoinValue => _coinValue;
        public int CoinCount => _coinCount;
    }
}
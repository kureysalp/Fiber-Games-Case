using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FiberCase.Gameplay
{
    [Serializable]
    public class CoinStack
    {
        [SerializeField] private CoinStackPair[] _coinStackPairs;

        public CoinStackPair[] CoinStackPairs => _coinStackPairs;
    }
}
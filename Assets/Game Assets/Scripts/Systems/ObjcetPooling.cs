using FiberCase.Gameplay;
using UnityEngine;

namespace FiberCase.Systems
{
    public class ObjcetPooling : MonoBehaviour
    {
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private int _coinPoolSize;
        [SerializeField] private int _coinPoolExpandSize;

        [SerializeField] private CoinHolder _coinHolderPrefab;
        [SerializeField] private int _coinHolderPoolSize;
        [SerializeField] private int _coinHolderPoolExpandSize;

        private void Awake()
        {
            Poolable.CreatePool<Coin>(_coinPrefab.gameObject, _coinPoolSize, _coinPoolExpandSize);
            Poolable.CreatePool<CoinHolder>(_coinHolderPrefab.gameObject, _coinHolderPoolSize, _coinHolderPoolExpandSize);
        }
    }
}
using UnityEngine;

namespace FiberCase.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "SO_Color_Coding", menuName = "Scriptable Objects/Color Coding")]
    public class CoinColorCoding : ScriptableObject
    {
        [SerializeField] private Color[] _colors;

        public Color[] Colors => _colors;
    }
}
using System;
using FiberCase.Systems;
using TMPro;
using UnityEngine;

namespace FiberCase.Gameplay
{
    public class Coin : Poolable
    {
        public int Value { get; private set; }
        
        [SerializeField] private TextMeshPro _valueText;
        
        private MaterialPropertyBlock _propertyBlock;
        private MeshRenderer _meshRenderer;
        
        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void SetValue(int value)
        {
            Value = value;
            _valueText.SetText(Value.ToString());
        }

        public void SetColor(Color color)
        {
            _meshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_BaseColor", color);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
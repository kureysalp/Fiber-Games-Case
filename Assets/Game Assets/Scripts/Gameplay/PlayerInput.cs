using System;
using FiberCase.Event;
using UnityEngine;

namespace FiberCase.Gameplay
{
    public class PlayerInput : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField] private LayerMask _nodeLayer;
        
        public Vector3 InputMovePosition { get; private set; }
        
        public bool HasInput { get; private set; }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            TouchGridNode();
        }

        private void TouchGridNode()
        {
            HasInput = false;

            if (Input.touchCount == 0) return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;

            var ray = _camera.ScreenPointToRay(touch.position);
            

            if (Physics.Raycast(ray, out var hit,Mathf.Infinity, _nodeLayer))
            {
                InputMovePosition = hit.point;
                HasInput = true;

                Debug.Log("touched something");
            }
        }   
    }
}
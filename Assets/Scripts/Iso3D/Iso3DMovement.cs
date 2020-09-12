using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Iso3D
{
    public class Iso3DMovement : MonoBehaviour
    {
        private static readonly int SpeedX = Animator.StringToHash("SpeedX");
        private static readonly int SpeedY = Animator.StringToHash("SpeedY");
    
        private const string DefaultSpriteName = "Sprite";
    
        public float moveSpeed = 4.5f;
        public float movementTolerance = 0.0001f;
        public GameObject spriteReference;
    
        private Animator _animator;
        private Rigidbody _rigidbody;
        private Vector2 _movement;

        private void Start()
        {
            if (spriteReference == null)
            {
                spriteReference = transform.Find(DefaultSpriteName).gameObject;
                Debug.LogWarning($"Please set the sprite reference for {name}");
                Debug.Assert(spriteReference != null);
            }

            _animator = spriteReference.GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var moveVals = new Vector3(_movement.x,0f, _movement.y);
            if (Math.Abs(_movement.x) > movementTolerance || Math.Abs(_movement.y) > movementTolerance)
            {
                _rigidbody.MovePosition(transform.position + moveVals.normalized * (Time.deltaTime * moveSpeed));
            }

            _animator.SetFloat(SpeedX, _movement.x);
            _animator.SetBool(SpeedY, Math.Abs(_movement.y) > movementTolerance);
        }

        public void Move(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }
    }
}

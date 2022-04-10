using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hide.Test
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class AnimationTestMovement : MonoBehaviour
    {
        public float speed = 300f;
        public float walkSpeedMultiplier = 1f;
        public float runSpeedMultiplier = 3f;
        public TextMeshProUGUI text;
        public List<AnimatorDetails> animatorControllerList;
        public float moveMagnitude = 6f;

        private PlayerInput _playerInput;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Vector3 _move;
        private int _controllerIndex;
        private float _lastX;
        private float _lastY;
        private bool _isAnimMoving;
        private bool _isControlMoving;
        private bool _isRunning;

        private readonly int _animMoveX = Animator.StringToHash("MoveX");
        private readonly int _animMoveY = Animator.StringToHash("MoveY");
        private readonly int _animIsMoving = Animator.StringToHash("IsMoving");
        private const float EPSILON = 0.001f;
        private const float SMALL_VALUE = 0.1f;

        private InputAction moveAction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            Debug.Assert(animatorControllerList.Count > 0,
                "There should be at least one animator controller in animatorControllerList");
            Debug.Assert(text != null, "Text should be set for displaying details");

            _animator.runtimeAnimatorController = animatorControllerList[_controllerIndex].controller;
            
            _playerInput = GetComponent<PlayerInput>();
            moveAction = _playerInput.currentActionMap["Move"];
            _playerInput.currentActionMap["Transform"].canceled += Transform;
            _playerInput.currentActionMap["Run"].started += delegate { _isRunning = true; };
            _playerInput.currentActionMap["Run"].canceled += delegate { _isRunning = false; };
        }

        private void Start()
        {
            DisplayInformation();
        }


        private void Transform(InputAction.CallbackContext context)
        {
            _controllerIndex = (_controllerIndex + 1) % animatorControllerList.Count;
            _animator.runtimeAnimatorController = animatorControllerList[_controllerIndex].controller;
            DisplayInformation();
        }

        private void FixedUpdate()
        {
            // physics
            _rigidbody.velocity =
                _move * (speed * Time.fixedDeltaTime * (_isRunning ? runSpeedMultiplier : walkSpeedMultiplier));
        }

        /// <summary>
        /// todo(TurnipXenon): document
        /// todo(TurnipXenon): possible improvement is to use enums instead of isMoving to readability
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="isMoving"></param>
        /// <returns></returns>
        private float NormalizeMoveForAnimation(float newValue, bool isMoving)
        {
            if (Mathf.Abs(newValue) < EPSILON)
            {
                // make old value smaller but not zero
                return 0f;
            }
            
            if (!isMoving)
            {
                return newValue > 0f ? SMALL_VALUE : -SMALL_VALUE;
            }

            return (newValue > 0f ? 1f : -1f)
                   * (_isRunning ? 2f : 1f);
        }

        private void Update()
        {
            // animations
            // this order is essential
            // todo(TurnipXenon): document
            _move = moveAction.ReadValue<Vector2>();

            var isMoving = _move.sqrMagnitude > 0f;
            if (isMoving)
            {
                // we want to normalize normally
                _lastY = NormalizeMoveForAnimation(_move.y, isMoving);
                _lastX = NormalizeMoveForAnimation(_move.x, isMoving);
            }
            else
            {
                // we want to minimize the normalization to activate idle animations
                _lastY = NormalizeMoveForAnimation(_lastY, isMoving);
                _lastX = NormalizeMoveForAnimation(_lastX, isMoving);
            }

            _animator.SetFloat(_animMoveY, _lastY);
            _animator.SetFloat(_animMoveX, _lastX);
        }

        private void DisplayInformation()
        {
            _animator.SetBool(_animIsMoving, _isAnimMoving);
            _animator.SetFloat(_animMoveX, _lastX);
            _animator.SetFloat(_animMoveY, _lastY);
            text.text = "===" +
                        $"\nCurrent animal: {animatorControllerList[_controllerIndex].name}";
        }

        public void ChangeWalkSpeed(string newText)
        {
            if (float.TryParse(newText, out var newSpeed))
            {
                walkSpeedMultiplier = newSpeed;
            }
        }

        public void ChangeRunSpeed(string newText)
        {
            if (float.TryParse(newText, out var newSpeed))
            {
                runSpeedMultiplier = newSpeed;
            }
        }
    }

    [Serializable]
    public class AnimatorDetails
    {
        public string name;
        public RuntimeAnimatorController controller;
    }
}
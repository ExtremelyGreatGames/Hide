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

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            Debug.Assert(animatorControllerList.Count > 0,
                "There should be at least one animator controller in animatorControllerList");
            Debug.Assert(text != null, "Text should be set for displaying details");

            _animator.runtimeAnimatorController = animatorControllerList[_controllerIndex].controller;
        }

        private void OnEnable()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.currentActionMap["Move"].started += PlayerOnMove;
            _playerInput.currentActionMap["Move"].performed += PlayerOnMove;
            _playerInput.currentActionMap["Move"].canceled += PlayerOnMove;
            _playerInput.currentActionMap["Transform"].canceled += Transform;
            _playerInput.currentActionMap["Run"].started += delegate { _isRunning = true; };
            _playerInput.currentActionMap["Run"].canceled += delegate { _isRunning = false; };
        }

        private void Start()
        {
            DisplayInformation();
        }

        // Fun fact: you can't user OnMove because it's reserved for Unity but they won't tell
        // why there's an error :D
        private void PlayerOnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
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

        private void Update()
        {
            // animations
            // this order is essential
            // todo(TurnipXenon): document
            if (_move.sqrMagnitude > 0.001f)
            {
                _lastY = _move.y * (_isRunning ? 5f : 1f);
                _lastX = _move.x * (_isRunning ? 5f : 1f);
            }

            _isAnimMoving = Mathf.Abs(_lastX) > 0.01f;
            _animator.SetFloat(_animMoveY, _lastY);
            _animator.SetFloat(_animMoveX, _lastX);
            _animator.SetBool(_animIsMoving, _isAnimMoving);
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
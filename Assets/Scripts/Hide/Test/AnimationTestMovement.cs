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
        private int _controllerIndex = 0;
        private float _lastX = 0;
        private bool _isMoving = false;
        private bool _isRunning = false;

        private readonly int _animMoveX = Animator.StringToHash("MoveX");
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
            _playerInput.currentActionMap["Move"].performed += PlayerOnMove;
            _playerInput.currentActionMap["Move"].canceled += PlayerOnMove;
            _playerInput.currentActionMap["Transform"].performed += Transform;
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
            _rigidbody.velocity =
                _move * (speed * Time.fixedDeltaTime * (_isRunning ? runSpeedMultiplier : walkSpeedMultiplier));

            _isMoving = Mathf.Abs(_move.x) > 0.01f;
            if (_isMoving)
            {
                _animator.SetFloat(_animMoveX, _move.x * (_isRunning ? 5f : 1f));
            }

            _animator.SetBool(_animIsMoving, _isMoving);
        }

        private void DisplayInformation()
        {
            _animator.SetFloat(_animMoveX, _lastX);
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
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Hide.Test {
  [RequireComponent(typeof(PlayerInput))]
  [RequireComponent(typeof(Collider2D))]
  [RequireComponent(typeof(Rigidbody2D))]
  [RequireComponent(typeof(Animator))]
  public class AnimationTestMovement : MonoBehaviour {
    public float speed = 1f;
    public bool shouldAnimateFull = true;
    public bool matchSpeed = true;
    public TextMeshProUGUI text;
    public List<AnimatorDetails> animatorControllerList;

    private PlayerInput _playerInput;
    private Vector3 _move;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private int _controllerIndex = 0;
    private float _lastX = 0;

    private readonly int _animMoveX = Animator.StringToHash("MoveX");
    private readonly int _animFullMotion = Animator.StringToHash("FullMotion");
    private readonly int _animMatchSpeed = Animator.StringToHash("MatchSpeed");

    private void Awake() {
      _rigidbody = GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();

      Debug.Assert(animatorControllerList.Count > 0, "There should be at least one animator controller in animatorControllerList");
      Debug.Assert(text != null, "Text should be set for displaying details");

      _animator.runtimeAnimatorController = animatorControllerList[_controllerIndex].controller;
    }

    private void OnEnable() {
      _playerInput = GetComponent<PlayerInput>();
      _playerInput.currentActionMap["Move"].performed += PlayerOnMove;
      _playerInput.currentActionMap["Move"].canceled += PlayerOnMove;
      _playerInput.currentActionMap["ToggleAnimation"].performed += ToggleAnimation;
      _playerInput.currentActionMap["Transform"].performed += Transform;
      _playerInput.currentActionMap["ToggleMatchSpeed"].performed += ToggleMatchSpeed;
    }

    private void Start() {
      DisplayInformation();
    }

    // Fun fact: you can't user OnMove because it's reserved for Unity but they won't tell
    // why there's an error :D
    private void PlayerOnMove(InputAction.CallbackContext context) {
      _move = context.ReadValue<Vector2>();
    }

    private void ToggleAnimation(InputAction.CallbackContext context) {
      shouldAnimateFull = !shouldAnimateFull;
      DisplayInformation();
    }

    private void Transform(InputAction.CallbackContext context) {
      _controllerIndex = (_controllerIndex + 1)%animatorControllerList.Count;
      _animator.runtimeAnimatorController = animatorControllerList[_controllerIndex].controller;
      DisplayInformation();
    }
    private void ToggleMatchSpeed(InputAction.CallbackContext context) {
      matchSpeed = !matchSpeed;
      DisplayInformation();
    }

    private void FixedUpdate() {
      _rigidbody.velocity = _move*(speed*Time.fixedDeltaTime);

      if (Mathf.Abs(_rigidbody.velocity.x) > 0.01f) {
        _lastX = _rigidbody.velocity.x;
        _animator.SetFloat(_animMoveX, _lastX);
      }
    }

    private string CurrentFrameRate() {
      return shouldAnimateFull ? "Frame cycle: 8" : "Frame cycle: 4";
    }

    private static string YesOrNo(bool b) {
      return b ? "Yes" : "No";
    }

    private void DisplayInformation() {
      _animator.SetBool(_animFullMotion, shouldAnimateFull);
      _animator.SetFloat(_animMoveX, _lastX);
      _animator.SetFloat(_animMatchSpeed, matchSpeed ? 1f : 2f);
      text.text = "===" +
                  $"\nCurrent animal: {animatorControllerList[_controllerIndex].name}" +
                  $"\n{CurrentFrameRate()}" +
                  $"\nHalf frame match speed? {YesOrNo(matchSpeed)}";
    }
  }

  [Serializable]
  public class AnimatorDetails {
    public string name;
    public AnimatorController controller;
  }
}

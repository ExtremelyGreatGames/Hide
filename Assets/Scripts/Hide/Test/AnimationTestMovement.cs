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
    public TextMeshProUGUI text;
    public List<AnimatorDetails> animatorControllerList;

    private PlayerInput _playerInput;
    private Vector3 _move;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private int controllerIndex = 0;
    private float lastX = 0;

    private int ANIM_MOVE_X = Animator.StringToHash("MoveX");
    private int ANIM_FULL_MOTION = Animator.StringToHash("FullMotion");

    private void Awake() {
      _rigidbody = GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();

      Debug.Assert(animatorControllerList.Count > 0, "There should be at least one animator controller in animatorControllerList");
      Debug.Assert(text != null, "Text should be set for displaying details");

      _animator.runtimeAnimatorController = animatorControllerList[controllerIndex].controller;
    }

    private void OnEnable() {
      _playerInput = GetComponent<PlayerInput>();
      _playerInput.currentActionMap["Move"].performed += PlayerOnMove;
      _playerInput.currentActionMap["Move"].canceled += PlayerOnMove;
      _playerInput.currentActionMap["ToggleAnimation"].performed += ToggleAnimation;
      _playerInput.currentActionMap["Transform"].performed += Transform;
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
      _animator.SetBool(ANIM_FULL_MOTION, shouldAnimateFull);
      DisplayInformation();
    }

    private void Transform(InputAction.CallbackContext context) {
      controllerIndex = (controllerIndex + 1)%animatorControllerList.Count;
      _animator.runtimeAnimatorController = animatorControllerList[controllerIndex].controller;
      _animator.SetBool(ANIM_FULL_MOTION, shouldAnimateFull);
      _animator.SetFloat(ANIM_MOVE_X, lastX);
      DisplayInformation();
    }

    private void FixedUpdate() {
      _rigidbody.velocity = _move*(speed*Time.fixedDeltaTime);

      if (Mathf.Abs(_rigidbody.velocity.x) > 0.01f) {
        lastX = _rigidbody.velocity.x;
        _animator.SetFloat(ANIM_MOVE_X, lastX);
      }
    }

    private string CurrentFrameRate() {
      return shouldAnimateFull ? "Frame cycle: 8" : "Frame cycle: 4";
    }

    private void DisplayInformation() {
      text.text = $"===\nCurrent animal: {animatorControllerList[controllerIndex].name}\n{CurrentFrameRate()}";
    }
  }

  [Serializable]
  public class AnimatorDetails {
    public string name;
    public AnimatorController controller;
  }
}

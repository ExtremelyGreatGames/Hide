using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float moveSpeed;

    [SerializeField]
    private GameObject _sprite;

    private Vector2 _inputValues;

    private PlayerInput playerInput;
    private HidePlayerInput inputKeyboard;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        inputKeyboard = new HidePlayerInput();
        inputKeyboard.Player.Enable();
    }

    private void Update()
    {
        _inputValues = inputKeyboard.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)_inputValues.normalized * moveSpeed * Time.deltaTime;
    }
}

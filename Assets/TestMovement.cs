using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public float _moveSpeed = 5f;

    Vector2 _moveInput = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        var inputVals = Vector2.zero;
        inputVals.x = Input.GetAxisRaw("Horizontal");
        inputVals.y = Input.GetAxisRaw("Vertical");

        _moveInput = inputVals.normalized;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)_moveInput * Time.fixedDeltaTime * _moveSpeed;
    }
}

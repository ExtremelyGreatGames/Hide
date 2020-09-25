using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5.0f;

    Inputs controls;
    Vector2 _move;

    Vector2 test_move;

    void Awake()
    {
        /*****************************************************/
        /* Need to find a way to switch between hider/seeker */
        /*****************************************************/

        controls = new Inputs();

        controls.Hider.Movement.performed += ctx => _move = ctx.ReadValue<Vector2>();
        controls.Hider.Movement.canceled += ctx => _move = Vector2.zero;
    }
    
    void OnEnable()
    {
        controls.Hider.Enable();
    }

    void OnDisable()
    {
        controls.Hider.Disable();
    }

    void Update() {
        if (!isLocalPlayer) return;

        transform.Translate(_move * Time.deltaTime * moveSpeed);
    }
}

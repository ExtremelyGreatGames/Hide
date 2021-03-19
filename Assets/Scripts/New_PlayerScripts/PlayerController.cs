using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    // Object References
    InputHandler inputHandler;
    SpriteRenderer spriteRenderer;

    Vector2 moveVals;

    public float moveSpeed;

    private void Start()
    {
        inputHandler = gameObject.GetComponent<InputHandler>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        moveVals = inputHandler.GetMoveVals();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Forced transform");
            OnSpeechRecognized("cow");
        }
    }

    private void FixedUpdate()
    {
        if (isServer) {
            return;
        }

        transform.Translate(moveVals.normalized * moveSpeed * Time.deltaTime);
    }

    public void OnSpeechRecognized(string animalKey) 
    {
        Debug.Log(animalKey);

        // switch on animal key, change sprite
        switch (animalKey)
        {
            case "cow":
                spriteRenderer.color = Color.red;
                break;
            case "pig":
                spriteRenderer.color = Color.blue;
                break;
            case "chicken":
                spriteRenderer.color = Color.green;
                break;
            default:
                //spriteRenderer.color = Color.white;
                break;
        }
    }

    // RPC? maybe need to pass player identity? Maybe RPC the OnSpeechRecognized?
    private void ChangeSprite()
    { 
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    // Object References
    InputHandler inputHandler;
    Animator animator;

    Vector2 moveVals;

    public float moveSpeed;

    private void Start()
    {
        inputHandler = gameObject.GetComponent<InputHandler>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!hasAuthority) { return; }

        moveVals = inputHandler.GetMoveVals();
        animator.SetFloat("MoveX", moveVals.x);
        animator.SetFloat("MoveY", moveVals.y);

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Forced transform");
            OnSpeechRecognized("cow");
        }
    }

    private void FixedUpdate()
    {
        if (!hasAuthority) { return; }
        transform.Translate(moveVals.normalized * moveSpeed * Time.deltaTime);
    }

    [Client]
    public void OnSpeechRecognized(string animalKey) 
    {
        if (!hasAuthority) { return; }

        //Debug.Log(animalKey);
        CmdChangeAnimal(animalKey);
    }

    [Command]
    private void CmdChangeAnimal(string animalKey) 
    {
        RpcChangeAnimal(animalKey);
    }

    // RPC? maybe need to pass player identity? Maybe RPC the OnSpeechRecognized?
    [ClientRpc]
    private void RpcChangeAnimal(string animalKey)
    {
        switch (animalKey)
        {
            case "cow":
                animator.SetTrigger("TriggerCow");
                animator.ResetTrigger("TriggerChicken");
                animator.ResetTrigger("TriggerPig");
                break;
            case "pig":
                animator.ResetTrigger("TriggerCow");
                animator.ResetTrigger("TriggerChicken");
                animator.SetTrigger("TriggerPig");
                break;
            case "chicken":
                animator.ResetTrigger("TriggerCow");
                animator.SetTrigger("TriggerChicken");
                animator.ResetTrigger("TriggerPig");
                break;
            default:
                break;
        }
    }
}

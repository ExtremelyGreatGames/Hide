using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Object References
    private KeywordHandler keywordHandler;

    private float moveH;
    private float moveV;

    private void Start()
    {
        keywordHandler = gameObject.GetComponent<KeywordHandler>();
    }

    private void Update()
    {
        // TODO change this into the new Unity Input system
        moveH = Input.GetAxisRaw("Horizontal");
        moveV = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.J)) {
            keywordHandler.StartSpeechRecognizer();
        }

        if (Input.GetKeyUp(KeyCode.J)) {
            keywordHandler.StopSpeechRecognizer();
        }
    }

    public Vector2 GetMoveVals()
    {
        return new Vector2(moveH, moveV);
    }

    public void StartSpeaking()
    {
        keywordHandler.StartSpeechRecognizer();
    }

    public void StopSpeaking()
    {
        keywordHandler.StopSpeechRecognizer();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed;

    Inputs controls;
    Vector2 _move;
    Animator animator;
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, string> keywordDict = new Dictionary<string, string>();

    // ================== Input System Setup ============================
    void Awake()
    {
        controls = new Inputs();

        controls.Hider.Movement.performed += ctx => _move = ctx.ReadValue<Vector2>();
        controls.Hider.Movement.canceled += ctx => _move = Vector2.zero;

        controls.Hider.Speak.performed += ctx => TestPress();
        controls.Hider.Speak.canceled += ctx => TestRelease();
    }
    
    void OnEnable()
    {
        controls.Hider.Enable();
    }

    void OnDisable()
    {
        controls.Hider.Disable();
    }
    // ==================================================================
    

    void Start()
    {
        animator = GetComponent<Animator>();

        // populate keywords dictionary. *there has to be a better way to do this*
        keywordDict.Add("moo","Cow");
        keywordDict.Add("oink","Pig");
        keywordDict.Add("cluck","Chicken");

        keywordRecognizer = new KeywordRecognizer(keywordDict.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized += speechRecognized;

        Debug.Log(keywordDict.Keys.ToArray());
    }

    void Update()
    {
        transform.Translate(_move * Time.deltaTime * moveSpeed);
    }

    void TestPress() {
        Debug.Log("Started recognizer");
        keywordRecognizer.Start();
    }

    void TestRelease() {
        Debug.Log("Stopped recognizer");
        keywordRecognizer.Stop();
    }

    void speechRecognized(PhraseRecognizedEventArgs args) {
        animator.SetTrigger("Change" + keywordDict[args.text]);
        Debug.Log("speech was recognized");
    }

}

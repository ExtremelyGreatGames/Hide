using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Mirror;

public class HiderScript : NetworkBehaviour
{
    Inputs controls;
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, string> keywordDict = new Dictionary<string, string>();

    KeywordHandler keywordHandler;

    void Awake()
    {
        /*****************************************************/
        /* Need to find a way to switch between hider/seeker */
        /*****************************************************/

        controls = new Inputs();

        controls.Hider.Speak.performed += ctx => OnSpeakPressed();
        controls.Hider.Speak.canceled += ctx => OnSpeakReleased();
    }

    void OnSpeakPressed()  {keywordHandler.keywordRecognizer.Start(); Debug.Log("Mic on");}
    void OnSpeakReleased() => keywordHandler.keywordRecognizer.Stop();

    void OnEnable() => controls.Hider.Enable();
    void OnDisable() => controls.Hider.Disable();

    void Start()
    {
        // // populate keywords dictionary. *there has to be a better way to do this*
        keywordDict.Add("moo","Cow");
        keywordDict.Add("oink","Pig");
        keywordDict.Add("cluck","Chicken");
        keywordDict.Add("cow","Cow");

        // keywordRecognizer = new KeywordRecognizer(keywordDict.Keys.ToArray());
        keywordHandler = GameObject.Find("KeywordHandler").GetComponent<KeywordHandler>();
        keywordRecognizer = keywordHandler.GetKeywordRecognizer();

        //keywordRecognizer.OnPhraseRecognized += speechRecognized;
        keywordHandler.keywordRecognizer.OnPhraseRecognized += speechRecognized;
        //Debug.Log(keywordDict.Keys.ToArray());
    }

    [Client]
    void speechRecognized(PhraseRecognizedEventArgs args) {
        Color c = Color.black;
        switch (keywordDict[args.text]) {
            case "Cow":
                c = Color.green;
                break;
            case "Pig":
                c = Color.red;
                break;
            case "Chicken":
                c = Color.blue;
                break;
        }
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) {
            // send information back
            CmdTransform(c);
            // change on local
            sr.color = c;
        }
        
    }

    // TODO replace with real transformation code
    [Command]
    void CmdTransform(Color c) {
        // Color c = Color.black;
        // switch (keywordDict[text]) {
        //     case "Cow":
        //         c = Color.green;
        //         break;
        //     case "Pig":
        //         c = Color.red;
        //         break;
        //     case "Chicken":
        //         c = Color.blue;
        //         break;
        // }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) {
            sr.color = c;
        }
    }
}

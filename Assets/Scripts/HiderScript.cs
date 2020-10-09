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
    KeywordRecognizer keywordRecognizer2;
    Dictionary<string, string> keywordDict = new Dictionary<string, string>();

    void Awake()
    {
        /*****************************************************/
        /* Need to find a way to switch between hider/seeker */
        /*****************************************************/

        controls = new Inputs();

        controls.Hider.Speak.performed += ctx => OnSpeakPressed();
        controls.Hider.Speak.canceled += ctx => OnSpeakReleased();
    }

    void OnSpeakPressed() => keywordRecognizer.Start();
    void OnSpeakReleased() => keywordRecognizer.Stop();

    void OnEnable() => controls.Hider.Enable();
    void OnDisable() => controls.Hider.Disable();

    void Start()
    {
        // populate keywords dictionary. *there has to be a better way to do this*
        keywordDict.Add("moo","Cow");
        keywordDict.Add("oink","Pig");
        keywordDict.Add("cluck","Chicken");
        keywordDict.Add("cow","Cow");

        keywordRecognizer = new KeywordRecognizer(keywordDict.Keys.ToArray());

        // Dictionary<string, string> copyDict = keywordDict;
        // copyDict.Add("test","Test");

        // keywordRecognizer2 = new KeywordRecognizer(copyDict.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized += speechRecognized;
        // keywordRecognizer2.OnPhraseRecognized += speechRecognized;
        //initKeywordRecognizer();
        //Debug.Log(keywordDict.Keys.ToArray());
    }

    [Client]
    void speechRecognized(PhraseRecognizedEventArgs args) {
        CmdTransform(args.text);
    }

    // TODO replace with real transformation code
    [Command]
    void CmdTransform(string text) {
        Color c = Color.black;
        switch (keywordDict[text]) {
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
            sr.color = c;
        }
    }

    [Client]
    void initKeywordRecognizer() {
        // populate keywords dictionary. *there has to be a better way to do this*
        keywordDict.Add("moo","Cow");
        keywordDict.Add("oink","Pig");
        keywordDict.Add("cluck","Chicken");
        keywordDict.Add("cow","Cow");

        keywordRecognizer = new KeywordRecognizer(keywordDict.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized += speechRecognized;
    }
}

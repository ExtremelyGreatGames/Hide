﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;
#endif

public class Transformation : MonoBehaviour
{
    // need sprite assets

#if UNITY_STANDALONE_WIN
    Animator animator;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, string> keywordDict = new Dictionary<string, string>();
    private string[] temp_wordlist;

    void Start()
    {
        animator = GetComponent<Animator>();
        temp_wordlist = new string[]{"moo","oink","cluck"};

        keywordDict.Add("moo","Cow");
        keywordDict.Add("oink","Pig");
        keywordDict.Add("cluck","Chicken");

        //keywordRecognizer = new KeywordRecognizer(temp_wordlist);
        keywordRecognizer = new KeywordRecognizer(keywordDict.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized += speechRecognized;
    }

    void Update()
    {
        return;
        // todo implement this
        
        if (Input.GetButtonDown("Speak")) keywordRecognizer.Start();
        if (Input.GetButtonUp("Speak"))   keywordRecognizer.Stop();
    }

    void speechRecognized(PhraseRecognizedEventArgs args) {
        //Debug.Log(args.text);
        //Debug.Log("YOOO");
        animator.SetTrigger("Change" + keywordDict[args.text]);
    }
#endif // UNITY_STANDALONE_WIN
}

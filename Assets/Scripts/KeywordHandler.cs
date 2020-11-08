using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordHandler : MonoBehaviour
{
    public KeywordRecognizer keywordRecognizer;
    Dictionary<string, string> keywordDict = new Dictionary<string, string>();

    void Start() {
        // populate keywords dictionary. *there has to be a better way to do this*
        keywordDict.Add("moo", "Cow");
        keywordDict.Add("oink", "Pig");
        keywordDict.Add("cluck", "Chicken");
        keywordDict.Add("cow", "Cow");

        keywordRecognizer = new KeywordRecognizer(keywordDict.Keys.ToArray());
    }

    public KeywordRecognizer GetKeywordRecognizer() {
        return keywordRecognizer;
    }

    public void StartKeywordRecognizer () {
        // populate keywords dictionary. *there has to be a better way to do this*
        keywordDict.Add("moo", "Cow");
        keywordDict.Add("oink", "Pig");
        keywordDict.Add("cluck", "Chicken");
        keywordDict.Add("cow", "Cow");

        keywordRecognizer = new KeywordRecognizer(keywordDict.Keys.ToArray());
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordHandler : MonoBehaviour
{
    // Object References
    private PlayerController playerController;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, string> keywords;

    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();

        keywords = new Dictionary<string, string>()
        {
            {"moo", "cow"},
            {"oink", "pig"},
            {"cluck", "chicken"}
        };

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
    }

    public void StartSpeechRecognizer() 
    {
        keywordRecognizer.Start();
    }

    public void StopSpeechRecognizer() 
    {
        keywordRecognizer.Stop();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args) 
    {

        playerController.OnSpeechRecognized(keywords[args.text]);
    }
}

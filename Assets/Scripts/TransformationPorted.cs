using System.Collections.Generic;
using System.Linq;
using Hide.Speech;
using UnityEngine;

public class TransformationPorted : MonoBehaviour
{
    // need sprite assets

    private Animator _animator;
    private SpeechRecognizer _speechRecognizer;

    private readonly Dictionary<string, string> _keywordDict = new Dictionary<string, string>
    {
        {"testing", "Cow"}, {"oink", "Pig"}, {"one", "Chicken"}
    };

    public void OnSpeechRecognizerPrepared()
    {
        _animator = GetComponent<Animator>();
        _speechRecognizer = GetComponent<SpeechRecognizer>();
        _speechRecognizer.SetKeyword(_keywordDict.Keys.ToArray());
        // keywordRecognizer.OnPhraseRecognized += speechRecognized; is assigned through editor
        // no buttons in Android too lazy too configure
#if UNITY_ANDROID
        _speechRecognizer.StartListening();
#endif // UNITY_ANDROID
    }

    private void Update()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetButtonDown("Speak")) _speechRecognizer.StartListening();
        if (Input.GetButtonUp("Speak")) _speechRecognizer.StopListening();
#endif // UNITY_STANDALONE_WIN
    }

    public void SpeechRecognized(HidePhraseRecognitionArgs args)
    {
        Debug.Log(args.text);
        _animator.SetTrigger("Change" + _keywordDict[args.text]);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Hide.Speech
{
    public class SpeechRecognizer : MonoBehaviour
    {
        [Tooltip("Assign a function to this to know when you can put keywords and start listening")]
        public UnityEvent onPreparedEvent;
        [Tooltip("Assign functions that takes a HidePhraseArgument as an argument")]
        public OnSpeechRecognizedEvent onSpeechRecognizedEvent;
        
        private ISpeechRecognition _speechRecognition;

        private void Awake()
        {
            _speechRecognition = SpeechRecognitionFactory.Create();
            _speechRecognition.OnPrepared += OnPrepared;
        }

        public void SetKeyword(string[] keywords)
        {
            if (_speechRecognition.IsUsable())
            {
                _speechRecognition.SetKeyword(keywords);
            }
        }

        public void StartListening()
        {
            if (_speechRecognition.IsUsable())
            {
                _speechRecognition.StartListening();
            }
        }

        public void StopListening()
        {
            if (_speechRecognition.IsUsable())
            {
                _speechRecognition.StopListening();
            }
        }

        private void OnPrepared()
        {
            if (_speechRecognition.IsUsable())
            {
                _speechRecognition.OnPhraseRecognized += OnSpeechRecognized;
                onPreparedEvent.Invoke();
            }
            else
            {
                Debug.LogWarning("Speech recognition not usable.");
            }
        }

        private void OnSpeechRecognized(HidePhraseRecognitionArgs args)
        {
            onSpeechRecognizedEvent.Invoke(args);
        }

        [Serializable]
        public class OnSpeechRecognizedEvent : UnityEvent<HidePhraseRecognitionArgs> { /* empty */ }
    }
}
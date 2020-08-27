using System;
using UnityEngine;
using UnityEngine.Windows.Speech;
#if UNITY_STANDALONE_WIN

#endif // UNITY_STANDALONE_WIN

namespace Hide.Speech.Windows
{
    public class WindowsSpeechRecognition : MonoBehaviour, ISpeechRecognition
    {
        public event HidePhraseRecognitionArgs.RecognizedDelegate OnPhraseRecognized;
        public event HidePhraseRecognitionArgs.OnPreparedDelegate OnPrepared;

        private KeywordRecognizer _keywordRecognizer;

        private void Start()
        {
#if UNITY_STANDALONE_WIN
            var onPrepared = OnPrepared;
            onPrepared?.Invoke();
#endif // UNITY_STANDALONE_WIN
        }

        public bool IsUsable()
        {
#if UNITY_STANDALONE_WIN
            return true; // no special setups
#else
            return false;
#endif
        }

        public void SetKeyword(string[] keywordList)
        {
#if UNITY_STANDALONE_WIN
            _keywordRecognizer = new KeywordRecognizer(keywordList);
            _keywordRecognizer.OnPhraseRecognized += SpeechRecognized;
#endif // UNITY_STANDALONE_WIN
        }

        public void StartListening()
        {
#if UNITY_STANDALONE_WIN
            _keywordRecognizer.Start();
#endif // UNITY_STANDALONE_WIN
        }

        public void StopListening()
        {
#if UNITY_STANDALONE_WIN
            _keywordRecognizer.Stop();
#endif // UNITY_STANDALONE_WIN
        }

#if UNITY_STANDALONE_WIN
        private void SpeechRecognized(PhraseRecognizedEventArgs args)
        {
            var phraseRecognized = OnPhraseRecognized;
            if (phraseRecognized == null) return;

            var translatedArgs = new HidePhraseRecognitionArgs();
            
            switch (args.confidence)
            {
                case ConfidenceLevel.High:
                    translatedArgs.confidenceLevel = 3 / 4f;
                    break;
                case ConfidenceLevel.Medium:
                    translatedArgs.confidenceLevel = 1 / 2f;
                    break;
                case ConfidenceLevel.Low:
                    translatedArgs.confidenceLevel = 1 / 4f;
                    break;
                case ConfidenceLevel.Rejected:
                    translatedArgs.confidenceLevel = 0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            translatedArgs.text = args.text;
            
            phraseRecognized(translatedArgs);
        }
#endif // UNITY_STANDALONE_WIN
    }
}
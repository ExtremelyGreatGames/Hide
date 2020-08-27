using System;
using Hide.Speech.Android;
using Hide.Speech.Windows;
using UnityEngine;

namespace Hide.Speech
{
    public static class SpeechRecognitionFactory
    {
        public static ISpeechRecognition Create()
        {
            var gameObject = new GameObject {name = "SpeechRecognitionCarrier"};

#if UNITY_ANDROID
            gameObject.AddComponent<HideAndroidSpeechRecognitionPlugin>();
            var speechRecognition = (ISpeechRecognition)gameObject
                .GetComponent<HideAndroidSpeechRecognitionPlugin>();
#elif UNITY_STANDALONE_WIN
            gameObject.AddComponent<WindowsSpeechRecognition>();
            var speechRecognition = (ISpeechRecognition) gameObject
                .GetComponent<WindowsSpeechRecognition>();
#else
            throw new NotImplementedException();
#endif // UNKNOWN

            return speechRecognition;
        }
    }
}
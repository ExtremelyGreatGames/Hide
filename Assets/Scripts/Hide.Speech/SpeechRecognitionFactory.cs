using UnityEngine;
using NotImplementedException = System.NotImplementedException;
#if UNITY_ANDROID
using Hide.Speech.Android;
#endif // UNITY_ANDROID

#if UNITY_STANDALONE_WIN
using Hide.Speech.Windows;

#endif // UNITY_STANDALONE_WIN

namespace Hide.Speech {
  public static class SpeechRecognitionFactory {
    public static ISpeechRecognition Create() {
      var gameObject = new GameObject {name = "SpeechRecognitionCarrier"};

#if UNITY_ANDROID
            gameObject.AddComponent<HideAndroidSpeechRecognitionPlugin>();
            var speechRecognition = (ISpeechRecognition)gameObject
                .GetComponent<HideAndroidSpeechRecognitionPlugin>();
#elif UNITY_STANDALONE_WIN
            gameObject.AddComponent<WindowsSpeechRecognition>();
            var speechRecognition = (ISpeechRecognition) gameObject
                .GetComponent<WindowsSpeechRecognition>();
#elif UNITY_WEBGL
      var speechRecognition = new WebGLMockSpeechRecognition();
#else
            throw new NotImplementedException();
#endif // UNKNOWN

      return speechRecognition;
    }
  }

#if UNITY_WEBGL
  // Make WEBGL not complain
  public class WebGLMockSpeechRecognition : ISpeechRecognition {
    public event HidePhraseRecognitionArgs.RecognizedDelegate OnPhraseRecognized;
    public event HidePhraseRecognitionArgs.OnPreparedDelegate OnPrepared;

    public bool IsUsable() {
      throw new NotImplementedException();
    }

    public void SetKeyword(string[] keywordList) {
      throw new NotImplementedException();
    }

    public void StartListening() {
      throw new NotImplementedException();
    }

    public void StopListening() {
      throw new NotImplementedException();
    }
  }
#endif // UNITY_WEBGL
}

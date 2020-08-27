using UnityEngine;
using UnityEngine.Events;

namespace Android
{
    public class HideAndroidSpeechRecognitionPlugin : MonoBehaviour
    {
        public UnityEvent onPrepared;
        
        private const string PluginName = "com.condimentalgames.hide.androidmodule.SpeechRecognizerFragment";
        private const string FuncCreateFromUnity = "createFromUnity";
        private const string FuncSetKeywords = "setKeywords";
        private const string FuncSetPollingRate = "setPollingRate";
        private const string FuncSetNoMatchErrorTolerance = "setNoMatchErrorTolerance";
        private const string FuncSetMaxResultCount = "setMaxResultCount";
        private const string FuncIsSpeechRecognitionAvailable = "isSpeechRecognitionAvailable";
        private const string FuncInitializeSpeechRecognizer = "initializeSpeechRecognizer";
        private const string FuncStartListening = "startListening";
        private const string FuncDestroySpeechRecognizer = "destroySpeechRecognizer";
        
        private AndroidJavaObject _joPlugin;

        private void Start()
        {
            using (var jc = new AndroidJavaClass(PluginName))
            {
                _joPlugin = jc.CallStatic<AndroidJavaObject>(FuncCreateFromUnity);
            }

            IsReady = true;
            onPrepared.Invoke();
        }
        
        public bool IsReady { private set; get; }

        public void SetKeyword(string[] keywordList)
        {
            Debug.Assert(_joPlugin != null, 
                "_joPlugin != null: Use OnPrepared to ensure the plugin is ready");
            _joPlugin.Call(FuncSetKeywords, AndroidCSUtility.JavaArrayFromCS(keywordList));
        }
    }
}
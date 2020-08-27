using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

namespace Android
{
    public class HideAndroidSpeechRecognitionPlugin : MonoBehaviour
    {
        #if PLATFORM_ANDROID
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
        private const string FuncAddUnityListener = "addUnityListener";

        private const string FuncOnSpeechRecognized = "OnSpeechRecognizedAndroid";
        
        public UnityEvent onPrepared;
        public event PhraseRecognizedDelegate OnPhraseRecognized;
        
        private AndroidJavaObject _joPlugin;
        private GameObject _dialog;
        private bool _isInitialized = false;

        private void Start()
        {
            IsReady = false;
            
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
                _dialog = new GameObject();
            }
        }

        // todo: Remove reliance on OnGUI which is expensive
        private void OnGUI()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
        }

        /// <summary>
        /// DO NOT COPY THE NAME. THE REFLECTION SYSTEM WILL BREAK.
        /// </summary>
        /// <param name="jsonArgs"></param>
        public void OnSpeechRecognizedAndroid(string jsonArgs)
        {
            PhraseRecognizedDelegate phraseRecognized = OnPhraseRecognized;
            
            if (phraseRecognized == null) { return; }

            var args = new HidePhraseRecognitionArgs();
            args.FromJsonOverwrite(jsonArgs);
            phraseRecognized(args);
        }

        public bool IsPluginAvailable => _joPlugin != null;
        public bool HasPermissionToListen { private set; get; }
        public bool HasSpeechRecognition { private set; get; }
        public bool IsReady { private set; get; }
        public bool IsUsable => IsReady && IsPluginAvailable && HasPermissionToListen && HasSpeechRecognition;

        private void Initialize()
        {
            HasPermissionToListen = Permission.HasUserAuthorizedPermission(Permission.Microphone);
            if (!HasPermissionToListen)
            {
                _dialog.AddComponent<HASRPPermissionRationaleDialog>();
                return;
            }

            if (_dialog != null)
            {
                Destroy(_dialog);
            }

            using (var jc = new AndroidJavaClass(PluginName))
            {
                _joPlugin = jc.CallStatic<AndroidJavaObject>(FuncCreateFromUnity);
            }

            if (IsPluginAvailable)
            {
                HasSpeechRecognition = _joPlugin.Call<bool>(FuncIsSpeechRecognitionAvailable);
            }
            
            if (HasSpeechRecognition)
            {
                _joPlugin.Call(FuncAddUnityListener, name, FuncOnSpeechRecognized);
                IsReady = _joPlugin.Call<bool>(FuncInitializeSpeechRecognizer);
            }
            
            onPrepared.Invoke();
            _isInitialized = true;
        }

        public void SetKeyword(string[] keywordList)
        {
            Debug.Assert(_joPlugin != null, 
                "_joPlugin != null: Use OnPrepared to ensure the plugin is ready");
            _joPlugin.Call(FuncSetKeywords, AndroidCSUtility.JavaArrayFromCS(keywordList));
        }

        public void StartListening()
        {
            Debug.Assert(IsUsable,
                $"IsUsable: Either IsReady ({IsReady}), IsPluginAvailable " +
                $"({IsPluginAvailable}), HasPermissionToListen ({HasPermissionToListen}), " +
                $"HasSpeechRecognition ({HasSpeechRecognition}) is false");
            _joPlugin.Call(FuncStartListening);
        }
        
        /// <summary>
        ///   <para>Delegate for OnPhraseRecognized event. Based on Windows.Speech.PhraseRecognizer</para>
        /// </summary>
        /// <param name="args">Information about a phrase recognized event.</param>
        public delegate void PhraseRecognizedDelegate(HidePhraseRecognitionArgs args);
        #endif
    }
}
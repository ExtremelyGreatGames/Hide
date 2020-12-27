using System;
using System.Collections.Generic;
using System.Linq;
using Hide.Speech;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Hide.System
{
    /// <summary>
    /// todo: document
    /// </summary>
    /// Note:
    /// [20/12/26] Based on Harrold's PlayerControls
    [RequireComponent(typeof(SpeechRecognizer))]
    public class ExternalInput : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public SoundPairs[] soundPairsArray = new []
        {
            new SoundPairs { sound = "moo", animal = "Cow" },
            new SoundPairs { sound = "oink", animal = "Pig" },
            new SoundPairs { sound = "cluck", animal = "Chicken" },
        };

        private Inputs controls;
        private Vector2 _move = Vector2.zero;
        private Animator _animator;
        private SpeechRecognizer _speechRecognizer;
        private KeywordHandler _keywordHandler;

        private Dictionary<string, string> _keywordDict = new Dictionary<string, string>();

        private void Awake()
        {
            /***********************************************************/
            /* todo: Need to find a way to switch between hider/seeker */
            /***********************************************************/
            controls = new Inputs();

            controls.Hider.Movement.performed += ctx =>
            {
                _move = ctx.ReadValue<Vector2>();
            };
            controls.Hider.Movement.canceled += ctx => _move = Vector2.zero;

            controls.Hider.Speak.performed += ctx => OnSpeakPressed();
            controls.Hider.Speak.canceled += ctx => OnSpeakReleased();

            controls.Hider.Ability.performed += ctx => OnAbilityPressed();
            
            
            _speechRecognizer = GetComponent<SpeechRecognizer>();
            _speechRecognizer.onPreparedEvent.AddListener(OnSpeechRecognizerPrepared);
            _speechRecognizer.onSpeechRecognizedEvent.AddListener(SpeechRecognized);
        }

        void Update()
        {
            transform.Translate(_move * (Time.deltaTime * moveSpeed));
        }

        void OnEnable()
        {
            controls?.Hider.Enable();
        }

        void OnDisable()
        {
            controls?.Hider.Disable();
        }

        void OnSpeakPressed()
        {
#if UNITY_STANDALONE_WIN
            _speechRecognizer.StartListening();
#endif
        }

        void OnSpeakReleased()
        {
#if UNITY_STANDALONE_WIN
            _speechRecognizer.StopListening();
#endif
        }

        public void OnSpeechRecognizerPrepared()
        {
            foreach (var pair in soundPairsArray)
            {
                _keywordDict[pair.sound] = pair.animal;
            }
            
            _animator = GetComponent<Animator>();
            _speechRecognizer = GetComponent<SpeechRecognizer>();
            _speechRecognizer.SetKeyword(_keywordDict.Keys.ToArray());
            // keywordRecognizer.OnPhraseRecognized += speechRecognized; is assigned through editor
            // no buttons in Android too lazy too configure
            _speechRecognizer.StartListening();
#if UNITY_ANDROID
        _speechRecognizer.StartListening();
#endif // UNITY_ANDROID
        }

        void OnAbilityPressed()
        {
            Debug.Log("Ability should activate. based on current animal");
        }

        void SpeechRecognized(HidePhraseRecognitionArgs args)
        {
            _animator.SetTrigger("Change" + _keywordDict[args.text]);
            Debug.Log("speech was recognized from: " + gameObject.name);
        }
    }

    [Serializable]
    public class SoundPairs
    {
        public string sound;
        public string animal;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Hide.Speech;
using UnityEngine;

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
        public SoundPairs[] soundPairsArray =
        {
            new SoundPairs {sound = "moo", animal = "Cow"},
            new SoundPairs {sound = "oink", animal = "Pig"},
            new SoundPairs {sound = "cluck", animal = "Chicken"},
        };

        private Inputs controls;
        private SpeechRecognizer _speechRecognizer;

        private KeywordHandler _keywordHandler;

        /* We need hasPossession because doing null checks are slow */
        private bool hasPossesion = false;
        private HidePawn _hidePawn;

        private Dictionary<string, string> _keywordDict = new Dictionary<string, string>();

        private void Awake()
        {
            /***********************************************************/
            /* todo: Need to find a way to switch between hider/seeker */
            /***********************************************************/
            controls = new Inputs();

            controls.Hider.Movement.performed += ctx =>
            {
                if (hasPossesion)
                {
                    _hidePawn.move = ctx.ReadValue<Vector2>();
                }
            };
            controls.Hider.Movement.canceled += ctx =>
            {
                if (hasPossesion)
                {
                    _hidePawn.move = Vector2.zero;
                }
            };

            controls.Hider.Speak.performed += ctx => OnSpeakPressed();
            controls.Hider.Speak.canceled += ctx => OnSpeakReleased();

            controls.Hider.Ability.performed += ctx => OnAbilityPressed();

#if !UNITY_STANDALONE_LINUX
            _speechRecognizer = GetComponent<SpeechRecognizer>();
            _speechRecognizer.onPreparedEvent.AddListener(OnSpeechRecognizerPrepared);
            _speechRecognizer.onSpeechRecognizedEvent.AddListener(SpeechRecognized);
#endif // UNITY_STANDALONE_LINUX
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
#if !UNITY_STANDALONE_LINUX
            foreach (var pair in soundPairsArray)
            {
                _keywordDict[pair.sound] = pair.animal;
            }
            
            _speechRecognizer = GetComponent<SpeechRecognizer>();
            _speechRecognizer.SetKeyword(_keywordDict.Keys.ToArray());
            // keywordRecognizer.OnPhraseRecognized += speechRecognized; is assigned through editor
            // no buttons in Android too lazy too configure
            _speechRecognizer.StartListening(); // todo: remove after debug
#if UNITY_ANDROID
        _speechRecognizer.StartListening();
#endif // UNITY_ANDROID
#endif // UNITY_STANDALONE_LINUX
        }

        void OnAbilityPressed()
        {
            Debug.Log("Ability should activate. based on current animal");
        }

        void SpeechRecognized(HidePhraseRecognitionArgs args)
        {
            if (hasPossesion)
            {
                _hidePawn.SpeechRecognized(args, _keywordDict);
            }
        }

        public void Possess(HidePawn hidePawn)
        {
            if (_hidePawn == null && !hidePawn.IsPossessed)
            {
                hasPossesion = true;
                _hidePawn = hidePawn;
                hidePawn.IsPossessed = true;
            }
            else
            {
                Debug.LogWarning("Invalid possession");
            }
        }

        public void Depossess()
        {
            if (_hidePawn != null)
            {
                hasPossesion = false;
                _hidePawn.IsPossessed = false;
                _hidePawn = null;
            }
        }
    }

    [Serializable]
    public class SoundPairs
    {
        public string sound;
        public string animal;
    }
}
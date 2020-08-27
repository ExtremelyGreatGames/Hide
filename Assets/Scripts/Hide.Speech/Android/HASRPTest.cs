using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hide.Speech.Android
{
#if UNITY_ANDROID
    [RequireComponent(typeof(HideAndroidSpeechRecognitionPlugin))]
#endif // UNITY_ANDROID
    public class HASRPTest : MonoBehaviour
    {
#if UNITY_ANDROID
        private HideAndroidSpeechRecognitionPlugin _plugin;
        
        private readonly Dictionary<string, string> _keywordDict = new Dictionary<string, string>
        {
            {"moo", "Cow"}, {"oink", "Pig"}, {"cluck", "Chicken"}, {"testing", "Testing"}
        };

        #region Events

        public void OnPrepared()
        {
            _plugin = GetComponent<HideAndroidSpeechRecognitionPlugin>();
            if (_plugin.IsUsable())
            {
                _plugin.SetKeyword(_keywordDict.Keys.ToArray());
                _plugin.OnPhraseRecognized += OnSpeechRecognized;
                _plugin.StartListening();
            }
            else
            {
                Debug.LogWarning("HASRP not usable");
            }
        }

        private void OnSpeechRecognized(HidePhraseRecognitionArgs args)
        {
            Debug.Log("Unity Received: " + args.text);
        }

        #endregion
#endif // UNITY_ANDROID
    }
}
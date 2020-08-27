using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Android
{
    [RequireComponent(typeof(HideAndroidSpeechRecognitionPlugin))]
    public class HasrpTest : MonoBehaviour
    {
        private HideAndroidSpeechRecognitionPlugin _plugin;
        
        private Dictionary<string, string> keywordDict = new Dictionary<string, string>()
        {
            {"moo", "Cow"}, {"oink", "Pig"}, {"cluck", "Chicken"}, {"testing", "Testing"}
        };

        #region Events

        public void OnPrepared()
        {
            _plugin = GetComponent<HideAndroidSpeechRecognitionPlugin>();
            _plugin.SetKeyword(keywordDict.Keys.ToArray());
        }

        #endregion
        
        /*private static AndroidJavaClass _pluginClass;
        private static AndroidJavaObject _pluginInstance;

        public static AndroidJavaClass PluginClass
        {
            get
            {
                if (_pluginClass == null)
                {
                    _pluginClass = new AndroidJavaClass(PluginName);
                }

                return _pluginClass;
            }
        }

        public static AndroidJavaObject PluginInstance
        {
            get
            {
                if (_pluginInstance == null)
                {
                    _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
                }

                return _pluginInstance;
            }
        }*/

        /*private float _elapsedTime = 0;

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= 5)
            {
                _elapsedTime -= 5;
                Debug.Log("Tick: " + getElapsedTime());
            }
        }

        double getElapsedTime()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return PluginInstance.Call<double>("getElapsedTime");
            }
            else
            {
                Debug.LogWarning("Wrong platform");
            }

            return 0;
        }*/
    }
}
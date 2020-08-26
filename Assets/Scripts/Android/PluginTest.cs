using System;
using UnityEngine;

namespace Android
{
    public class PluginTest : MonoBehaviour
    {
        private const string PluginName = "com.condimentalgames.hide.androidmodule.HideMainActivity";

        private static AndroidJavaClass _pluginClass;
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
        }

        private void Start()
        {
            Debug.Log("Elapsed time: " + getElapsedTime());
        }

        private float _elapsedTime = 0;

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
        }
    }
}
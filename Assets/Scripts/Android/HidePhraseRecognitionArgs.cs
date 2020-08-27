using UnityEngine;

namespace Android
{
    /// <summary>
    /// Based on Windows.Speech.PhraseRecognizedEventArgs
    /// </summary>
    public class HidePhraseRecognitionArgs
    {
        // todo: Decide whether to include missing attributes in UnityEngine.Windows.Speech.PhraseRecognizedEventArgs
        public float confidenceLevel = 0;
        public string text = "";

        public void FromJsonOverwrite(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
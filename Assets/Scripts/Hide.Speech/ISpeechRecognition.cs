using System;
using Hide.Speech.Android;

namespace Hide.Speech
{
    public interface ISpeechRecognition
    {
        event HidePhraseRecognitionArgs.RecognizedDelegate OnPhraseRecognized;
        event HidePhraseRecognitionArgs.OnPreparedDelegate OnPrepared;
        bool IsUsable();
        void SetKeyword(string[] keywordList);
        void StartListening(); // does not really apply to Windows
        void StopListening();
    }
}
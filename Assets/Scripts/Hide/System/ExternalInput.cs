using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Hide.System
{
    public class ExternalInput : MonoBehaviour
    {
        public float moveSpeed = 3f;

        private Input _controls = new Input();
        private Vector2 _move = Vector2.zero;
        private Animator _animator;
        private KeywordRecognizer _keywordRecognizer;
        private Dictionary<string, string> keywordDict = new Dictionary<string, string>();
        private KeywordHandler _keywordHandler;
        
        
    }
}
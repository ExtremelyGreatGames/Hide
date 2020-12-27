using System;
using System.Collections.Generic;
using Hide.Speech;
using UnityEngine;

namespace Hide.System
{
    [RequireComponent(typeof(Animator))]
    public class PawnController : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public Vector2 move = Vector2.zero;
        private Animator _animator;
        public bool IsPossessed { get; set; }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            transform.Translate(move * (Time.deltaTime * moveSpeed));
        }

        public void SpeechRecognized(HidePhraseRecognitionArgs args, Dictionary<string, string> keywordDict)
        {
            _animator.SetTrigger("Change" + keywordDict[args.text]);
            Debug.Log("speech was recognized from: " + gameObject.name);   
        }
    }
}
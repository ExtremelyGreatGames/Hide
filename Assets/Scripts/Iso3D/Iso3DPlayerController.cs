using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Iso3D
{
    public class Iso3DPlayerController : MonoBehaviour
    {
        public string levelManagerName = "LevelManager";

        private IPlayer _player;
        private bool _isReady = false;

        private void Start()
        {
            var levelManager = GameObject.Find(levelManagerName)
                .GetComponent<Iso3DLevelManager>();
            _player = levelManager.PossessPlayer();
            name = $"{_player.GetGameObject().name} - Controller";
            _isReady = _player != null;
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (_isReady)
            {
                _player.Move(context);
            }
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (_isReady)
            {
                _player.Interact(context);
            }
        }

        public void Speak(InputAction.CallbackContext context)
        {
            if (_isReady)
            {
                _player.Speak(context);
            }
        }
    }
}

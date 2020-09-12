using UnityEngine;
using UnityEngine.InputSystem;

namespace Iso3D
{
    public interface IPlayer
    {
        void Move(InputAction.CallbackContext context);
        void Interact(InputAction.CallbackContext context);
        void Speak(InputAction.CallbackContext context);
        GameObject GetGameObject();
    }
}
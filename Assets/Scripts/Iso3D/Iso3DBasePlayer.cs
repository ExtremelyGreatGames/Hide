using UnityEngine;
using UnityEngine.InputSystem;

namespace Iso3D
{
    public abstract class Iso3DBasePlayer : MonoBehaviour, IPlayer
    {
        public Iso3DMovement iso3DMovement;
        
        public void Move(InputAction.CallbackContext context)
        {
            iso3DMovement.Move(context);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public abstract void Interact(InputAction.CallbackContext context);

        public abstract void Speak(InputAction.CallbackContext context);
    }
}
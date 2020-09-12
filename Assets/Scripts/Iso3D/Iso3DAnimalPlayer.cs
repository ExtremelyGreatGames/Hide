using UnityEngine;
using UnityEngine.InputSystem;

namespace Iso3D
{
    public class Iso3DAnimalPlayer : Iso3DBasePlayer
    {
        public override void Interact(InputAction.CallbackContext context)
        {
            // do nothing??
        }

        public override void Speak(InputAction.CallbackContext context)
        {
            // todo: call transformation.cs and do speak
            throw new System.NotImplementedException();
        }
    }
}
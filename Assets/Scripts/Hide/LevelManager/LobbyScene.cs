using Hide.System;
using UnityEngine;

namespace Hide.LevelManager
{
    public class LobbyScene : MonoBehaviour
    {
        public ExternalInput externalInput;
        // todo: remove harcoded pawncontroller find
        public HidePawn hidePawn;
        
        private void Start()
        {
            // todo: find right pawn to possess
            externalInput.Possess(hidePawn);
        }

        private void OnDestroy()
        {
            externalInput.Depossess();
        }
    }
}
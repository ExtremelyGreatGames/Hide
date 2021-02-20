using Hide.ScriptableObjects;
using Mirror.Examples.NetworkRoom;
using UnityEngine;
using UnityEngine.UI;

namespace Hide.Network
{
    public class HideRoomPlayer : NetworkRoomPlayerExt
    {
        public LobbyData lobbyData;
        
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            
            lobbyData.startButton.onClick.AddListener(() => CmdChangeReadyState(!readyToBegin));
        }
    }
}
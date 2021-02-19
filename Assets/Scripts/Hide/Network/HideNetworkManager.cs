using System.Collections.Generic;
using Mirror;
using Mirror.Examples.NetworkRoom;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hide.Network
{
    /// <summary>
    /// todo: document summary here for network manager
    /// </summary>
    /// <remarks>
    /// <para>todo: document little stuff heere for  network manager</para>
    /// </remarks>
    public class HideNetworkManager : NetworkRoomManagerExt
    {
        [Header("Hide properties")] 
        
        public GameObject hiderPrefab;
        public GameObject wolfPrefab;
        
        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn, GameObject roomPlayer)
        {
            // todo: this is where we decide if you're a wolf or not
            return base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
        }

        public override void OnGUI()
        {
            // don't do anything!!
        }
    }
}

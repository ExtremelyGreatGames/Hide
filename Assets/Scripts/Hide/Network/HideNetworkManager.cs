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
        public bool debugOnLocal = true;
        public GameObject hiderPrefab;
        public GameObject wolfPrefab;

        public override void Awake()
        {
            base.Awake();

            if (debugOnLocal)
            {
                networkAddress = "localhost";
            }
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            Destroy(roomPlayer);
            return true;
        }
    }
}

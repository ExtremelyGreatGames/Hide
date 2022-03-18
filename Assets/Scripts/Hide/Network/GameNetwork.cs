#if false
using System;
using System.Text;
using MLAPI;
using MLAPI.SceneManagement;
using MLAPI.Transports.Tasks;
using MLAPI.Transports.UNET;
using RoboRyanTron.Unite2017.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Hide.Network
{
    /// <summary>
    /// Handles all the players connected, from lobby to game scene
    /// </summary>
    /// Note: 
    [RequireComponent(typeof(NetworkingManager))]
    public class GameNetwork : MonoBehaviour
    {
        public GameEvent onClientConnectedEvent;
        public GameEvent onClientDisconnectedEvent;
        
        private UserType _userType;
        private static GameNetwork _gameNetwork = null;
        private NetworkingManager _networkingManager;
        private SocketTasks _socketTasks;
        private UnetTransport _unetTransport;

        /// <summary>
        /// Gets a singleton instance of game network.
        /// </summary>
        /// <remarks>
        /// (1) Avoid calling during Awake. Call during Start is safe.
        /// (2) Do not call frequently. Keep a local reference once, preferably done during Start.
        /// </remarks>
        public static GameNetwork Instance
        {
            get
            {
                if (_gameNetwork == null)
                {
                    _gameNetwork = NetworkingManager.Singleton.GetComponent<GameNetwork>();
                }
                return _gameNetwork;
            }
        }
        
        

        private enum UserType
        {
            Host,
            Client,
            Uninitialized
        }

        private void Start()
        {
            // Since this requires Networking Manager, when it detects another Networking Manager
            // out there, it destroys the game object. We will reach this function body, if we
            // are the first object out there. 
            if (_gameNetwork == null)
            {
                _gameNetwork = this;
            }

            _networkingManager = NetworkingManager.Singleton;
            _unetTransport = _networkingManager.GetComponent<UnetTransport>();
            _userType = UserType.Uninitialized;

            _networkingManager.OnClientConnectedCallback += OnClientConnected;
            _networkingManager.OnClientDisconnectCallback += OnClientDisconnect;
            _networkingManager.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes("room password");
        }

        private void OnClientDisconnect(ulong clientId)
        {
            // todo: OnClientDisconnect
            Debug.LogWarning($"Client disconnected {clientId}");
        }

        private void OnClientConnected(ulong clientId)
        {
            // todo: OnClientConnected
            Debug.LogWarning($"Client connected {clientId}");
            onClientConnectedEvent.Raise();
        }

        private void ApprovalCheck(
            byte[] connectionData,
            ulong clientId,
            NetworkingManager.ConnectionApprovedDelegate callback)
        {
            // todo: logic here
            bool approved = false;
            bool createPlayerObject = true;
            callback(false, clientId, approved, null, null);

            // The prefab hash. Use null to use the default player prefab
            // If using this hash, replace "MyPrefabGenerator" with the name of a prefab
            // added to the NetworkPrefabs field of your NetworkingManager object in the scene. */
            // ulong? prefabHash = SpawnManager.GetPrefabHashFromGenerator("MyPrefabHashGenerator");
            
            // If approve is true, the connection gets added.
            // If it's false, the client gets disconnected.
            // callback(createPlayerObject, null, approve, positionToSpawnAt, rotationToSpawnWith);
            Debug.Log("Approval check here");
            
            // Check code based on Space Shooter example by derekdominoes for rooms
            // https://github.com/Unity-Technologies/com.unity.multiplayer.mlapi/issues/220#issuecomment-501905747
            // Debug.Log( $"Appoving client {clientId}" );
            // bool approved = false;
            // try {
            //     string receivedClientCode = System.Text.Encoding.ASCII.GetString( connectionData );
            //     if (receivedClientCode == clientCode)
            //         approved = true;
            // }
            // catch (System.Exception) {
            //     Debug.Log( "Invalid security code" );
            // }
            // connApprovalDel( clientId, null, approved, null, null );
        }

        public void PlayAsHost()
        {
            _userType = UserType.Host;
            // _networkingManager.OnServerStarted += OnServerStarted
            _networkingManager.ConnectionApprovalCallback += ApprovalCheck;
            _socketTasks = _networkingManager.StartHost();
            NetworkSceneManager.SwitchScene("LobbyScene");
            Debug.Log("Play host!");
            
            // forcing client connected here because it won't trigger in host
            // todo: change ui
        }

        public void PlayAsClient(string ipAddress)
        {
            var encodedIpAddress = ProperlyEncode(ipAddress);
            _unetTransport.ConnectAddress = encodedIpAddress;
            _userType = UserType.Client;
            _socketTasks = _networkingManager.StartClient();
            Debug.Log($"Play client: {encodedIpAddress}");
            // todo: change ui
        }

        /// <summary>
        /// Encode any string to something appropriate for IP Address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>ip address with the right encoding</returns>
        private string ProperlyEncode(string ipAddress)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var character in ipAddress)
            {
                if (character.Equals('.'))
                {
                    sb.Append('.');
                }
                else
                {
                    var num = char.GetNumericValue(character);
                    if (0 <= num && num <= 9)
                    {
                        sb.Append($"{num}");
                    }
                }
            }
            
            return sb.ToString();
        }

        public void StartGame()
        {
        }
    }
}
#endif
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Frame;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Text = UnityEngine.UI.Text;

namespace Hide.Network
{
    public class HideLobbyManager : MonoBehaviour, IHideLobbyMaster
    {
        public GameObject PlayerItem;
        public HideLobbyPlayerItem Myself;
        public InputField ChatInputBox;
        public Text Chatbox;
        public Transform Grid;

        private const int BUFFER_PLAYER_ITEMS = 10;
        private List<HideLobbyPlayerItem> _lobbyPlayersInactive = new List<HideLobbyPlayerItem>();
        private List<HideLobbyPlayerItem> _lobbyPlayersPool = new List<HideLobbyPlayerItem>();
        private HideLobbyPlayer _myself;
        
        private NetworkObject _networkObjectReference;

        #region Interface Members

        private List<IClientHidePlayer> _lobbyPlayers = new List<IClientHidePlayer>();

        public List<IClientHidePlayer> LobbyPlayers
        {
            get { return _lobbyPlayers; }
        }

        private Dictionary<uint, IClientHidePlayer> _lobbyPlayersMap = new Dictionary<uint, IClientHidePlayer>();

        public Dictionary<uint, IClientHidePlayer> LobbyPlayersMap
        {
            get { return _lobbyPlayersMap; }
        }

        private Dictionary<int, List<IClientHidePlayer>> _lobbyTeams = new Dictionary<int, List<IClientHidePlayer>>();

        public Dictionary<int, List<IClientHidePlayer>> LobbyTeams
        {
            get { return _lobbyTeams; }
        }

        #endregion

        public void Awake()
        {
            if (NetworkManager.Instance.IsServer)
            {
                SetupComplete();
                return;
            }


            for (int i = 0; i < NetworkObject.NetworkObjects.Count; ++i)
            {
                NetworkObject n = NetworkObject.NetworkObjects[i];
                if (n is HideLobbyService.LobbyServiceNetworkObject)
                {
                    SetupService(n);
                    return;
                }
            }

            NetworkManager.Instance.Networker.objectCreateRequested += CheckForService;
            NetworkManager.Instance.Networker.factoryObjectCreated += FactoryObjectCreated;
        }

        private void FactoryObjectCreated(NetworkObject obj)
        {
            if (obj.UniqueIdentity != HideLobbyService.LobbyServiceNetworkObject.IDENTITY)
                return;

            NetworkManager.Instance.Networker.factoryObjectCreated -= FactoryObjectCreated;
            SetupService(obj);
        }

        private void CheckForService(NetWorker networker, int identity, uint id, FrameStream frame,
            Action<NetworkObject> callback)
        {
            if (identity != HideLobbyService.LobbyServiceNetworkObject.IDENTITY)
            {
                return;
            }

            NetworkObject obj = new HideLobbyService.LobbyServiceNetworkObject(networker, id, frame);
            if (callback != null)
                callback(obj);
            SetupService(obj);
        }

        private void SetupService(NetworkObject obj)
        {
            HideLobbyService.Instance.Initialize(obj);
            SetupComplete();
        }

        #region Public API

        public void ChangeName(HideLobbyPlayerItem item, string newName)
        {
            HideLobbyService.Instance.SetName(newName);
        }

        public void KickPlayer(HideLobbyPlayerItem item)
        {
            HideLobbyPlayer playerKicked = item.AssociatedPlayer;
            HideLobbyService.Instance.KickPlayer(playerKicked.NetworkId);
        }

        public void ChangeAvatarID(HideLobbyPlayerItem item, int nextID)
        {
            HideLobbyService.Instance.SetAvatar(nextID);
        }

        public void ChangeTeam(HideLobbyPlayerItem item, int nextTeam)
        {
            HideLobbyService.Instance.SetTeamId(nextTeam);
        }

        public void SendPlayersMessage()
        {
            string chatMessage = ChatInputBox.text;
            if (string.IsNullOrEmpty(chatMessage))
                return;

            HideLobbyService.Instance.SendPlayerMessage(chatMessage);
            ChatInputBox.text = string.Empty;
        }

        public void StartGame(int sceneID)
        {
            if (NetworkManager.Instance.IsServer)
            {
                var players = HideLobbyService.Instance.MasterLobby.LobbyPlayers;
                var wolfIndex = Mathf.FloorToInt(Random.value * players.Count);
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].Role = (wolfIndex == i) ? HidePlayerRole.Wolf : HidePlayerRole.Animal;
                }
                
#if UNITY_5_6_OR_NEWER
                SceneManager.LoadScene(sceneID);
#else
            Application.LoadLevel(sceneID);

#endif
            }
        }

        #endregion

        #region Private API

        private HideLobbyPlayerItem GetNewPlayerItem()
        {
            HideLobbyPlayerItem returnValue = null;

            if (_lobbyPlayersInactive.Count > 0)
            {
                returnValue = _lobbyPlayersInactive[0];
                _lobbyPlayersInactive.Remove(returnValue);
                _lobbyPlayersPool.Add(returnValue);
            }
            else
            {
                //Generate more!
                for (int i = 0; i < BUFFER_PLAYER_ITEMS - 1; ++i)
                {
                    HideLobbyPlayerItem item = CreateNewPlayerItem();
                    item.ToggleObject(false);
                    item.SetParent(Grid);
                    _lobbyPlayersInactive.Add(item);
                }

                returnValue = CreateNewPlayerItem();
                _lobbyPlayersPool.Add(returnValue);
            }

            returnValue.ToggleObject(true);

            return returnValue;
        }

        private void PutBackToPool(HideLobbyPlayerItem item)
        {
            item.ToggleInteractables(false);
            item.ToggleObject(false);
            _lobbyPlayersPool.Remove(item);
            _lobbyPlayersInactive.Add(item);
        }

        private HideLobbyPlayerItem CreateNewPlayerItem()
        {
            var returnValue = Instantiate(PlayerItem).GetComponent<HideLobbyPlayerItem>();
            returnValue.Init(this);
            return returnValue;
        }

        private HideLobbyPlayerItem GrabLobbyPlayerItem(IClientHidePlayer player)
        {
            HideLobbyPlayerItem returnValue = null;

            for (int i = 0; i < _lobbyPlayersPool.Count; ++i)
            {
                if (_lobbyPlayersPool[i].AssociatedPlayer.NetworkId == player.NetworkId)
                {
                    returnValue = _lobbyPlayersPool[i];
                }
            }

            return returnValue;
        }

        private HideLobbyPlayer GrabPlayer(IClientHidePlayer player)
        {
            HideLobbyPlayer returnValue = player as HideLobbyPlayer;
            if (returnValue == null)
            {
                for (int i = 0; i < LobbyPlayers.Count; ++i)
                {
                    if (LobbyPlayers[i].NetworkId == player.NetworkId)
                    {
                        var tPlayer = LobbyPlayers[i] as HideLobbyPlayer;
                        if (tPlayer == null)
                        {
                            tPlayer = new HideLobbyPlayer();
                            tPlayer.Name = player.Name;
                            tPlayer.NetworkId = player.NetworkId;
                            tPlayer.AvatarID = player.AvatarID;
                            tPlayer.TeamID = player.TeamID;
                            LobbyPlayers[i] = tPlayer;
                        }

                        returnValue = tPlayer;
                        returnValue.Name = player.Name;
                        break;
                    }
                }

                if (returnValue == null)
                {
                    returnValue = new HideLobbyPlayer();
                    returnValue.Name = player.Name;
                    returnValue.NetworkId = player.NetworkId;
                    returnValue.AvatarID = player.AvatarID;
                    returnValue.TeamID = player.TeamID;
                }
            }

            return returnValue;
        }

        #endregion

        #region Interface API

        public void OnFNPlayerConnected(IClientHidePlayer player)
        {
            HideLobbyPlayer convertedPlayer = GrabPlayer(player);
            if (convertedPlayer == _myself || _myself == null)
                return; //Ignore re-adding ourselves

            bool playerCreated = false;
            for (int i = 0; i < _lobbyPlayersPool.Count; ++i)
            {
                if (_lobbyPlayersPool[i].AssociatedPlayer.NetworkId == player.NetworkId)
                    playerCreated = true;
            }

            playerCreated = convertedPlayer.Created;
            if (playerCreated)
                return;

            convertedPlayer.Created = true;

            if (!LobbyPlayers.Contains(convertedPlayer))
                _lobbyPlayers.Add(convertedPlayer);
            if (_lobbyPlayersMap.ContainsKey(convertedPlayer.NetworkId))
                _lobbyPlayersMap[convertedPlayer.NetworkId] = convertedPlayer;
            else
                _lobbyPlayersMap.Add(convertedPlayer.NetworkId, convertedPlayer);

            OnFNTeamChanged(convertedPlayer);

            MainThreadManager.Run(() =>
            {
                var item = GetNewPlayerItem();
                item.Setup(convertedPlayer, false);
                if (HideLobbyService.Instance.IsServer)
                    item.KickButton.SetActive(true);
                item.SetParent(Grid);
            });
        }

        public void OnFNPlayerDisconnected(IClientHidePlayer player)
        {
            var convertedPlayer = GrabPlayer(player);
            MainThreadManager.Run(() =>
            {
                if (LobbyPlayers.Contains(convertedPlayer))
                {
                    _lobbyPlayers.Remove(convertedPlayer);
                    _lobbyPlayersMap.Remove(convertedPlayer.NetworkId);

                    HideLobbyPlayerItem item = GrabLobbyPlayerItem(convertedPlayer);
                    if (item != null)
                        PutBackToPool(item);
                }
            });
        }

        public void OnFNPlayerNameChanged(IClientHidePlayer player)
        {
            HideLobbyPlayer convertedPlayer = GrabPlayer(player);
            convertedPlayer.Name = player.Name;
            if (_myself == convertedPlayer)
                Myself.ChangeName(convertedPlayer.Name);
            else
            {
                HideLobbyPlayerItem item = GrabLobbyPlayerItem(convertedPlayer);
                if (item != null)
                    item.ChangeName(convertedPlayer.Name);
            }
        }

        public void OnFNTeamChanged(IClientHidePlayer player)
        {
            int newID = player.TeamID;
            if (!LobbyTeams.ContainsKey(newID))
                LobbyTeams.Add(newID, new List<IClientHidePlayer>());

            //We do this to not make Foreach loops
            IEnumerator iter = LobbyTeams.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                if (iter.Current != null)
                {
                    KeyValuePair<int, List<IClientHidePlayer>> kv =
                        (KeyValuePair<int, List<IClientHidePlayer>>) iter.Current;
                    if (kv.Value.Contains(player))
                    {
                        kv.Value.Remove(player);
                        break;
                    }
                }
                else
                    break;
            }

            //We prevent the player being added twice to the same team
            if (!LobbyTeams[newID].Contains(player))
            {
                var convertedPlayer = player as HideLobbyPlayer;
                convertedPlayer.TeamID = newID;

                if (_myself == convertedPlayer)
                    Myself.ChangeTeam(newID);
                else
                {
                    HideLobbyPlayerItem item = GrabLobbyPlayerItem(convertedPlayer);
                    if (item != null)
                        item.ChangeTeam(newID);
                }

                LobbyTeams[newID].Add(player);
            }
        }

        public void OnFNAvatarIDChanged(IClientHidePlayer player)
        {
            HideLobbyPlayer convertedPlayer = GrabPlayer(player);
            convertedPlayer.AvatarID = player.AvatarID;
            if (_myself == convertedPlayer)
                Myself.ChangeAvatarID(convertedPlayer.AvatarID);
            else
            {
                HideLobbyPlayerItem item = GrabLobbyPlayerItem(convertedPlayer);
                if (item != null)
                    item.ChangeAvatarID(convertedPlayer.AvatarID);
            }
        }

        public void OnFNLobbyPlayerMessageReceived(IClientHidePlayer player, string message)
        {
            HideLobbyPlayer convertedPlayer = GrabPlayer(player);
            Chatbox.text += string.Format("{0}: {1}\n", convertedPlayer.Name, message);
        }

        public void OnFNPlayerSync(IClientHidePlayer player)
        {
            OnFNAvatarIDChanged(player);
            OnFNTeamChanged(player);
            OnFNPlayerNameChanged(player);
        }

        public void OnFNLobbyMasterKnowledgeTransfer(IHideLobbyMaster previousLobbyMaster)
        {
            LobbyPlayers.Clear();
            LobbyPlayersMap.Clear();
            LobbyTeams.Clear();
            for (int i = 0; i < previousLobbyMaster.LobbyPlayers.Count; ++i)
            {
                HideLobbyPlayer player = GrabPlayer(previousLobbyMaster.LobbyPlayers[i]);
                LobbyPlayers.Add(player);
                LobbyPlayersMap.Add(player.NetworkId, player);
            }

            IEnumerator iterTeams = previousLobbyMaster.LobbyTeams.GetEnumerator();
            iterTeams.Reset();
            while (iterTeams.MoveNext())
            {
                if (iterTeams.Current != null)
                {
                    KeyValuePair<int, List<IClientHidePlayer>> kv =
                        (KeyValuePair<int, List<IClientHidePlayer>>) iterTeams.Current;
                    List<IClientHidePlayer> players = new List<IClientHidePlayer>();
                    for (int i = 0; i < kv.Value.Count; ++i)
                    {
                        players.Add(GrabPlayer(kv.Value[i]));
                    }

                    LobbyTeams.Add(kv.Key, players);
                }
                else
                    break;
            }

            IEnumerator iterPlayersMap = previousLobbyMaster.LobbyPlayersMap.GetEnumerator();
            iterPlayersMap.Reset();
            while (iterPlayersMap.MoveNext())
            {
                if (iterPlayersMap.Current != null)
                {
                    KeyValuePair<uint, IClientHidePlayer> kv =
                        (KeyValuePair<uint, IClientHidePlayer>) iterPlayersMap.Current;

                    if (LobbyPlayersMap.ContainsKey(kv.Key))
                        LobbyPlayersMap[kv.Key] = GrabPlayer(kv.Value);
                    else
                        LobbyPlayersMap.Add(kv.Key, GrabPlayer(kv.Value));
                }
                else
                    break;
            }
        }

        private void SetupComplete()
        {
            HideLobbyService.Instance.SetLobbyMaster(this);
            HideLobbyService.Instance.Initialize(NetworkManager.Instance.Networker);

            //If I am the host, then I should show the kick button for all players here
            HideLobbyPlayerItem item = GetNewPlayerItem(); //This will just auto generate the 10 items we need to start with
            item.SetParent(Grid);
            PutBackToPool(item);

            _myself = GrabPlayer(HideLobbyService.Instance.MyMockPlayer);
            if (!LobbyPlayers.Contains(_myself))
                LobbyPlayers.Add(_myself);
            Myself.Init(this);
            Myself.Setup(_myself, true);

            List<IClientHidePlayer> currentPlayers = HideLobbyService.Instance.MasterLobby.LobbyPlayers;
            for (int i = 0; i < currentPlayers.Count; ++i)
            {
                IClientHidePlayer currentPlayer = currentPlayers[i];
                if (currentPlayer == _myself)
                    continue;
                OnFNPlayerConnected(currentPlayers[i]);
            }
        }

        #endregion
    }
}
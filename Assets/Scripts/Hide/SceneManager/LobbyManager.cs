using System;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkedVar;
using MLAPI.NetworkedVar.Collections;
using TMPro;
using UnityEngine;

namespace Hide.SceneManager
{
    public class LobbyManager : NetworkedBehaviour
    {
        public TextMeshProUGUI textDetails;
        
        private NetworkedList<ulong> _playerIds = new NetworkedList<ulong>(
            new NetworkedVarSettings()
            {
                ReadPermission = NetworkedVarPermission.Everyone,
                WritePermission =  NetworkedVarPermission.Everyone,
                SendTickrate = 5
            }
        , new List<ulong>());

        private NetworkingManager _networkManager;

        private void Start()
        {
            _networkManager = NetworkingManager.Singleton;
            var conn = _networkManager.LocalClientId;
            _playerIds.Add(conn);
            Refresh();
        }

        public void Refresh()
        {
            textDetails.text = "";
            foreach (var playerId in _playerIds)
            {
                textDetails.text += $"ID: {playerId}\n";
            }
        }
    }
}
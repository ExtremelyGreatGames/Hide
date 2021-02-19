using UnityEngine;
using UnityEngine.UI;

namespace Hide.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LobbyData", menuName = "ScriptableObject/Data/Lobby", order = 0)]
    public class LobbyData : ScriptableObject
    {
        public Button startButton;
    }
}
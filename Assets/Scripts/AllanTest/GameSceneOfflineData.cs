using UnityEngine;

namespace AllanTest
{
    [CreateAssetMenu(fileName = "GameSceneOfflineData", menuName = "ScriptableObjects/Data/GameSceneOffline", order = 0)]
    public class GameSceneOfflineData : ScriptableObject
    {
        public VirtualCameraScript virtualCamera;
    }
}
using Hide.System;
using UnityEngine;

namespace Hide.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSceneData", menuName = "ScriptableObject/Data/GameSceneData", order = 0)]
    public class GameSceneData : ScriptableObject
    {
        public ExternalInput externalInput;
    }
}
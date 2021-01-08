using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Hide.Network
{
    /// <summary>
    /// The player data synced between each clients.
    /// Data here is not tied to the hide player so never destroy this.
    /// </summary>
    public class GameLogic : GameLogicBehavior
    {
        public static GameLogic Instance { get; private set; }
        
        private void Awake()
        {
            
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        public override void IdentifyRole(RpcArgs args)
        {
            // todo: GameLogic.IdentifyRole
            throw new NotImplementedException();
        }
    }
}
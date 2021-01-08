using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.Playables;
using NotImplementedException = System.NotImplementedException;

namespace Hide.Network
{
    /// <summary>
    /// The player character with data synced between each clients
    /// </summary>
    public class HidePlayer : PlayerBehavior
    {
        
        
        protected override void NetworkStart()
        {
            base.NetworkStart();
            
            // If this networkObject is actually the **enemy** Player
            // hence not the one we will control and own
            if (networkObject.IsOwner)
            {
                // todo: create a controller
                // todo: request server who am i
                
                // Debug.Log($"{networkObject.NetworkId} = {}");
            }
        }

        public override void UpdateName(RpcArgs args)
        {
            // ignore the method name for now
            // todo: check player info then send back who they are via RPC
            networkObject.SendRpc(args.Info.SendingPlayer, RPC_UPDATE_NAME, "");
        }
    }
}
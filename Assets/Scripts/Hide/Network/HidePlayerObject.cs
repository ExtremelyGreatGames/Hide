namespace Hide.Network
{
    /*public partial class HidePlayerObject : PlayerNetworkObject
    {
        protected override bool ServerAllowRpc(byte methodId, Receivers receivers, RpcArgs args)
        {
            // The methodName is the name of the RPC that is trying to be called right now
            // The receivers is who the client is trying to send the RPC to
            // The args are the arguments that were sent as part of the RPC message and what the receivers will receive as arguments to the call
            if (methodId == PlayerBehavior.RPC_UPDATE_NAME)
            {
                // todo: change to assign something??? only  host can assign roles
                // this does mean that the host can cheat, huh?
                return NetworkingManager.Singleton.IsHost;
            }

            return true;
        }
    }*/
}
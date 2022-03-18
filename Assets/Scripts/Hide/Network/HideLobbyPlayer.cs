#if false
using BeardedManStudios.Forge.Networking;

namespace Hide.Network
{
    public class HideLobbyPlayer : IClientHidePlayer
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private uint _networkID;
        public uint NetworkId
        {
            get
            {
                return _networkID;
            }

            set
            {
                _networkID = value;
            }
        }

        private int _avatarID;
        public int AvatarID
        {
            get { return _avatarID; }
            set { _avatarID = value; }
        }

        private int _teamID;
        public int TeamID
        {
            get { return _teamID; }
            set { _teamID = value; }
        }

        public HidePlayerRole Role
        {
            get { return _role;}
            set { _role = value; }
        }

        private bool _created;
        private HidePlayerRole _role;

        public bool Created
        {
            get { return _created; }
            set { _created = value; }
        }
    }
}
#endif
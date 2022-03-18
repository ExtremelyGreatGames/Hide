#if false
using Hide.Network;
using MLAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hide.LevelManager
{
    /// <summary>
    /// NetworkChoiceManager
    /// todo: improve documentation
    /// </summary>
    /// Note:
    /// Q: (1) Why not directly hook up the on click events in the Editor?
    /// A: (1) Since GameNetwork is attached to NetworkManager, on the second time
    /// we arrive in the scene, our scene's GameNetwork gets destroyed. The reference
    /// we made to the buttons will be lost.
    public class ChooseNetworkScene : MonoBehaviour
    {
        public string defaultIpAddress = "127.0.0.1";

        public Animator animatorUi;
        public TextMeshProUGUI textDetails;
        public TextMeshProUGUI textInput;
        public Button buttonClient;
        public Button buttonHost;

        private NetworkingManager _networkingManager;
        private GameNetwork _gameNetwork;

        private void Start()
        {
            Debug.Assert(textInput != null);
            Debug.Assert(buttonClient != null);
            Debug.Assert(buttonHost != null);
            
            _networkingManager = NetworkingManager.Singleton;
            _gameNetwork = GameNetwork.Instance;
            textInput.text = defaultIpAddress;
            
            buttonClient.onClick.AddListener(HandleButtonClientClick);
            buttonHost.onClick.AddListener(HandleButtonHostClick);
        }

        private void HandleButtonHostClick()
        {
            _gameNetwork.PlayAsHost();
            // todo: handle not connecting correctly
        }

        private void HandleButtonClientClick()
        {
            _gameNetwork.PlayAsClient(textInput.text);
            // todo: handle not connecting correctly
        }
    }
}
#endif
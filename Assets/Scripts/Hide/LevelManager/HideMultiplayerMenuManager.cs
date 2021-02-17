using Hide.Network;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hide.LevelManager
{
    /// <summary>
    /// NetworkChoiceManager
    /// todo: improve documentation
    /// </summary>
    /// <remarks>
    /// <para>
    /// Q: Why not directly hook up the on click events in the Editor?
    /// A: Since GameNetwork is attached to NetworkManager, on the second time
    /// we arrive in the scene, our scene's GameNetwork gets destroyed. The reference
    /// we made to the buttons will be lost.
    /// </para>
    /// </remarks>
    public class HideMultiplayerMenuManager : MonoBehaviour
    {
        public string defaultIpAddress = "127.0.0.1";

        public Animator animatorUi;
        public TextMeshProUGUI textDetails;
        public TextMeshProUGUI textInput;
        public Button buttonClient;
        public Button buttonHost;

        private NetworkManager _networkManager;

        private void Start()
        {
            // Debug.Assert(textInput != null);
            Debug.Assert(buttonClient != null);
            Debug.Assert(buttonHost != null);
            
            _networkManager = NetworkManager.singleton;
            // textInput.text = defaultIpAddress;
            
            buttonClient.onClick.AddListener(HandleButtonClientClick);
            buttonHost.onClick.AddListener(HandleButtonHostClick);
        }

        private void HandleButtonHostClick()
        {
            _networkManager.StartHost();
            // todo: handle not connecting correctly
        }

        private void HandleButtonClientClick()
        {
            _networkManager.StartClient();
            // todo: handle not connecting correctly
        }
    }
}
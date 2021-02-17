using UnityEngine;
using UnityEngine.SceneManagement;
using NotImplementedException = System.NotImplementedException;

namespace Hide.Network
{
    /// <summary>
    /// The player data synced between each clients.
    /// Data here is not tied to the hide player so never destroy this.
    /// </summary>
    // public class GameLogic : GameLogicBehavior
    public class GameLogic
    {
        private HidePlayer _localPlayer;
        public static GameLogic Instance { get; private set; }
        
        /*private void Awake()
        {
            
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public override void IdentifyRole(RpcArgs args)
        {
            // todo: GameLogic.IdentifyRole
            throw new NotImplementedException();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name.Equals("HideGame"))
            {
                NetworkManager.Instance.InstantiatePlayer(position: new Vector3(Random.value * 5f, Random.value * 5f));
            }
            else
            {
                Debug.Log($"Game logic detect scene change to {scene.name}");
            }
        }*/
    }
}
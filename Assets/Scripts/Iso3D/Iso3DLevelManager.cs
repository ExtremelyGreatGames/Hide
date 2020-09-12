using UnityEngine;

namespace Iso3D
{
    public class Iso3DLevelManager : MonoBehaviour
    {
        public int maxPlayerCount = 2;
    
        public GameObject wolfPrefab;
        public GameObject animalPrefab;

        private int _playerCount = 0;

        private void Start()
        {
            Debug.Assert(wolfPrefab.GetComponent<Iso3DWolfPlayer>() != null, 
                "wolfPrefab.GetComponent<Iso3DWolfPlayer>() != null");
            Debug.Assert(animalPrefab.GetComponent<Iso3DAnimalPlayer>() != null,
                "animalPrefab.GetComponent<Iso3DAnimalPlayer>() != null");
        }

        public IPlayer PossessPlayer()
        {
            IPlayer player = null;
        
            if (_playerCount == 0)
            {
                player = Instantiate(wolfPrefab, Vector3.right * _playerCount, 
                        wolfPrefab.transform.rotation).GetComponent<Iso3DWolfPlayer>();
            }
            else if (_playerCount < maxPlayerCount)
            {
                player = Instantiate(animalPrefab, Vector3.right * _playerCount, 
                        animalPrefab.transform.rotation).GetComponent<Iso3DAnimalPlayer>();
            }
            else
            {
                Debug.LogWarning($"Number of players ({_playerCount}) exceeded {maxPlayerCount}");
            }

            _playerCount++;

            return player;
        }
    }
}

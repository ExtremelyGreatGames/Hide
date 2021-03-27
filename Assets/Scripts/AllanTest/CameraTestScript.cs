using UnityEngine;

namespace AllanTest
{
    public class CameraTestScript : MonoBehaviour
    {
        public GameSceneOfflineData cameraData;
        
        private bool isActivated = false;
    
        void Update()
        {
            if (!isActivated && Input.GetButtonDown("Jump") && cameraData.virtualCamera != null)
            {
                isActivated = true;
                cameraData.virtualCamera.Attach(transform);
            }
        }
    }
}

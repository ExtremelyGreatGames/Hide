using System;
using Cinemachine;
using UnityEngine;

namespace AllanTest
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraScript : MonoBehaviour
    {
        public GameSceneOfflineData data;
        private CinemachineVirtualCamera virtualCamera;

        private void OnDestroy()
        {
            data.virtualCamera = null;
            this.virtualCamera.Follow = null;
        }

        private void Start()
        {
            this.virtualCamera = GetComponent<CinemachineVirtualCamera>();
            data.virtualCamera = this;
        }

        public void Attach(Transform focus)
        {
            this.virtualCamera.Follow = focus;
        }
    }
}
using System;
using UnityEngine;

namespace Iso3D
{
    public class CameraTarget : MonoBehaviour
    {
        public Transform objectTarget;

        private bool _hasTarget = false;
        
        private void Start()
        {
            _hasTarget = objectTarget != null;
        }

        private void Update()
        {
            if (_hasTarget)
            {
                transform.position = objectTarget.position;
            }
        }
    }
}
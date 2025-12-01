using System;
using Cameras;
using Configs;
using UnityEngine;
using VContainer;

namespace Gameplay.Background
{
    public class BackgroundManager : MonoBehaviour
    {
        [Inject] private readonly BackgroundConfig _config;
        [Inject] private readonly ICameraManager _mainCamera;
        [SerializeField] private Transform forrestTransform;
        [SerializeField] private Transform skyTransform;
        private Vector3 _cameraPosition;
        private float _forrestYAxis;
        private void Start()
        {
            _forrestYAxis = forrestTransform.position.y;
        }

        private void Update()
        {
            _cameraPosition = _mainCamera.GetCameraPosition();
            _cameraPosition.z = 0f;
            skyTransform.position = _cameraPosition;
            forrestTransform.position = new Vector3(
                _cameraPosition.x * _config.forestXSpeedMultiplier,
                _forrestYAxis + _cameraPosition.y * +_config.forestYSpeedMultiplier,
                0);
        }
    }
}
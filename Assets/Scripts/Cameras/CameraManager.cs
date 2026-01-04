using Unity.Cinemachine;
using UnityEngine;
namespace Cameras
{
    public class CameraManager : ICameraManager
    {
        private Camera _mainCamera;
        private CinemachineBrain _cinemachineBrain;
        private CinemachineCamera _cinemachineCamera;
        private CinemachineConfiner2D _cinemachineConfiner;
        public void Initialize()
        {
            _mainCamera = Camera.main;
            
            _cinemachineBrain = _mainCamera?.GetComponent<CinemachineBrain>();
            _cinemachineCamera = _cinemachineBrain?.ActiveVirtualCamera as CinemachineCamera;
            _cinemachineConfiner = _cinemachineCamera?.gameObject.GetComponent<CinemachineConfiner2D>();
        }

        public void FollowPlayer(Transform playerTransform)
        {
            _cinemachineCamera.Follow = playerTransform;
            _cinemachineCamera.LookAt = playerTransform;
        }

        public void SetCinemachineConfiner(BoxCollider2D confinerCollider)
        {
            _cinemachineConfiner.BoundingShape2D = confinerCollider;
        }

        public void UnfollowPlayer()
        {
            _cinemachineCamera.Follow = null;
            _cinemachineCamera.LookAt = null;
        }

        public Camera GetMainCamera()
        {
            return _mainCamera;
        }

        public Vector3 GetCameraPosition()
        {
            return _mainCamera.transform.position;
        }

        public float GetCameraWidth()
        {
            return _mainCamera.orthographicSize * _mainCamera.aspect;
        }

        public float GetOrthographicSize()
        {
            return _mainCamera.orthographicSize;
        }

        public float GetCameraAspect()
        {
            return _mainCamera.aspect;
        }
    }
}
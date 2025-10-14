using UnityEngine;
namespace Cameras
{
    public class CameraManager : ICameraManager
    {
        private Camera _mainCamera;
        public void Initialize()
        {
            _mainCamera = Camera.main;
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
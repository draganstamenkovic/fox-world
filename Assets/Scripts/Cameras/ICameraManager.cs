using UnityEngine;

namespace Cameras
{
    public interface ICameraManager
    {
        void Initialize();
        Camera GetMainCamera();
        Vector3 GetCameraPosition();
        float GetCameraWidth();
        float GetOrthographicSize();
        float GetCameraAspect();
    }
}
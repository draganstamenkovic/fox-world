using UnityEngine;

namespace Helpers.RuntimeInfo
{
    public class RuntimeInformation : IRuntimeInformation
    {
        public RuntimeOSPlatform OSPlatform { get; set; }

        public void Initialize()
        {
#if UNITY_EDITOR
            Debug.Log("Unity Editor");
            OSPlatform = RuntimeOSPlatform.Editor;
#elif UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("Android");
            Platform = RuntimePlatform.Android;
#elif UNITY_IOS && !UNITY_EDITOR
            Debug.Log("IOS");      
            Platform = RuntimePlatform.IOS;
#endif
        }
    }
}
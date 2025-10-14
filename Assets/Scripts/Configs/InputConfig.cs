using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        public RectTransform joystickPrefab;
        public RectTransform fireButtonPrefab;
        public float joystickSensitivity;
    }
}
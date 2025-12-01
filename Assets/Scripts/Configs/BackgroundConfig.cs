using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "BackgroundConfig", menuName = "Configs/BackgroundConfig")]
    public class BackgroundConfig : ScriptableObject
    {
        [Tooltip("Multiplier for forest X axis while following camera on X axis")]
        public float forestXSpeedMultiplier = 1.05f;
        [Tooltip("Multiplier for forest Y axis when following camera on Y axis")]
        public float forestYSpeedMultiplier = 0.5f;
    }
}
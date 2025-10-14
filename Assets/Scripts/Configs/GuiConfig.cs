using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GuiConfig", menuName = "Configs/GuiConfig")]
    public class GuiConfig : ScriptableObject
    {
        public float loadingRotateSpeed = 1f;
    }
}
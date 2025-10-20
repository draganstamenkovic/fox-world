using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "BackgroundConfig", menuName = "Configs/BackgroundConfig")]
    public class BackgroundConfig : ScriptableObject
    {
        public Sprite skyImage;
        public Sprite forestImage;
    }
}
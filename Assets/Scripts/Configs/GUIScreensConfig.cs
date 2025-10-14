using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GUIScreensConfig", menuName = "Configs/GUIScreensConfig")]
    public class GUIScreensConfig : ScriptableObject
    {
        public Vector2 Forward = new Vector2(3000,0);
        public Vector2 Backward = new Vector2(-3000,0);
        
        public float TransitionDuration = 0.5f;
        public Ease TransitionEase = Ease.OutBack;
        public List<ScreenData> Screens = new();
    }

    [Serializable]
    public class ScreenData
    {
        public string Name;
        public RectTransform Screen;
    }
}

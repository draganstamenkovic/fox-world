using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PopupsConfig", menuName = "Configs/PopupsConfig")]
    public class PopupsConfig : ScriptableObject
    {
        public float ShowTransitionDuration = 0.35f;
        public float HideTransitionDuration = 0.15f;
        public Ease TransitionEase = Ease.OutBack;
        public List<PopupPrefabData> Popups = new();
    }
    
    [Serializable]
    public class PopupPrefabData
    {
        public string Name;
        public RectTransform PopupPrefab;
    }
}
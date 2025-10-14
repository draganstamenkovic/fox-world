using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private List<Skin> playerSkins;
        public Vector3 startPosition;
        public float speed = 5f;
        public float offsetPositionY = 0.5f;

        public GameObject GetActivePlayerSkin(string skinId)
        {
            return new GameObject("Player");
        }
    }

    [Serializable]
    public class Skin
    {
        public string Id;
        public GameObject Prefab;
    }
}
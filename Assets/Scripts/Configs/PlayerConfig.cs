using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Player")]
        public GameObject playerPrefab;
        public GameObject onLandParticle;
        
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float airControlFactor = 0.8f;
        
        [Header("Jump Settings")]
        public float jumpForce = 10f;
        public float jumpBufferTime = 0.1f;
        public float coyoteTime = 0.1f;
        public float jumpCutMultiplier = 0.5f;
        
        [Header("Gravity Settings")]
        public float defaultGravity = 8f;
        public float afterJumpGravity = 12f;
        
        [Header("Ground Detection")]
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;

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
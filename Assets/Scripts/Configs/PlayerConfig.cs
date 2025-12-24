using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Player")]
        public GameObject playerPrefab;
        public GameObject onLandParticle;
        public Vector3 startPosition;
        
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        
        [Header("Jump Settings")]
        public float jumpForce = 10f;

        public float jumpForceOnKill = 20f;
        public float jumpBufferTime = 0.1f;
        
        [Header("Gravity Settings")]
        public float defaultGravity = 8f;
        public float afterJumpGravity = 12f;
        
        [Header("Ground Detection")]
        public float groundCheckRadius = 0.1f;
        public LayerMask groundLayer;
        
        public float topHitThreshold = 0.5f;
    }
}
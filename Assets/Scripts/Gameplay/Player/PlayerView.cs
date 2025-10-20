using System;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerView : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rigidBody;
        public Animator animator;
        public Transform dustParticleTransform;
                
        [Header("Ground Detection")]
        public Transform groundCheck;
        
        public Action OnUpdate;

        void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
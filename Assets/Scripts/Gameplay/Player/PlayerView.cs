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
        public CapsuleCollider2D playerCollider;
        
        [Header("Ground Detection")]
        public Transform groundCheck;

        [Header("Jump platform check")] 
        public Transform topCheck;
        
        public Action OnUpdate;
        public Action<Collision2D> OnCollisionEnter;

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEnter?.Invoke(collision);
        }
    }
}
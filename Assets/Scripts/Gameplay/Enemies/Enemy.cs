using UnityEngine;
using DG.Tweening;

namespace Gameplay.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D enemyCollider;
        [SerializeField] private Transform enemyTransform;
        [SerializeField] private Transform startPointTransform;
        [SerializeField] private Transform endPointTransform;
        
        public float enemyMoveDuration = 2f;
        private bool _stopMovement;
        private void Start()
        {
            Move();
        }
        
        public void Die()
        {
            enemyTransform.DOKill();
            _stopMovement = true;
            animator.Play(AnimationIds.EnemyDied);
            enemyCollider.enabled = false;
            Destroy(gameObject, 0.5f);
        }
        
        private void Move()
        {
            if (_stopMovement) return;
            
            enemyTransform.DOMove(endPointTransform.position, enemyMoveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (_stopMovement) return;
                    
                    enemyTransform.localScale = new Vector3(-1, 1, 1);
                    enemyTransform.DOMove(startPointTransform.position, enemyMoveDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => 
                        { 
                            if (_stopMovement) return;
                            
                            enemyTransform.localScale = Vector3.one;
                            Move();
                        });
                });
        }
    }
}
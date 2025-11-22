using UnityEngine;

namespace Gameplay.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CircleCollider2D circleCollider;

        public virtual void Die()
        {
            animator.Play(AnimationIds.EnemyDied);
            circleCollider.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}
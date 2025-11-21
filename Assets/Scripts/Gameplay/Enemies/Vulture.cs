using Configs;
using UnityEngine;
using DG.Tweening;

namespace Gameplay.Enemies
{
    public class Vulture : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform vultureTransform;
        [SerializeField] private Transform startPointTransform;
        [SerializeField] private Transform endPointTransform;
        [SerializeField] private EnemiesConfig enemiesConfig;
        private void Start()
        {
            Move();
        }

        private void Move()
        {
            vultureTransform.DOMove(endPointTransform.position,enemiesConfig.vultureMoveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    vultureTransform.localScale = new Vector3(-1, 1, 1);
                    vultureTransform.DOMove(startPointTransform.position, enemiesConfig.vultureMoveDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => 
                        { 
                            vultureTransform.localScale = Vector3.one;
                            Move();
                        });
                });
        }
    }
}

using System;
using DG.Tweening;
using Message.Messages;
using UnityEngine;

namespace Gameplay.Collectables
{
    public class CollectableItem : MonoBehaviour
    {
        public CollectedItem itemType;
        public void OnCollected(Camera mainCamera, Vector3 itemWorldPosition, Action onComplete)
        {
            // Screen position of the UI rect (0..screenWidth, 0..screenHeight)
            var screenPoint = RectTransformUtility.WorldToScreenPoint(
                null, // use null for Screen Space - Overlay, safe for others too
                itemWorldPosition
            );
            
            // Convert UI screen pos to world pos on the collectable's plane
            var worldTarget = mainCamera.ScreenToWorldPoint(
                new Vector3(screenPoint.x, screenPoint.y, 0)
            );

            transform.DOMove(worldTarget, 0.25f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                    onComplete?.Invoke();
                });
                
        }
    }
}
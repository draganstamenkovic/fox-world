using System;
using Configs;
using DG.Tweening;
using Gui.Popups.Builder;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Gui.Popups.Views
{
    public abstract class PopupView : MonoBehaviour, IPopupView
    {
        [Inject] private PopupsConfig _popupsConfig;
        
        public CanvasGroup canvasGroup;
        public Button backgroundButton;
        public string ID { get; set; }
        public Action OnShow { get; set; }
        public Action OnShown { get; set; }
        public Action OnHide { get; set; }
        public Action OnHidden { get; set; }

        public virtual void Initialize()
        {
            canvasGroup.alpha = 0;
        }

        public virtual void Show(Action onComplete = null)
        {
            OnShow?.Invoke();
            gameObject.SetActive(true);
            canvasGroup.DOFade(1f, _popupsConfig.showTransitionDuration)
                .SetEase(_popupsConfig.transitionEase).OnComplete(() =>
                {
                    OnShown?.Invoke();
                    onComplete?.Invoke();
                });
        }

        public virtual void Hide(Action onComplete = null)
        {
            OnHide?.Invoke();
            canvasGroup.DOFade(0f, _popupsConfig.hideTransitionDuration)
                .SetEase(_popupsConfig.transitionEase).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnHidden?.Invoke();
                    onComplete?.Invoke();
                });
        }
    }
}
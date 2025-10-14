using System;
using Gui.Popups.Builder;

namespace Gui.Popups.Views
{
    public interface IPopupView
    {
        string ID { get; }
        Action OnShow { get; }
        Action OnShown { get; }
        Action OnHide { get; }
        Action OnHidden { get; }
        void Initialize();
        void Show(Action onComplete = null);
        void Hide(Action onComplete = null);
    }
}
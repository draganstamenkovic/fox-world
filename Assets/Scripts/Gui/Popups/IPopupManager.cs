using System;
using Gui.Popups.Builder;
using UnityEngine;

namespace Gui.Popups
{
    public interface IPopupManager
    {
        void Initialize(Transform parent, GameObject screenBlocker);
        void ShowConfirmationPopup(PopupData popupDataData, Action callback = null);
        void ShowPopupScreen(string id, Action callback = null);
        void HidePopup(string id, Action callback = null);
        
    }
}
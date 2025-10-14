using System;
using UnityEngine;

namespace Gui.Popups.Builder
{
    public interface IPopupBuilder
    {
        IPopupBuilder Title(string title);
        IPopupBuilder Text(string text);
        IPopupBuilder AddButton(string buttonText, Color color, Action onButtonClick);
        void Clear();
        PopupData Build();
    }
}
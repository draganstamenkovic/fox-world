using System.Collections.Generic;
using Gui.Popups.Builder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Popups.Views
{
    public class ConfirmationPopupView : PopupView
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private Image _icon;
        [SerializeField] List<PopupButtonData> _buttons;
        public override void Initialize()
        {
            base.Initialize();
            ID = PopupIds.ConfirmationPopup;
        }

        private void SetupButtons(List<PopupButton> buttonsData)
        {
            foreach (var buttonData in _buttons)
            {
                buttonData.Button.onClick.RemoveAllListeners();
                buttonData.Button.gameObject.SetActive(false);
            }
            
            for (int index = 0; index < buttonsData.Count; index++)
            {
                if (index < buttonsData.Count)
                {
                    _buttons[index].Text.text = buttonsData[index].Text;
                    _buttons[index].Color = buttonsData[index].Color;
                    var i = index;
                    _buttons[index].Button.onClick.AddListener(() =>
                    {
                        buttonsData[i].OnClick();
                    });
                    
                    _buttons[index].Button.gameObject.SetActive(true);
                }
                else
                {
                    _buttons[index].Button.gameObject.SetActive(false);
                }
            }
        }

        public void SetData(PopupData popupData)
        {
            _title.text = popupData.Title;
            _message.text = popupData.Text;
            if (popupData.Icon != null)
                _icon.sprite = popupData.Icon;
            SetupButtons(popupData.Buttons);
        }
    }
}
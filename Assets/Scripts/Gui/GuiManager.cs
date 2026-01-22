using Gui.Popups;
using Gui.Screens;
using Message;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Gui
{
    public class GuiManager : MonoBehaviour
    {
        [Inject] private readonly IScreenManager _screenManager;
        [Inject] private readonly IPopupManager _popupManager;
        [Inject] private readonly IMessageBroker _messageBroker;

        [SerializeField] private Transform screensContainer;
        [SerializeField] private Transform popupsContainer;
        
        [SerializeField] private GameObject screenBlocker;
        [SerializeField] private GameObject fullScreen;
        [SerializeField] private Button fullScreenButton;
        public void Initialize()
        {
            _screenManager.Initialize(screensContainer, screenBlocker);
            _popupManager.Initialize(popupsContainer, screenBlocker);

#if UNITY_WEBGL && !UNITY_EDITOR
            fullScreen.gameObject.SetActive(true);
            fullScreenButton.onClick.AddListener(OnFullScreenButtonClick);
#endif
        }
        
        private void OnFullScreenButtonClick()
        {
            Screen.fullScreen = !Screen.fullScreen;
            fullScreen.gameObject.SetActive(false);
        }
    }
}

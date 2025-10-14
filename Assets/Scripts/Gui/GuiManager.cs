using Configs;
using Gui.Popups;
using Gui.Popups.Builder;
using Gui.Screens;
using UnityEngine;
using VContainer;

namespace Gui
{
    public class GuiManager : MonoBehaviour
    {
        [Inject] private readonly IScreenManager _screenManager;
        [Inject] private readonly IPopupManager _popupManager;
        [Inject] private readonly IPopupBuilder _popupBuilder;
        [Inject] private GuiConfig _guiConfig;

        [SerializeField] private Transform _screensContainer;
        [SerializeField] private Transform _popupsContainer;
        
        [SerializeField] private Transform _spinner;
        [SerializeField] private GameObject _screenBlocker;
        private bool _activeSpinner;
        public void Initialize()
        {
            SetSpinnerActive(true);
            _screenManager.Initialize(_screensContainer, _screenBlocker);
            _popupManager.Initialize(_popupsContainer, _screenBlocker);

            _screenManager.ShowScreen(GuiScreenIds.MainMenuScreen);
            SetSpinnerActive(false);
        }

        public void SetSpinnerActive(bool active)
        {
            _activeSpinner = active;
            _spinner.gameObject.SetActive(active);
        }

        private void Update()
        {
            if(_activeSpinner)
                _spinner.transform.Rotate(new Vector3(0, 0, _guiConfig.loadingRotateSpeed));
        }
    }
}

using Gui.Screens.Views;
using Message;
using Message.Messages;
using UnityEngine;
using VContainer;

namespace Gui.Screens.Controllers
{
    public class LevelSelectScreenController : IScreenController
    {
        [Inject] private readonly IMessageBroker _messageBroker;
        private LevelSelectScreenView _view;
        private IScreenManager _screenManager;
        public string ID => GuiScreenIds.LevelSelectScreen;

        public void SetView(IScreenView view)
        {
            _view = view as LevelSelectScreenView;
        }

        public void Initialize(IScreenManager screenManager)
        { 
            _screenManager = screenManager;
            _view.OnShow = RegisterListeners;
            _view.OnHidden = RemoveListeners;
        }

        private void RegisterListeners()
        {
            foreach (var levelButton in _view.levelButtons)
            {
                levelButton.LevelButtonClicked = OnLevelButtonClicked;
            }
            _view.backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            _screenManager.HideScreen(ID);
            _screenManager.ShowScreen(GuiScreenIds.MainMenuScreen);
        }

        private void OnLevelButtonClicked(int levelIndex)
        {
            _screenManager.HideScreen(ID);
            _screenManager.ShowScreen(GuiScreenIds.PlayScreen);
            _messageBroker.Publish(new SelectedLevelMessage(levelIndex));
            _messageBroker.Publish(new PlayGameMessage());
        }

        private void RemoveListeners()
        {
            foreach (var levelButton in _view.levelButtons)
            {
                levelButton.LevelButtonClicked = null;
            }
            _view.backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }
}

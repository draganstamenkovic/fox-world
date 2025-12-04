using System;
using System.Collections.Generic;
using Configs;
using Gui.Popups.Builder;
using Gui.Popups.Controllers;
using Gui.Popups.Views;
using Message;
using Message.Messages;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gui.Popups
{
    public class PopupManager : IPopupManager
    {
        [Inject] private readonly PopupsConfig _popupConfig;
        [Inject] private IEnumerable<IPopupController> _controllers;
        [Inject] private readonly IMessageBroker _messageBroker;
        
        private IObjectResolver _objectResolver;
        private readonly Dictionary<string, RectTransform> _popups = new();
        private readonly Dictionary<string, RectTransform> _spawnedPopups = new();
        private readonly Dictionary<string, IPopupView> _spawnedPopupViews = new();
        
        private Transform _popupParent;
        private GameObject _screenBlocker;

        public PopupManager(IObjectResolver resolver)
        {
            _objectResolver = resolver;
        }

        public void Initialize(Transform parent, GameObject screenBlocker)
        {
            _popupParent = parent;
            _screenBlocker = screenBlocker;
            _messageBroker.Receive<ShowPopupMessage>().Subscribe(message =>
            {
                ShowPopupScreen(message.Id);
            });
            
            foreach (var popup in _popupConfig.popups)
            {
                if (!_popups.ContainsKey(popup.name))
                {
                    _popups.Add(popup.name, popup.popupPrefab);
                }
            }
        }
        public void ShowConfirmationPopup(PopupData popupData, Action callback = null)
        {
            _screenBlocker.SetActive(true);
            var popup = GetPopupRectTransform(PopupIds.ConfirmationPopup);
            
            var popupView = popup.GetComponent<ConfirmationPopupView>();
            popupView.SetData(popupData);
            
            popupView.Show(() =>
            {
                _screenBlocker.SetActive(false);
                callback?.Invoke();
            });
        }

        public void HidePopup(string id, Action callback = null)
        {
            if (_spawnedPopupViews.TryGetValue(id, out var popupView))
            {
                _screenBlocker.SetActive(true);
                popupView.Hide(() =>
                {
                    _screenBlocker.SetActive(false);
                    callback?.Invoke();
                });
            }
            else
            {
                Debug.LogError($"Popup with id {id} not found");
                _screenBlocker.SetActive(false);
            }
        }

        public void ShowPopupScreen(string id, Action callback = null)
        {
            _screenBlocker.SetActive(true);
            var popup = GetPopupRectTransform(id);
            var popupView = popup.GetComponent<IPopupView>();
            popupView.Show(() =>
            {
                _screenBlocker.SetActive(false);
                callback?.Invoke();
            });
        }

        private RectTransform GetPopupRectTransform(string popupId)
        {
            if (_spawnedPopups.TryGetValue(popupId, out var spawnedPopup))
            {
                return spawnedPopup;
            }

            if (!_popups.TryGetValue(popupId, out var popup))
            {
                Debug.LogError($"Popup Id {popupId} is not added in config");
            }

            var popupRect = _objectResolver.Instantiate(popup, _popupParent);
            _spawnedPopups.Add(popupId, popupRect);
            _spawnedPopupViews.Add(popupId, popupRect.GetComponent<IPopupView>());
            
            InitializePopup(popupRect);
            
            return popupRect;
        }

        private void InitializePopup(RectTransform popupRect)
        {
            var view = popupRect.GetComponent<IPopupView>();
            if (view == null)
            {
                Debug.LogError("Popup view is not added to popup GameObject");
            }
            else
            {
                view.Initialize();
                foreach (var controller in _controllers)
                {
                    if (controller.ID.Equals(view.ID))
                    {
                        controller.SetView(view);
                        controller.Initialize(this);
                        break;
                    }
                }
            }
        }
    }
}
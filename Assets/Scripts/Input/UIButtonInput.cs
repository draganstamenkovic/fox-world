using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Input
{
    public class UIButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action OnPressed;
        public Action OnReleased;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnReleased?.Invoke();
        }
    }
}

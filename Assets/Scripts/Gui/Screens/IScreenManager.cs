using UnityEngine;

namespace Gui.Screens
{
    public interface IScreenManager
    {
        void Initialize(Transform parent, GameObject screenBlocker);
        void ShowScreen(string screenName);
        void HideScreen(string screenName);
    }
}
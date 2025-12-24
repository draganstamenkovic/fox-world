using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gui.Screens.Views
{
    public class PlayScreenView : ScreenView
    {
        public Button pauseButton;
        public TextMeshProUGUI gemsCountText;
        public TextMeshProUGUI cherriesCountText;
        public RectTransform gemsRectTransform;
        public RectTransform cherriesRectTransform;
        public override void Initialize()
        {
            ID = GuiScreenIds.PlayScreen;
        }

        public void UpdateScore(int gems, int cherries)
        {
            gemsCountText.text = gems.ToString();
            cherriesCountText.text = cherries.ToString();
        }
        
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Gui.Popups.Builder
{
    public class PopupData
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public Sprite Icon { get; set; }
        public List<PopupButton> Buttons { get; set; }
    }
}
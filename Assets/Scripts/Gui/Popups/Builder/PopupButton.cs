using System;
using UnityEngine;

namespace Gui.Popups.Builder
{
    public class PopupButton
    {
        public string Text { get; set; }
        public Color Color { get; set; }
        public Action OnClick { get; set; }
        
        
    }
}
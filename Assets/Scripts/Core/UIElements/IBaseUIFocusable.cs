//Made by Galactspace Studios

using System;
using Scriptable.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Localization;

namespace Core.UIElements
{
    public interface IBaseUIFocusable
    {
        public string Up { get; set; }
        public string Down { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }

        public void RegisterFocus(GameInputSo gameInput);
        public void UnregisterFocus(GameInputSo gameInput);

        public void OnUp();
        public void OnDown();
        public void OnLeft();
        public void OnRight();
    }
}

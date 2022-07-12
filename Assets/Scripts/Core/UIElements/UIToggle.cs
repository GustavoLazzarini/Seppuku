//Made by Galactspace Studios

using System;
using UnityEngine;
using UnityEngine.UIElements;
using Scriptable.InputSystem;

namespace Core.UIElements
{
    public class UIToggle : Toggle, IBaseUIElement<bool>, IBaseUIFocusable
    {
        public new class UxmlFactory : UxmlFactory<UIToggle, UxmlTraits> { }

        public new class UxmlTraits : Toggle.UxmlTraits
        {
            private UxmlStringAttributeDescription m_Up = new UxmlStringAttributeDescription
            {
                name = "up"
            };

            private UxmlStringAttributeDescription m_Down = new UxmlStringAttributeDescription
            {
                name = "down"
            };

            private UxmlStringAttributeDescription m_Left = new UxmlStringAttributeDescription
            {
                name = "left"
            };

            private UxmlStringAttributeDescription m_Right = new UxmlStringAttributeDescription
            {
                name = "right"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                UIToggle button = (UIToggle)ve;
                button._up = m_Up.GetValueFromBag(bag, cc);
                button._down = m_Down.GetValueFromBag(bag, cc);
                button._left = m_Left.GetValueFromBag(bag, cc);
                button._right = m_Right.GetValueFromBag(bag, cc);
                base.Init(ve, bag, cc);
            }
        }

        private string _up;
        private string _down;
        private string _left;
        private string _right;

        public bool ElementValue { get => value; set => this.value = value; }
        public string ElementText { get => label; set => label = value; }

        public string Up { get => _up; set => _up = value; }
        public string Down { get => _down; set => _down = value; }
        public string Left { get => _left; set => _left = value; }
        public string Right { get => _right; set => _right = value; }

        public void RegisterFocus(GameInputSo gameInput)
        {
            Focus();
            gameInput.LinkNavs(OnUp, OnDown, OnLeft, OnRight);
        }

        public void UnregisterFocus(GameInputSo gameInput)
        {
            gameInput.UnlinkNavs(OnUp, OnDown, OnLeft, OnRight);
        }

        public void OnUp() { }
        public void OnDown() { }
        public void OnLeft() { }
        public void OnRight() { }
    }
}

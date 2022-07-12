//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.UIElements;
using Scriptable.InputSystem;

namespace Core.UIElements
{
    public class UISliderInt : SliderInt, IBaseUIElement<int>, IBaseUIFocusable
    {
        public new class UxmlFactory : UxmlFactory<UISliderInt, TargetUxmlTraits> { }

        public class TargetUxmlTraits : UxmlTraits
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

            private UxmlIntAttributeDescription m_Subdivisions = new UxmlIntAttributeDescription
            {
                name = "subdivisions",
                defaultValue = 10
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                UISliderInt slider = (UISliderInt)ve;
                slider._subdivisions = m_Subdivisions.GetValueFromBag(bag, cc);
                slider._up = m_Up.GetValueFromBag(bag, cc);
                slider._down = m_Down.GetValueFromBag(bag, cc);
                slider._left = m_Left.GetValueFromBag(bag, cc);
                slider._right = m_Right.GetValueFromBag(bag, cc);
                base.Init(ve, bag, cc);
            }
        }

        private VisualElement _fillElement;

        private int _subdivisions;

        private string _up;
        private string _down;
        private string _left;
        private string _right;

        public int Subdivisions { get => _subdivisions; set => _subdivisions = value; }

        public int ElementValue { get => this.value; set => this.value = Mathf.Clamp(value, lowValue, highValue); }
        public string ElementText { get => label; set => label = value; }
        
        public string Up { get => _up; set => _up = value; }
        public string Down { get => _down; set => _down = value; }
        public string Left { get => _left; set => _left = value; }
        public string Right { get => _right; set => _right = value; }

        private int SubdividedValue => (int)(((float)highValue - (float)lowValue) / (float)Subdivisions);

        public UISliderInt() : base()
        {
            VisualElement dragContainer = this.Q<VisualElement>("unity-drag-container");

            styleSheets.Add(Resources.Load<StyleSheet>("USS_CustomControls/UISlider"));

            _fillElement = new VisualElement() { name = "unity-fill" };
            _fillElement.AddToClassList("unity-base-slider__fill");

            dragContainer.Insert(1, _fillElement);
        }

        public void RegisterFocus(GameInputSo gameInput)
        {
            Focus();
            gameInput.LinkNavs(OnUp, OnDown, OnLeft, OnRight);
        }

        public void UnregisterFocus(GameInputSo gameInput)
        {
            gameInput.UnlinkNavs(OnUp, OnDown, OnLeft, OnRight);
        }

        public void OnUp()
        {
            if (direction == SliderDirection.Horizontal) return;
            ElementValue += SubdividedValue;
        }
        
        public void OnDown()
        {
            if (direction == SliderDirection.Horizontal) return;
            ElementValue -= SubdividedValue;
        }
        
        public void OnLeft()
        {
            if (direction == SliderDirection.Vertical) return;
            ElementValue -= SubdividedValue;
        }
        
        public void OnRight()
        {
            if (direction == SliderDirection.Vertical) return;
            ElementValue += SubdividedValue;
        }

        private void UpdateSliderFill(int value)
        {
            int tvalue = Mathf.Clamp(value, lowValue, highValue);
            _fillElement.transform.scale = new Vector3(((float)tvalue - (float)lowValue) / ((float)highValue - (float)lowValue), 1);
        }

        public override void SetValueWithoutNotify(int newValue)
        {
            base.SetValueWithoutNotify(newValue);
            UpdateSliderFill(newValue);
        }
    }
}

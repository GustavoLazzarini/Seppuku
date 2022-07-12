//Made by Galactspace Studios

using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UIElements
{
    public class UIStaticSlider : BaseField<float>, IBaseUIElement<float>
    {
        public new class UxmlFactory : UxmlFactory<UIStaticSlider, UxmlTraits> { }

        public new class UxmlTraits : BaseFieldTraits<float, UxmlFloatAttributeDescription>
        {
            private UxmlFloatAttributeDescription m_lowValue = new UxmlFloatAttributeDescription
            {
                name = "low-value"
            };

            private UxmlFloatAttributeDescription m_highValue = new UxmlFloatAttributeDescription
            {
                name = "high-value",
                defaultValue = 10f
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                UIStaticSlider slider = (UIStaticSlider) ve;
                slider._lowValue = m_lowValue.GetValueFromBag(bag, cc);
                slider._highValue = m_highValue.GetValueFromBag(bag, cc);
                base.Init(ve, bag, cc);
            }
        }

        private VisualElement _holder;
        private VisualElement _fill;
        private VisualElement _tracker;

        private float _lowValue;
        private float _highValue;

        public float LowValue
        {
            get => _lowValue;
            set => _lowValue = value;
        }
        public float HighValue
        {
            get => _highValue;
            set => _highValue = value;
        }

        public float ElementValue { get => value; set => this.value = value; }
        public string ElementText { get => label; set => label = value; }

        public UIStaticSlider() : this(null, null)
        {}

        public UIStaticSlider(string label, VisualElement visualInput) : base(label, visualInput)
        {
            _holder = new VisualElement() { name = "unity-drag-container" };
            _tracker = new VisualElement() { name = "slider-tracker" };
            _fill = new VisualElement() { name = "slider-fill" };

            this.AddToClassList("unity-base-slider");
            _holder.AddToClassList("unity-base-slider__drag-container");
            _tracker.AddToClassList("unity-base-slider__tracker");
            _fill.AddToClassList("unity-base-slider__fill");

            styleSheets.Add(Resources.Load<StyleSheet>("USS_CustomControls/UISlider"));

            Children().ToArray()[0].Add(_holder);
            _holder.Add(_tracker);
            _holder.Add(_fill);
        }

        private void UpdateSliderFill(float value)
        {
            float tvalue = Mathf.Clamp(value, _lowValue, _highValue);
            _fill.transform.scale = new Vector3(((float)tvalue - (float)_lowValue) / ((float)_highValue - (float)_lowValue), 1);
        }

        public override void SetValueWithoutNotify(float newValue)
        {
            base.SetValueWithoutNotify(newValue);
            UpdateSliderFill(newValue);
        }
    }
}

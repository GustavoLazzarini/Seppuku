//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.Transitions
{
    [RequireComponent(typeof(UIDocument))]
    public class SlideTransition : Transition
    {
        private VisualElement _element;

        [SerializeField] private UIDocument doc;
        [SerializeField] private float duration;

        [Space]
        [SerializeField] private bool inverted;
        [SerializeField] private float angle;
        [SerializeField] private EasingMode easingMode;

        private VisualElement GetFadeElement(Vector2 defaultValue)
        {
            if (_element != null) return _element;

            _element = doc.rootVisualElement.Q<VisualElement>("Background");
            _element.transform.position = defaultValue;

            _element.style.transitionProperty = new List<StylePropertyName>
            {
                new StylePropertyName("translate")
            };
            _element.style.transitionDuration = new List<TimeValue>
            {
                new TimeValue(duration, TimeUnit.Second)
            };
            _element.style.transitionTimingFunction = new List<EasingFunction>
            {
                new EasingFunction(easingMode)
            };

            return _element;
        }

        private async void SetTranslate(Vector2 value, Vector2 defaultValue = default)
        {
            VisualElement fadeElement = GetFadeElement(defaultValue);
            await Taskf.WaitSeconds(0.1f);
            fadeElement.transform.position = value;
        }

        protected override async Task TransitionIn()
        {
            Vector2 value = new Vector2((float)Mathf.Cos(angle * Mathf.Deg2Rad), (float)Mathf.Sin(angle * Mathf.Deg2Rad)) * (5000 * (inverted ? -1 : 1));
            SetTranslate(Vector2.zero, -value);

            await Taskf.WaitSeconds(duration + 0.1f);
        }

        protected override async Task TransitionOut()
        {
            Vector2 value = new Vector2((float)Mathf.Cos(angle * Mathf.Deg2Rad), (float)Mathf.Sin(angle * Mathf.Deg2Rad)) * (5000 * (inverted ? -1 : 1));
            SetTranslate(value, Vector2.zero);
            await Taskf.WaitSeconds(duration + 0.1f);
        }
    }
}

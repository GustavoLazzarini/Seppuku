//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.Transitions
{
    [RequireComponent(typeof(UIDocument))]
    public class FadeTransition : Transition
    {
        private VisualElement _element;

        [SerializeField] private UIDocument doc;
        [SerializeField] private float duration = 1;

        private VisualElement GetFadeElement(int defaultValue)
        {
            if (_element != null) return _element;

            _element = doc.rootVisualElement.Q<VisualElement>("Background");
            _element.style.opacity = defaultValue;

            _element.style.transitionProperty = new List<StylePropertyName>
            {
                new StylePropertyName("opacity")
            };
            _element.style.transitionDuration = new List<TimeValue>
            {
                new TimeValue(duration, TimeUnit.Second)
            };

            return _element;
        }

        private async void SetAlpha(float value, int defaultValue = 0)
        {
            VisualElement fadeElement = GetFadeElement(defaultValue);
            await Taskf.WaitSeconds(0.1f);
            fadeElement.style.opacity = value;
        }

        protected override async Task TransitionIn()
        {
            SetAlpha(1, 0);
            
            await Taskf.WaitSeconds(duration + 0.1f);
        }

        protected override async Task TransitionOut()
        {
            SetAlpha(0, 1);
            await Taskf.WaitSeconds(duration + 0.1f);
        }
    }
}

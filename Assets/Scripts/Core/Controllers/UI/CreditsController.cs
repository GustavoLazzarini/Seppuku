//Made by Galactspace Studios

using UnityEngine;
using Core.Callers;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.Controllers.UI
{
    [RequireComponent(typeof(SceneCaller))]
    public class CreditsController : UIController
    {
        private SceneCaller _caller;

        private int _stageIndex;
        private VisualElement[] _stages;

        private VisualElement CurrentStage => _stages[_stageIndex];

        [Space]
        [SerializeField] private float stageDuration = 3;
        [SerializeField] private EasingMode easeMode = EasingMode.Linear;

        private void Start()
        {
            _caller = GetComponent<SceneCaller>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _stages = GetVisualElements();
            
            CreditsAnimation();
        }

        protected override void Update()
        {
            base.Update();
        }

        private async void CreditsAnimation()
        {
            SetDefaults();
            await Taskf.WaitSeconds(1);
            for (int i = 0; i < _stages.Length; i++)
            {
                CurrentStage.style.opacity = 1;
                await Taskf.WaitSeconds(stageDuration + 1);
                CurrentStage.style.opacity = 0;
                await Taskf.WaitSeconds(1);
                _stageIndex++;
            }

            _caller.Call();
        }

        private void SetDefaults()
        {
            for (int i = 0; i < _stages.Length; i++)
            {
                VisualElement stage = _stages[i];
                stage.style.opacity = 0;
                stage.style.transitionProperty = new List<StylePropertyName>
                {
                    new StylePropertyName("opacity")
                };
                stage.style.transitionDuration = new List<TimeValue>
                {
                    new TimeValue(1, TimeUnit.Second)
                };
                stage.style.transitionTimingFunction = new List<EasingFunction>
                {
                    new EasingFunction(easeMode)
                };
            }
        }
    }
}

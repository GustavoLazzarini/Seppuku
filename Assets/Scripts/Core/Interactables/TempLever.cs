//Created by Galactspace

using UnityEngine;
using UnityEngine.Events;

namespace Core.Interactables
{
    public class TempLever : Lever
    {
        [HideInInspector] public float OnRate;

        [Space]
        [SerializeField] protected UnityEvent<float> _onUpdate;

        [Space]
        [SerializeField] private float _decayRate;

        protected override void Interact()
        {
            base.Interact();

            SetOnRate(1);

            Routinef.LoopWhile(() =>
            {
                SetOnRate(OnRate - (_decayRate * Time.fixedDeltaTime));

            }, () => OnRate > 0.1f, Time.fixedDeltaTime, this, () =>
            {
                SetOnRate(0);
                _isOn = false;
                _onSwitch?.Invoke(false);
            });
        }

        private void SetOnRate(float value)
        {
            OnRate = value;
            _onUpdate?.Invoke(OnRate);
        }
    }
}
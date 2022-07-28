//Copyright Galactspace 2022

using UnityEngine;
using UnityEngine.Events;

namespace Core.Interactables
{
    public class Lever : Interactable
    {
        private Animator _animator;

        protected bool _isOn;
        public bool IsOn => _isOn;

        [Space]
        [SerializeField] protected UnityEvent<bool> _onSwitch;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        protected override void Interact()
        {
            base.Interact();
            _animator.SetTrigger("Do");
            _isOn = !_isOn;
            _onSwitch?.Invoke(_isOn);
        }
    }
}
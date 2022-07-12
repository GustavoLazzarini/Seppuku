//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.Events;

namespace Core.Interactables
{
    public class InteractableAction : Interactable
    {
        [Space]
        [SerializeField] private UnityEvent interactEvent;

        protected override void Interact()
        {
            base.Interact();
            interactEvent?.Invoke();
        }
    }
}

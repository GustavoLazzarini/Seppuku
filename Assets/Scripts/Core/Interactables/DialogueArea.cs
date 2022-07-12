//Made by Galactspace Studios

using UnityEngine;
using Core.Callers;

namespace Core.Interactables
{
    [RequireComponent(typeof(DialogueCaller))]
    public class DialogueArea : Interactable
    {
        private DialogueCaller caller;

        private void Awake()
        {
            caller = GetComponent<DialogueCaller>();
        }

        protected override void Interact()
        {
            base.Interact();
            caller.Call();
        }
    }
}

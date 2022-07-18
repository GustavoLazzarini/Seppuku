//Made by Galactspace Studios

using System.Linq;
using UnityEngine;
using Core.Entities;
using Core.Controllers;
using UnityEngine.Localization;

namespace Core.Interactables
{
    [RequireComponent(typeof(CubeCollider))]
    public abstract class Interactable : MonoBehaviour
    {
        protected bool _popupOnScreen;
        protected bool _hasInteracted;

        protected CubeCollider[] _colliders;

        protected Protagonist _protagonist;

        [SerializeField] protected bool _multiple;
        [SerializeField] protected bool _drawGizmos = true;

        [Space]
        [SerializeField] protected LocalizedString _popupText;

        protected bool CanInteract => !(!InsideAnyCollider() || (!_multiple && _hasInteracted));

        protected virtual void OnEnable() => InputMan.GameInputs.InteractChannel.Link(OnInteract);
        protected virtual void OnDisable()
        {
            InputMan.GameInputs.InteractChannel.Unlink(OnInteract);
            HidePopup();
        }

        protected void ShowPopup()
        {
            if (_popupOnScreen || string.IsNullOrWhiteSpace(_popupText.TableReference)) return;

            PopupMan.ShowInfoPopup(_popupText);
            _popupOnScreen = true;
        }

        protected void HidePopup()
        {
            if (!_popupOnScreen) return;

            PopupMan.Hide();
            _popupOnScreen = false;
        }

        protected virtual bool InsideAnyCollider()
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i].InsideCollider(_protagonist.transform.position)) return true;
            }

            return false;
        }

        protected virtual void Awake()
        {
            _colliders = GetComponents<CubeCollider>();
            _protagonist = FindObjectOfType<Protagonist>();
        }

        protected virtual void Update() 
        {
            if (!_multiple && _hasInteracted) return;

            if (InsideAnyCollider()) ShowPopup();
            else HidePopup();
        }

        protected virtual void OnInteract()
        {
            if (!CanInteract) return;
            Interact();
        }

        protected virtual void Interact()
        {
            HidePopup();
            _hasInteracted = true;
        }
    }
}
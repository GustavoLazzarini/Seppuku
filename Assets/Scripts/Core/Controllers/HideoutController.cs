//Copyright Galactspace Studio

using UnityEngine;
using Core.Entities;
using Core.Interactables;
using Scriptable.Generic;

namespace Core.Controllers
{
    public class HideoutController : Interactable
    {
        private Protagonist _player;

        private Vector3 _enterPos;
        private bool _insideHideout;

        [Space]
        [SerializeField] private ChannelSo _enterChannel;
        [SerializeField] private ChannelSo _exitChannel;

        [Space]
        [SerializeField] private Vector3 _hideoutPosition;
        [SerializeField] private float _hideAngle;

        protected override void OnEnable()
        {
            _enterChannel.Link(Hide);
            _exitChannel.Link(Unhide);
        }

        protected override void OnDisable()
        {
            _enterChannel.Unlink(Hide);
            _exitChannel.Unlink(Unhide);
            HidePopup();
        }

        private void Start()
        {
            _player = FindObjectOfType<Protagonist>();
        }

        private void Hide()
        {
            if (!InsideAnyCollider() || (!_multiple && _hasInteracted) || _insideHideout) return;

            _insideHideout = true;
            _enterPos = _player.transform.position;
            _player.SetMoveStage(MoveStage.None);
            _player.Lerp(transform.position + _hideoutPosition, new Vector3(0, _hideAngle, 0));
        }

        private void Unhide()
        {
            if (!InsideAnyCollider() || (!_multiple && _hasInteracted) || !_insideHideout) return;

            _insideHideout = false;
            _player.SetMoveStage(MoveStage.Walk);
            _player.Lerp(new Vector3(transform.position.x, _enterPos.y, _enterPos.z), new Vector3(0, 90, 0));
        }

        private void OnDrawGizmosSelected() {
            Gizmosf.DrawSphere(transform.position + _hideoutPosition, 0.05f, new Color(0, 0, 1, 1));
        }
    }
}
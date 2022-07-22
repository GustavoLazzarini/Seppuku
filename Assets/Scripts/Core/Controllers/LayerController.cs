//Copyright Galactspace 2022

using System.Linq;
using UnityEngine;
using Core.Entities;
using Core.Attributes;
using Core.Interactables;
using System.Collections.Generic;
using System;
using Scriptable.Generic;

namespace Core.Controllers
{
    public class LayerController : Interactable
    {
        private List<Bubble> _bubbles;

        private Bubble PlayerNearestBubble => _bubbles.OrderBy(x => (x.transform.position - _protagonist.transform.position).magnitude).ToList()[0];
        private Bubble GetBubble(Func<Bubble, bool> predicate) => _bubbles.First(predicate);

        [Space]
        [SerializeField] private ChannelSo _upChannel;
        [SerializeField] private ChannelSo _downChannel;
        [SerializeField] private ChannelSo _leftChannel;
        [SerializeField] private ChannelSo _rightChannel;

        [Space]
        [SerializeField, Button] private bool _createBubbles;

        private void OnValidate()
        {
            if (_createBubbles)
            {
                _createBubbles = false;

                if (transform.childCount > 0) return;

                var b1 = new GameObject("Bubble_001", typeof(Bubble)).GetComponent<Bubble>();
                var b2 = new GameObject("Bubble_002", typeof(Bubble)).GetComponent<Bubble>();

                b1.transform.SetParent(transform);
                b2.transform.SetParent(transform);

                b1.Position = transform.position;
                b2.Position = transform.position + new Vector3(0, 0, 1);

                b1.Up = b2;
                b2.Down = b1;
            }
        }

        protected override void OnEnable()
        {
            _upChannel.Link(OnUp);
            _downChannel.Link(OnDown);
            _leftChannel.Link(OnLeft);
            _rightChannel.Link(OnRight);
        }

        protected override void OnDisable()
        {
            _upChannel.Unlink(OnUp);
            _downChannel.Unlink(OnDown);
            _leftChannel.Unlink(OnLeft);
            _rightChannel.Unlink(OnRight);
            HidePopup();
        }

        private void Start()
        {
            _bubbles = GetComponentsInChildren<Bubble>().ToList();
            _protagonist = FindObjectOfType<Protagonist>();
        }

        private void OnUp()
        {
            if (!CanInteract || PlayerNearestBubble.Up == null) return;
            Interact(PlayerNearestBubble.Up, new Vector3(0, 0, 0));
        }

        private void OnDown()
        {
            if (!CanInteract || PlayerNearestBubble.Down == null) return;
            Interact(PlayerNearestBubble.Up, new Vector3(0, 180, 0));
        }

        private void OnLeft()
        {
            if (!CanInteract || PlayerNearestBubble.Left == null) return;
            Interact(PlayerNearestBubble.Up, new Vector3(0, 270, 0));
        }

        private void OnRight()
        {
            if (!CanInteract || PlayerNearestBubble.Right == null) return;
            Interact(PlayerNearestBubble.Up, new Vector3(0, 90, 0));
        }

        protected void Interact(Bubble b, Vector3 euler)
        {
            base.Interact();

            _protagonist.SetMoveStage(MoveStage.None);

            _protagonist.EAnimator.SetBool("IsWalking", true);
            _protagonist.CurrentMoveSize = 2;

            _protagonist.LinearLerp(b.Position, euler, _protagonist.RunSpeed, 1, true, true, true, () =>
            {
                _protagonist.CurrentMoveSize = 0;
                _protagonist.SetMoveStage(MoveStage.Walk);
            });
        }
    }
}
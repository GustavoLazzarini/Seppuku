//Copyright Galactspace Studio

using System;
using System.Linq;
using UnityEngine;
using Core.Entities;
using Core.Interactables;
using System.Collections.Generic;
using Scriptable.Generic;

namespace Core.Controllers
{
    public class StairController : Interactable
    {
        public enum Direction { Up, Down, Left, Right }

        private List<Bubble> _bakedBubbles = new List<Bubble>();

        private CapsuleCollider _protagonistCollider;

        [Space]
        [SerializeField] private ChannelSo _upChannel;
        [SerializeField] private ChannelSo _downChannel;

        [Space]
        [SerializeField] private Vector3 _upLayer;
        [SerializeField] private Vector3 _downLayer;

        [SerializeField] private float _insideLayer;

        [Space]
        [SerializeField] private bool _autoEnter;

        private Bubble[] _bubbles;

        private bool _enteredUp;
        private Vector3 _enterPos;

        public bool IsActive => _protagonist.CurrentStair == this;

        private bool _canJump;

        private Bubble _activeBubble;
        public Bubble ActiveBubble
        {
            get => _activeBubble;
            private set => _activeBubble = value;
        }

        private Bubble _topBubble;
        private Bubble TopBubble => _topBubble ??= GetBubble(x => x.BubbleType == Bubble.Data.Top);

        private Bubble _bottomBubble;
        private Bubble BottomBubble => _bottomBubble ??= GetBubble(x => x.BubbleType == Bubble.Data.Bottom);

        private Bubble[] GetTops => GetBubbles(x => x.BubbleType == Bubble.Data.Top).ToArray();
        private Bubble[] GetInsides => GetBubbles(x => x.BubbleType == Bubble.Data.Inside).ToArray();
        private Bubble[] GetBottoms => GetBubbles(x => x.BubbleType == Bubble.Data.Bottom).ToArray();

        [ContextMenu("Link Bottom to Top")]
        private void LinkBottomTop()
        {
            Bubble last = null;
            foreach (Bubble b in GetComponentsInChildren<Bubble>())
            {
                if (last != null)
                {
                    b.Down = last;
                    last.Up = b;
                }

                last = b;
            }
        }

        [ContextMenu("Link Top to Bottom")]
        private void LinkTopBottom()
        {
            Bubble last = null;
            foreach (Bubble b in GetComponentsInChildren<Bubble>())
            {
                if (last != null)
                {
                    b.Up = last;
                    last.Down = b;
                }

                last = b;
            }
        }

        [ContextMenu("Link Left to Right")]
        private void LinkLeftRight()
        {
            Bubble last = null;
            foreach (Bubble b in GetComponentsInChildren<Bubble>())
            {
                if (last != null)
                {
                    b.Left = last;
                    last.Right = b;
                }

                last = b;
            }
        }

        [ContextMenu("Link Right to Left")]
        private void LinkRightLeft()
        {
            Bubble last = null;
            foreach (Bubble b in GetComponentsInChildren<Bubble>())
            {
                if (last != null)
                {
                    b.Right = last;
                    last.Left = b;
                }

                last = b;
            }
        }

        private void Start()
        {
            _bubbles = GetComponentsInChildren<Bubble>();
            _protagonist = FindObjectOfType<Protagonist>();
        }

        protected override void OnEnable()
        {
            _upChannel.Link(OnUp);
            _downChannel.Link(OnDown);
        }

        protected override void OnDisable()
        {
            _upChannel.Unlink(OnUp);
            _downChannel.Unlink(OnDown);
            HidePopup();
        }

        protected override void Interact()
        {
            base.Interact();
            SetActive(!IsActive);
        }

        private Bubble GetBubble(Func<Bubble, bool> predicate) => _bubbles.First(predicate);
        private Bubble GetBubble(Func<Bubble, bool> predicate, out Bubble bubble)
        {
            bubble = _bubbles.First(predicate);
            return bubble;
        }
        private List<Bubble> GetBubbles(Func<Bubble, bool> predicate) => _bubbles.Where(predicate).ToList();
        private Bubble GetNearestBubble(Bubble bubble, params Bubble[] exclude)
        {
            List<Bubble> list = _bubbles.ToList().OrderBy(x => MathF.Abs((x.transform.position - bubble.transform.position).magnitude)).ToList();
            list.Remove(bubble);
            list.RemoveAll(x => x.InsideOf(exclude));
            return list[0];
        }
        private Bubble GetNearestBubbleWhere(Vector3 pos, Func<Bubble, bool> predicate)
        {
            List<Bubble> bubbles = GetBubbles(predicate);

            if (bubbles.Count == 0) return null;
            if (bubbles.Count == 1) return bubbles[0];

            return bubbles.OrderBy(x => (x.transform.position - pos).magnitude).ToList()[0];
        }
        private Bubble GetConnectedWhere(Bubble target, Func<Bubble, bool> predicate)
        {
            int index = Array.IndexOf(_bubbles, target);
            if (index > 0 && predicate(_bubbles[index - 1])) return _bubbles[index - 1];
            if (index < _bubbles.Length - 1 && predicate(_bubbles[index + 1])) return _bubbles[index + 1];
            return _bubbles[index];
        }

        private Bubble GetStairByDirection(Direction dir) => dir switch
        {
            Direction.Up => ActiveBubble.Up,
            Direction.Down => ActiveBubble.Down,
            Direction.Left => ActiveBubble.Left,
            Direction.Right => ActiveBubble.Right,
            _ => throw new NotImplementedException()
        };

        private void OnUp()
        {
            if (!InsideAnyCollider() || (!_multiple && _hasInteracted) || IsActive) return;

            List<Bubble> bbs = GetBubbles(x => x.BubbleType != Bubble.Data.Inside);
            Bubble nearstBubble = bbs.OrderBy(x => (x.transform.position - _protagonist.transform.transform.position).magnitude).ToList()[0];
            _enteredUp = nearstBubble.BubbleType == Bubble.Data.Top;

            if (!_enteredUp) SetActive(true);
        }

        private void OnDown()
        {
            if (!InsideAnyCollider() || (!_multiple && _hasInteracted) || IsActive) return;

            List<Bubble> bbs = GetBubbles(x => x.BubbleType != Bubble.Data.Inside);
            Bubble nearstBubble = bbs.OrderBy(x => (x.transform.position - _protagonist.transform.transform.position).magnitude).ToList()[0];
            _enteredUp = nearstBubble.BubbleType == Bubble.Data.Top;

            if (_enteredUp) SetActive(true);
        }

        public void JumpTo(Direction direction)
        {
            if (!_canJump) return;

            Bubble bubble = GetStairByDirection(direction);
            if (bubble == null) return;

            _activeBubble = bubble;

            if (_activeBubble.BubbleType == Bubble.Data.Bottom)
            {
                _enterPos = _activeBubble.transform.position;
                SetActive(false);
                return;
            }
            else if (_activeBubble.BubbleType == Bubble.Data.Top)
            {
                _enterPos = _activeBubble.transform.position;
                _protagonist.Set_Anim_FinishClimbing();
                return;
            }

            _protagonist.SetEuler(new Vector3(0, bubble.transform.eulerAngles.y, 0));
            _protagonist.JumpToStairFromArm(bubble.transform.position, null, (x) => _canJump = x);
        }

        public void SetActive(bool value)
        {
            if (!value)
            {
                _protagonist.LerpTo(new Vector3(_enterPos.x, _enterPos.y, _enterPos.z), new Vector3(0, 90, 0), -1, 0.01f, () => _protagonist.SetMoveStage(MoveStage.Walking));
                _protagonist.CurrentStair = null;
                return;
            }

            List<Bubble> bbs = GetBubbles(x => x.BubbleType != Bubble.Data.Inside);
            Bubble nearstBubble = bbs.OrderBy(x => (x.transform.position - _protagonist.transform.transform.position).magnitude).ToList()[0];

            _enteredUp = nearstBubble.BubbleType == Bubble.Data.Top;

            //_protagonist.CurrentStair = this;
            _enterPos = _protagonist.transform.transform.position;
            _protagonist.SetMoveStage(MoveStage.MonkeyClimbing);

            ActiveBubble = nearstBubble.GetAnyConnected();
            
            _protagonist.JumpToStairStart(_activeBubble.transform.position, new Vector3(0, _activeBubble.transform.eulerAngles.y, 0));
        
            _canJump = true;
        }

        protected override void Update()
        {
            base.Update();

            if (!IsActive)
            {
                if (_autoEnter && InsideAnyCollider() && Player.CurrentStair == null) SetActive(true);
                return;
            }

            _protagonistCollider ??= _protagonist.GetComponent<CapsuleCollider>();
        }
    }
}
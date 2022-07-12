//Made by Galactspace Studios

using System;
using UnityEngine;
using Scriptable.Generic;
using Scriptable.Configuration;

namespace Scriptable.InputSystem
{
    [CreateAssetMenu(menuName = "Game/Input/Game Input")]
    public class GameInputSo : ScriptableObject
    {
        [Space]
        [SerializeField] private ButtonTextLinkSo _buttonLink;

        [Space]
        [SerializeField] private ControllerIconsSo _inputDeviceIcons;
        [SerializeField] private InputDeviceTypeChannelSo _inputDeviceChannel;

        [Space]
        [SerializeField] private Vector2ChannelSo _mDeltaChannel;
        [SerializeField] private Vector2ChannelSo _mPositionChannel;

        [Space]
        [SerializeField] private Vector2ChannelSo _lStickChannel;
        [SerializeField] private Vector2ChannelSo _rStickChannel;

        [Space]
        [SerializeField] private BoolChannelSo _runChannel;
        [SerializeField] private ChannelSo _jumpChannel;
        [SerializeField] private ChannelSo _crouchChannel;
        [SerializeField] private BoolChannelSo _dashChannel;
        [SerializeField] private Vector2ChannelSo _movementChannel;

        [Space]
        [SerializeField] private ChannelSo _backChannel;
        [SerializeField] private ChannelSo _menuChannel;
        [SerializeField] private ChannelSo _actionMenuChannel;

        [Space]
        [SerializeField] private ChannelSo _interactChannel;
        [SerializeField] private ChannelSo _passDialogChannel;
        [SerializeField] private ChannelSo _btnInteractChannel;
        
        [Space]
        [SerializeField] private ChannelSo _aimChannel;
        [SerializeField] private ChannelSo _mAttackChannel;
        [SerializeField] private ChannelSo _rAttackChannel;

        [Space]
        [SerializeField] private ChannelSo _upChannel;
        [SerializeField] private ChannelSo _downChannel;
        [SerializeField] private ChannelSo _leftChannel;
        [SerializeField] private ChannelSo _rightChannel;

        [Space]
        [SerializeField] private BoolChannelSo _constantUpChannel;
        [SerializeField] private BoolChannelSo _constantDownChannel;
        [SerializeField] private BoolChannelSo _constantLeftChannel;
        [SerializeField] private BoolChannelSo _constantRightChannel;

        public ButtonTextLinkSo ButtonLink => _buttonLink;

        public ControllerIconsSo InputDeviceIcons => _inputDeviceIcons;
        public InputDeviceTypeChannelSo InputDeviceChannel => _inputDeviceChannel;

        public Vector2ChannelSo MouseDeltaChannel => _mDeltaChannel;
        public Vector2ChannelSo MousePositionChannel => _mPositionChannel;
        
        public Vector2ChannelSo LeftStickChannel => _lStickChannel;
        public Vector2ChannelSo RightStickChannel => _rStickChannel;

        public BoolChannelSo RunChannel => _runChannel;
        public ChannelSo JumpChannel => _jumpChannel;
        public ChannelSo CrouchChannel => _crouchChannel;
        public BoolChannelSo DashChannel => _dashChannel;
        public Vector2ChannelSo MovementChannel => _movementChannel;
    
        public ChannelSo BackChannel => _backChannel;
        public ChannelSo MenuChannel => _menuChannel;
        public ChannelSo ActionMenuChannel => _actionMenuChannel;

        public ChannelSo InteractChannel => _interactChannel;
        public ChannelSo PassDialogChannel => _passDialogChannel;
        public ChannelSo ButtonInteractChannel => _btnInteractChannel;

        public ChannelSo AimChannel => _aimChannel;
        public ChannelSo MeleeAttackChannel => _mAttackChannel;
        public ChannelSo RangeAttackChannel => _rAttackChannel;

        public ChannelSo UpChannel => _upChannel;
        public ChannelSo DownChannel => _downChannel;
        public ChannelSo LeftChannel => _leftChannel;
        public ChannelSo RightChannel => _rightChannel;

        public BoolChannelSo ConstantUpChannel => _constantUpChannel;
        public BoolChannelSo ConstantDownChannel => _constantDownChannel;
        public BoolChannelSo ConstantLeftChannel => _constantLeftChannel;
        public BoolChannelSo ConstantRightChannel => _constantRightChannel;

        public void LinkNavs(Action onUp, Action onDown, Action onLeft, Action onRight)
        {
            UpChannel.Link(onUp);
            DownChannel.Link(onDown);
            LeftChannel.Link(onLeft);
            RightChannel.Link(onRight);
        }

        public void UnlinkNavs(Action onUp, Action onDown, Action onLeft, Action onRight)
        {
            UpChannel.Unlink(onUp);
            DownChannel.Unlink(onDown);
            LeftChannel.Unlink(onLeft);
            RightChannel.Unlink(onRight);
        }

        public void LinkConstantNavs(Action<bool> onUp, Action<bool> onDown, Action<bool> onLeft, Action<bool> onRight)
        {
            ConstantUpChannel.Link(onUp);
            ConstantDownChannel.Link(onDown);
            ConstantLeftChannel.Link(onLeft);
            ConstantRightChannel.Link(onRight);
        }

        public void UnlinkConstantNavs(Action<bool> onUp, Action<bool> onDown, Action<bool> onLeft, Action<bool> onRight)
        {
            ConstantUpChannel.Unlink(onUp);
            ConstantDownChannel.Unlink(onDown);
            ConstantLeftChannel.Unlink(onLeft);
            ConstantRightChannel.Unlink(onRight);
        }
    }
}

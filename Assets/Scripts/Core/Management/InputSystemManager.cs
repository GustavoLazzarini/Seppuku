//Made by Galactspace Studios

using System;
using Core.Types;
using System.Linq;
using UnityEngine;
using Scriptable.Font;
using Scriptable.InputSystem;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.DualShock;

namespace Core.Management
{
    [RequireComponent(typeof(CursorManager))]
    public class InputSystemManager : MonoBehaviour
    {
        private CursorManager _cursorManager;
        private GameInputAction _gameInputAction;

        private FontPackSo _startFont;

        public GameInputSo GameInputs;

        public bool HasMouse => !Mouse.current.IsNull();
        public bool HasKeyboard => !Keyboard.current.IsNull();
        public bool HasGamepad => !Gamepad.current.IsNull();
        public bool HasXbox => HasGamepad && Gamepad.current is XInputController;
        public bool HasDualshock => HasGamepad && !DualShockGamepad.current.IsNull();
        public bool HasSwitchPro => HasGamepad && Gamepad.current is SwitchProControllerHID;

        private bool PressedKeyboard()
        {
            if (!HasKeyboard) return false;
            return Keyboard.current.anyKey.IsPressed();
        }

        private bool MovedMouse()
        {
            if (!HasMouse) return false;
            if (Mouse.current.delta.ReadValue().magnitude > 0.3f) return true;
            return Mouse.current.IsPressed();
        }

        private bool PressedGamepad()
        {
            if (!HasGamepad) return false;
            return Gamepad.current.allControls.Any(x => x.IsPressed());
        }

        private FontPackSo GetFontPack(InputDeviceType controllerType) => controllerType switch
        {
            InputDeviceType.Keyboard => GameInputs.InputDeviceIcons.PcFont,
            InputDeviceType.Xbox => GameInputs.InputDeviceIcons.XboxFont,
            InputDeviceType.Playstation => GameInputs.InputDeviceIcons.PlaystationFont,
            _ => GameInputs.InputDeviceIcons.PcFont
        };

        private void UpdateDeviceType()
        {
            GameInputs.InputDeviceChannel.Invoke(GetDeviceType());

            FontPackSo fontPack = GetFontPack(GetDeviceType());

            GameInputs.InputDeviceIcons.SetActiveFont(fontPack);
        }

        private void Awake() 
        {
            _gameInputAction = new GameInputAction();
            _cursorManager = GetComponent<CursorManager>();

            _startFont = GameInputs.InputDeviceIcons.ActiveFont;
            GameInputs.InputDeviceIcons.ForceInit();
        }

        private void Update()
        {
            UpdateDeviceType();

            CursorMan.SetOverhide(HasGamepad);

            GameInputs.RunChannel.Invoke(_gameInputAction.Map.Run.IsPressed());
            GameInputs.DashChannel.Invoke(_gameInputAction.Map.Dash.IsPressed());

            GameInputs.MousePositionChannel.Invoke(_gameInputAction.Map.MousePosition.ReadValue<Vector2>());

            GameInputs.LeftStickChannel.Invoke(_gameInputAction.Map.LeftStick.ReadValue<Vector2>());
            GameInputs.RightStickChannel.Invoke(_gameInputAction.Map.RightStick.ReadValue<Vector2>());

            GameInputs.MovementChannel.Invoke(_gameInputAction.Map.Movement.ReadValue<Vector2>());
        }

        private void OnEnable() => Link();
        private void OnDisable() => Unlink();

        private void Link()
        {
            _gameInputAction.Map.MouseDelta.performed += GameInputs.MouseDeltaChannel.Invoke<Vector2>;

            _gameInputAction.Map.Jump.performed += GameInputs.JumpChannel.Invoke;
            _gameInputAction.Map.Crouch.performed += GameInputs.CrouchChannel.Invoke;

            _gameInputAction.Map.Back.performed += GameInputs.BackChannel.Invoke;
            _gameInputAction.Map.Menu.performed += GameInputs.MenuChannel.Invoke;
            _gameInputAction.Map.ActionMenu.performed += GameInputs.ActionMenuChannel.Invoke;

            _gameInputAction.Map.Interact.performed += GameInputs.InteractChannel.Invoke;
            _gameInputAction.Map.PassDialog.performed += GameInputs.PassDialogChannel.Invoke;
            _gameInputAction.Map.ButtonInteract.performed += GameInputs.ButtonInteractChannel.Invoke;

            _gameInputAction.Map.Aim.performed += GameInputs.AimChannel.Invoke;
            _gameInputAction.Map.MeleeAttack.performed += GameInputs.MeleeAttackChannel.Invoke;
            _gameInputAction.Map.RangeAttack.performed += GameInputs.RangeAttackChannel.Invoke;

            _gameInputAction.Map.Up.performed += GameInputs.UpChannel.Invoke;
            _gameInputAction.Map.Down.performed += GameInputs.DownChannel.Invoke;
            _gameInputAction.Map.Left.performed += GameInputs.LeftChannel.Invoke;
            _gameInputAction.Map.Right.performed += GameInputs.RightChannel.Invoke;

            _gameInputAction.Map.Enable();
            GameInputs.InputDeviceIcons.ForceInit();
        }

        private void Unlink()
        {
            _gameInputAction.Map.Disable();

            _gameInputAction.Map.MouseDelta.performed -= GameInputs.MouseDeltaChannel.Invoke<Vector2>;
            _gameInputAction.Map.MousePosition.performed -= GameInputs.MousePositionChannel.Invoke<Vector2>;

            _gameInputAction.Map.LeftStick.performed -= GameInputs.LeftStickChannel.Invoke<Vector2>;
            _gameInputAction.Map.RightStick.performed -= GameInputs.RightStickChannel.Invoke<Vector2>;

            _gameInputAction.Map.Jump.performed -= GameInputs.JumpChannel.Invoke;
            _gameInputAction.Map.Crouch.performed -= GameInputs.CrouchChannel.Invoke;

            _gameInputAction.Map.Back.performed -= GameInputs.BackChannel.Invoke;
            _gameInputAction.Map.Menu.performed -= GameInputs.MenuChannel.Invoke;
            _gameInputAction.Map.ActionMenu.performed -= GameInputs.ActionMenuChannel.Invoke;

            _gameInputAction.Map.Interact.performed -= GameInputs.InteractChannel.Invoke;
            _gameInputAction.Map.PassDialog.performed -= GameInputs.PassDialogChannel.Invoke;
            _gameInputAction.Map.ButtonInteract.performed -= GameInputs.ButtonInteractChannel.Invoke;

            _gameInputAction.Map.Aim.performed -= GameInputs.AimChannel.Invoke;
            _gameInputAction.Map.MeleeAttack.performed -= GameInputs.MeleeAttackChannel.Invoke;
            _gameInputAction.Map.RangeAttack.performed -= GameInputs.RangeAttackChannel.Invoke;

            _gameInputAction.Map.Up.performed -= GameInputs.UpChannel.Invoke;
            _gameInputAction.Map.Down.performed -= GameInputs.DownChannel.Invoke;
            _gameInputAction.Map.Left.performed -= GameInputs.LeftChannel.Invoke;
            _gameInputAction.Map.Right.performed -= GameInputs.RightChannel.Invoke;

            GameInputs.InputDeviceIcons.SetActiveFont(_startFont);
        }
    
        private InputDeviceType GetDeviceType()
        {
            if (PressedKeyboard() || MovedMouse()) return InputDeviceType.Keyboard;
            else if (PressedGamepad())
            {
                if (HasXbox) return InputDeviceType.Xbox;
                else if (HasDualshock) return InputDeviceType.Playstation;
            }

            return GameInputs.InputDeviceChannel.Baked;
        }
    }
}

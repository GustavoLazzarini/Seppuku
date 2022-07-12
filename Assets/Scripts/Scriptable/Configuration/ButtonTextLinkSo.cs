//Copyright Galactspace Studios 2022

//References
using Core.Types;
using UnityEngine;
using Scriptable.InputSystem;
using UnityEngine.Localization;

namespace Scriptable.Configuration
{
	[CreateAssetMenu(menuName = "Game/Configuration/Button Text Link")]
	public class ButtonTextLinkSo : ScriptableObject
	{
		[Space]
		[SerializeField] private GameInputSo _gameInputSo;

		[Space]
		[SerializeField] private string _moveMousePc;		

		public char GetMoveMouse() => Choose(_moveMousePc, "", "");
		
		[Space]
		[SerializeField] private string _lStickXbox;
		[SerializeField] private string _lStickPs;

		public char GetLeftStick() => Choose("", _lStickXbox, _lStickPs);
		
		[Space]
		[SerializeField] private string _rStickXbox;
		[SerializeField] private string _rStickPs;

		public char GetRightStick() => Choose("", _rStickXbox, _rStickPs);

		[Space]
		[SerializeField] private string _runPc;
		[SerializeField] private string _runXbox;
		[SerializeField] private string _runPs;

		public char GetRun() => Choose(_runPc, _runXbox, _runPs);
		
		[Space]
		[SerializeField] private string _jumpPc;
		[SerializeField] private string _jumpXbox;
		[SerializeField] private string _jumpPs;

		public char GetJump() => Choose(_jumpPc, _jumpXbox, _jumpPs);

		[Space]
		[SerializeField] private string _movementPc;
		[SerializeField] private string _movementXbox;
		[SerializeField] private string _movementPs;

		public char GetMovement() => Choose(_movementPc, _movementXbox, _movementPs);

		[Space]
		[SerializeField] private string _menuPc;
		[SerializeField] private string _menuXbox;
		[SerializeField] private string _menuPs;

		public char GetMenu() => Choose(_menuPc, _menuXbox, _menuPs);

		[Space]
		[SerializeField] private string _actionMenuPc;
		[SerializeField] private string _actionMenuXbox;
		[SerializeField] private string _actionMenuPs;

		public char GetActionMenu() => Choose(_actionMenuPc, _actionMenuXbox, _actionMenuPs);

		[Space]
		[SerializeField] private string _interactPc;
		[SerializeField] private string _interactXbox;
		[SerializeField] private string _interactPs;

		public char GetInteract() => Choose(_interactPc, _interactXbox, _interactPs);

		[Space]
		[SerializeField] private string _passDialogPc;
		[SerializeField] private string _passDialogXbox;
		[SerializeField] private string _passDialogPs;

		public char GetPassDialog() => Choose(_passDialogPc, _passDialogXbox, _passDialogPs);

		[Space]
		[SerializeField] private string _btnInteractPc;
		[SerializeField] private string _btnInteractXbox;
		[SerializeField] private string _btnInteractPs;

		public char GetButtonInteract() => Choose(_btnInteractPc, _btnInteractXbox, _btnInteractPs);

		[Space]
		[SerializeField] private string _aimPc;
		[SerializeField] private string _aimXbox;
		[SerializeField] private string _aimPs;

		public char GetAim() => Choose(_aimPc, _aimXbox, _aimPs);

		[Space]
		[SerializeField] private string _backPc;
		[SerializeField] private string _backXbox;
		[SerializeField] private string _backPs;

		public char GetBack() => Choose(_backPc, _backXbox, _backPs);

		[Space]
		[SerializeField] private string _mAttackPc;
		[SerializeField] private string _mAttackXbox;
		[SerializeField] private string _mAttackPs;

		public char GetMeleeAttack() => Choose(_mAttackPc, _mAttackXbox, _mAttackPs);

		[Space]
		[SerializeField] private string _rAttackPc;
		[SerializeField] private string _rAttackXbox;
		[SerializeField] private string _rAttackPs;

		public char GetRangeAttack() => Choose(_rAttackPc, _rAttackXbox, _rAttackPs);

		[Space]
		[SerializeField] private string _upPc;
		[SerializeField] private string _upXbox;
		[SerializeField] private string _upPs;

		public char GetUp() => Choose(_upPc, _upXbox, _upPs);

		[Space]
		[SerializeField] private string _downPc;
		[SerializeField] private string _downXbox;
		[SerializeField] private string _downPs;

		public char GetDown() => Choose(_downPc, _downXbox, _downPs);

		[Space]
		[SerializeField] private string _leftPc;
		[SerializeField] private string _leftXbox;
		[SerializeField] private string _leftPs;

		public char GetLeft() => Choose(_leftPc, _leftXbox, _leftPs);

		[Space]
		[SerializeField] private string _rightPc;
		[SerializeField] private string _rightXbox;
		[SerializeField] private string _rightPs;

		public char GetRight() => Choose(_rightPc, _rightXbox, _rightPs);

		private char Choose(string pc, string xbox, string playstation)
		{
			switch (_gameInputSo.InputDeviceChannel.Baked)
			{
				case InputDeviceType.Keyboard:
				return (char)int.Parse(pc.Substring(2), System.Globalization.NumberStyles.HexNumber);

				case InputDeviceType.Xbox:
				return (char)int.Parse(xbox.Substring(2), System.Globalization.NumberStyles.HexNumber);

				case InputDeviceType.Playstation:
				return (char)int.Parse(playstation.Substring(2), System.Globalization.NumberStyles.HexNumber);

				default:
				return (char)int.Parse(pc.Substring(2), System.Globalization.NumberStyles.HexNumber);
			}
		}

		public string GetString(LocalizedString text) => GetString(text.GetLocalizedString());

		public string GetString(string text)
        {
            string finalString = "";
            char[] dividers = new [] { '{', '}' };
            string[] parcial = text.Split(dividers);

            for (int i = 0; i < parcial.Length; i++)
            {
                if (i % 2 != 0) //Impar
                {
                    char final = default;
                    string parcialReplace = parcial[i].ToLower();

                    switch (parcialReplace)
                    {
                        case "movemouse" or "mousemove":
                            final = GetMoveMouse();
                        break;
                        case "lstick" or "leftstick":
                            final = GetLeftStick();
                        break;
                        case "rstick" or "rightstick":
                            final = GetRightStick();
                        break;
                        case "run":
                            final = GetRun();
                        break;
                        case "jump":
                            final = GetJump();
                        break;
                        case "movement":
                            final = GetMovement();
                        break;
                        case "menu":
                            final = GetMenu();
                        break;
                        case "actionmenu":
                            final = GetActionMenu();
                        break;
                        case "interact":
                            final = GetInteract();
                        break;
                        case "passdialog" or "passdialogue":
                            final = GetPassDialog();
                        break;
                        case "btninteract" or "buttoninteract":
                            final = GetButtonInteract();
                        break;
                        case "aim":
                            final = GetAim();
                        break;
						case "back":
							final = GetBack();
						break;
                        case "melee" or "meleeattack":
                            final = GetMeleeAttack();
                        break;
                        case "range" or "rangeattack":
                            final = GetRangeAttack();
                        break;
                        case "up":
                            final = GetUp();
                        break;
                        case "down":
                            final = GetDown();
                        break;
                        case "left":
                            final = GetLeft();
                        break;
                        case "right":
                            final = GetRight();
                        break;
						default:
							final = '\u0000';
						break;
                    }
                    
                    parcial[i] = final.ToString();
                }
    
                finalString += parcial[i];
            }

            return finalString;
        }
	}
}

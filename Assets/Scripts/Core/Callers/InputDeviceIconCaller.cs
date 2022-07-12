//Made by Galactspace Studios

using TMPro;
using QFSW.QC;
using Core.Types;
using UnityEngine;
using Scriptable.InputSystem;

namespace Core.Callers
{
    [RequireComponent(typeof(TMP_Text))]
    public class InputDeviceIconCaller : Caller
    {
        private TMP_Text _text;
        private string _mainString;

        [SerializeField] private GameInputSo gameInput;

        private void Awake() 
        {
            _text = GetComponent<TMP_Text>();
            _mainString = _text.text;
        }

        protected override void OnEnableSub() => gameInput.InputDeviceChannel.Link(InputLink);
        private void OnDisable() => gameInput.InputDeviceChannel.Unlink(InputLink);

        private void InputLink(InputDeviceType arg) => Call();

        [Command("InputDeviceIconCaller.Call", MonoTargetType.All)]
        public override void Call() 
        {
            _text.SetText(gameInput.ButtonLink.GetString(_mainString));
        }
    
        public void Call(string arg)
        {
            _mainString = arg;
            Call();
        }
    }
}

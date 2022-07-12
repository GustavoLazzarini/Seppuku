//Made by Galactspace Studios

using UnityEngine;
using Scriptable.Popup;

namespace Core.Callers
{
    public class PopupCaller : Caller
    {
        [Space]
        [SerializeField] private PopupCallerSo callerSo;

        [Space]
        [SerializeField] private GameObject prefab;
        [SerializeField] private string mainText;

        public override void Call() => callerSo.ShowPopupChannel.Invoke(prefab, mainText);
    }
}

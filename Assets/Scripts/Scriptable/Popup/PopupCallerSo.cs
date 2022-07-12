//Copyright Galactspace Studios 2022

//References
using System;
using UnityEngine;
using Scriptable.Generic;
using Scriptable.Configuration;

namespace Scriptable.Popup
{
	[CreateAssetMenu(menuName = "Game/Popup/Popup Caller")]
	public class PopupCallerSo : ScriptableObject
	{
		[Space]
		[SerializeField] private ButtonTextLinkSo _linker;

		[Space]
		[SerializeField] private GameObjectStringChannelSo _showPopupChannel;
		[SerializeField] private ChannelSo _hidePopupChannel;

		public ButtonTextLinkSo Linker => _linker;

		public GameObjectStringChannelSo ShowPopupChannel => _showPopupChannel;
		public ChannelSo HidePopupChannel => _hidePopupChannel;

		public void Link(Action<GameObject, string> onShowPopup, Action onHidePopup)
        {
			ShowPopupChannel.Link(onShowPopup);
			HidePopupChannel.Link(onHidePopup);
        }

		public void Unlink(Action<GameObject, string> onShowPopup, Action onHidePopup)
        {
			ShowPopupChannel.Unlink(onShowPopup);
			HidePopupChannel.Unlink(onHidePopup);
        }
	}
}

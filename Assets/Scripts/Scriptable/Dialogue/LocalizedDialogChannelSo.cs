//Copyright Galactspace Studios 2022

//References
using Core.Types;
using UnityEngine;
using Scriptable.Abstract;
using UnityEngine.Localization;

namespace Scriptable.Dialogue
{
	[CreateAssetMenu(menuName = "Game/Dialogue/Channels/Localized Dialog Channel")]
	public class LocalizedDialogChannelSo : ChannelSo<LocalizedDialogueSo>
	{}
}

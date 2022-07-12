//Copyright Galactspace Studios 2022

//References
using UnityEngine;
using Scriptable.Generic;
using Scriptable.InputSystem;
using Scriptable.Configuration;

namespace Scriptable.Dialogue
{
	[CreateAssetMenu(menuName = "Game/Dialogue/Dialogue Caller")]
	public class DialogueCallerSo : ScriptableObject
	{
		[Space]
		[SerializeField] private GameObject _prefab;
		public GameObject Prefab => _prefab;

		[Space]
		[SerializeField] private DialogueConfigurationSo _configuration;
		public DialogueConfigurationSo Configuration => _configuration;
		
		[SerializeField] private ButtonTextLinkSo _buttonLink;
		public ButtonTextLinkSo ButtonLink => _buttonLink;

		[SerializeField] private InputDeviceTypeChannelSo _inputDeviceChannel;
		public InputDeviceTypeChannelSo InputDeviceChannel => _inputDeviceChannel;

		[SerializeField] private ChannelSo _passDialogChannel;
		public ChannelSo PassDialogChannel => _passDialogChannel;
		
		[Space]
		[SerializeField] private LocalizedDialogChannelSo _startDialogChannel;
		public LocalizedDialogChannelSo StartDialogChannel => _startDialogChannel;

		[SerializeField] private ChannelSo _dialogStartedChannel;
		public ChannelSo DialogStartedChannel => _dialogStartedChannel;

		[SerializeField] private ChannelSo _dialogPassedChannel;
		public ChannelSo DialogPassedChannel => _dialogPassedChannel;
		
		[SerializeField] private ChannelSo _dialogEndedChannel;
		public ChannelSo DialogEndedChannel => _dialogEndedChannel;
	}
}

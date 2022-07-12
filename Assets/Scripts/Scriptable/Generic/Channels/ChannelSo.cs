//Made by Galactspace Studios

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scriptable.Generic
{
	[CreateAssetMenu(menuName = "Game/Generic/Channels/Channel")]
    public class ChannelSo : ScriptableObject
    {
		//Variables
		public Action Channel = delegate {};

		[Space]
		[SerializeField] private bool _log;

		//Methods
		public void Invoke() 
		{
			Channel?.Invoke();	
			if (_log) DebugManager.Engine($"[{name}] Invoked");
		}
		
		public void Invoke(InputAction.CallbackContext arg) 
		{
			Channel?.Invoke();
			if (_log) DebugManager.Engine($"[{name}] Invoked");
		}
		
		public void Link(Action onInvoke) => Channel += onInvoke;
		public void Unlink(Action onInvoke) => Channel -= onInvoke;
    }
}
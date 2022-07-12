//Copyright Galactspace Studios 2022

//References
using System;
using UnityEngine;
using Scriptable.Generic;

namespace Scriptable.Transition
{
	[CreateAssetMenu(menuName = "Game/Transition/Transition Caller")]
	public class TransitionCallerSo : ScriptableObject
	{
		[Space]
		[SerializeField] private ActionChannelSo _inChannel;
		public ActionChannelSo InChannel => _inChannel;

		[SerializeField] private ActionChannelSo _outChannel;
		public ActionChannelSo OutChannel => _outChannel;

		public void Link(Action<Action> transitionIn, Action<Action> transitionOut)
		{
			InChannel.Channel += transitionIn;
			OutChannel.Channel += transitionOut;
		}

		public void Unlink(Action<Action> transitionIn, Action<Action> transitionOut)
		{
			InChannel.Channel -= transitionIn;
			OutChannel.Channel -= transitionOut;
		}
	}
}

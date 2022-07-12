//Copyright Galactspace Studios 2022

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Scriptable.Abstract
{
	public abstract class ChannelSo<T> : ScriptableObject
	{
		//Variables
		public Action<T> Channel = delegate {};

		[Space]
		[SerializeField] private bool _log;

		[Space]
		[SerializeField] private T _baked;
		public T Baked => _baked;

		//Methods
		public void Bake(T arg)
		{
			_baked = arg;
			if (_log) DebugManager.Engine($"[{name}] Backed value '{arg}'");
		}

		public void Invoke(T arg)
		{
			Bake(arg);
			Channel?.Invoke(arg);
			if (_log) DebugManager.Engine($"[{name}] Invoked with value '{arg}'");
		}
		
		public void Invoke<TRes>(InputAction.CallbackContext arg) where TRes : struct
		{
			if (typeof(TRes) != typeof(T)) return;

			T tValue = (T)(object)arg.ReadValue<TRes>();
			
			Bake(tValue);
			Channel?.Invoke(tValue);
			if (_log) DebugManager.Engine($"[{name}] Invoked with value '{tValue}'");
		}

		public void Link(Action<T> onInvoke)
		{
			Channel += onInvoke;
		}

		public void Unlink(Action<T> onInvoke)
		{
			Channel -= onInvoke;
		}
		
		public void Link(UnityEvent<T> onInvoke)
		{
			Channel += onInvoke.Invoke;
		}

		public void Unlink(UnityEvent<T> onInvoke)
		{
			Channel -= onInvoke.Invoke;
		}
	}

	public abstract class ChannelSo<T, T2> : ScriptableObject
	{
		//Variables
		public Action<T, T2> Channel = delegate {};

		[Space]
		[SerializeField] private bool _log;

		[Space]
		[SerializeField] private T _baked0;
		public T Baked0 => _baked0;
		
		[SerializeField] private T2 _baked1;
		public T2 Baked1 => _baked1;

		//Methods		
		public void Bake(T arg0, T2 arg1)
		{
			_baked0 = arg0;
			_baked1 = arg1;
			if (_log) DebugManager.Engine($"[{name}] Backed");
		}

		public void Invoke(T arg0, T2 arg1)
		{
			Bake(arg0, arg1);
			Channel?.Invoke(arg0, arg1);
			if (_log) DebugManager.Engine($"[{name}] Invoked");
		}
		
		public void Link(Action<T, T2> onInvoke)
		{
			Channel += onInvoke;
		}

		public void Unlink(Action<T, T2> onInvoke)
		{
			Channel -= onInvoke;
		}
	}

	public abstract class ChannelSo<T, T2, T3> : ScriptableObject
	{
		//Variables
		public Action<T, T2, T3> Channel = delegate {};

		[Space]
		[SerializeField] private T _baked0;
		public T Baked0 => _baked0;
		
		[SerializeField] private T2 _baked1;
		public T2 Baked1 => _baked1;
		
		[SerializeField] private T3 _baked2;
		public T3 Baked2 => _baked2;

		//Methods		
		public void Bake(T arg0, T2 arg1, T3 arg2)
		{
			_baked0 = arg0;
			_baked1 = arg1;
			_baked2 = arg2;
		}

		public void Invoke(T arg0, T2 arg1, T3 arg2)
		{
			Bake(arg0, arg1, arg2);
			Channel?.Invoke(arg0, arg1, arg2);
		}
	
		public void Link(Action<T, T2, T3> onInvoke)
		{
			Channel += onInvoke;
		}

		public void Unlink(Action<T, T2, T3> onInvoke)
		{
			Channel -= onInvoke;
		}
	}
}
//Copyright Galactspace Studios 2022

//References
using UnityEngine;
using Scriptable.Generic;

namespace Scriptable.Cursor
{
	[CreateAssetMenu(menuName = "Game/Cursor/Cursor Caller")]
	public class CursorCallerSo : ScriptableObject
	{
		//Variables
		[SerializeField] private Texture2DChannelSo _textureChannel;		
		[SerializeField] private BoolChannelSo _visibilityChannel;
		[SerializeField] private CursorLockModeChannelSo _lockModeChannel;
		[SerializeField] private BoolChannelSo _overHideChannel;

		[Space]
		[SerializeField] private Texture2D _normal;
		[SerializeField] private Texture2D _down;
		[SerializeField] private Texture2D _hand;
		[SerializeField] private Texture2D _write;

		public Texture2D Normal => _normal;
		public Texture2D Down => _down;
		public Texture2D Hand => _hand;
		public Texture2D Write => _write;

		public Texture2DChannelSo TextureChannel => _textureChannel;
		public BoolChannelSo VisibilityChannel => _visibilityChannel;
		public CursorLockModeChannelSo LockmodeChannel => _lockModeChannel;
		public BoolChannelSo OverHideChannel => _overHideChannel;

		public void Invoke(bool visibility, CursorLockMode lockMode, Texture2D cursorTexture)
		{
			bool hasTexture = cursorTexture != null;

			LockmodeChannel.Invoke(lockMode);
			VisibilityChannel.Invoke(visibility);
			
			if (hasTexture) TextureChannel.Invoke(cursorTexture);
		}
	}
}

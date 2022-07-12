//Made by Galactspace Studios

using UnityEngine;
using Scriptable.Cursor;

namespace Core.Callers
{
    public class CursorCaller : Caller
    {
        [Space]
        [SerializeField] private CursorCallerSo cursorCaller;

        [Space]
        [SerializeField] private bool cursorVisibility;
        [SerializeField] private Texture2D cursorSprite;
        [SerializeField] private CursorLockMode cursorLockMode;

        public override void Call()
        {
            if (cursorSprite != null)
                cursorCaller.TextureChannel?.Invoke(cursorSprite);

            cursorCaller.LockmodeChannel?.Invoke(cursorLockMode);
            cursorCaller.VisibilityChannel?.Invoke(cursorVisibility);
        }

        public void Call(bool visibility, Texture2D sprite, CursorLockMode lockMode)
        {
            if (sprite != null)
                cursorCaller.TextureChannel?.Invoke(sprite);

            cursorCaller.LockmodeChannel?.Invoke(lockMode);
            cursorCaller.VisibilityChannel?.Invoke(visibility);
        }
    }
}

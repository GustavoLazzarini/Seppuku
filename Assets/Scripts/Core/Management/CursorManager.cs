//Made by Galactspace Studios

using UnityEngine;

namespace Core.Management
{
    public class CursorManager : MonoBehaviour
    {
        private bool _overhide;
        private bool _overshow;

        private bool _cursorVisibility;
        private Texture2D _cursorTexture;
        private CursorLockMode _cursorLockMode;

        public bool Overhide => _overhide;
        public bool Overshow => _overshow;

        public bool CursorVisibility => _cursorVisibility;
        public Texture2D CursorTexture => _cursorTexture;
        public CursorLockMode CursorLockMode => _cursorLockMode;

        private void Awake()
        {
            _cursorVisibility = Cursor.visible;
            _cursorLockMode = Cursor.lockState;
        }

        public void SetCursorVisibility(bool visibility)
        {
            if (!_overhide && !_overshow) Cursor.visible = visibility;
            _cursorVisibility = visibility;
        }

        public void SetCursorTexture(Texture2D texture)
        {
            if (!_overhide && !_overshow) Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
            _cursorTexture = texture;
        }

        public void SetCursorLockState(CursorLockMode lockMode)
        {
            if (!_overhide && !_overshow) Cursor.lockState = lockMode;
            _cursorLockMode = lockMode;
        }

        public void SetOverhide(bool value)
        {
            if (value && _overshow) SetOverhide(false);
            if (value == _overhide) return;

            _overhide = value;

            if (value)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = _cursorVisibility;
                Cursor.lockState = _cursorLockMode;
                Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            }
        }

        public void SetOvershow(bool value)
        {
            if (value && _overhide) SetOvershow(false);
            if (value == _overshow) return;

            _overshow = value;

            if (value)
            {
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = _cursorVisibility;
                Cursor.lockState = _cursorLockMode;
                Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            }
        }
    }
}

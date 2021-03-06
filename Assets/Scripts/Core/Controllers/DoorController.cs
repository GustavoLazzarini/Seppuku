//Created by Galactspace

using UnityEngine;
using Core.Attributes;

namespace Core.Controllers
{
    public class DoorController : MonoBehaviour
    {
        public bool IsOpened { get => _isOpened; private set => _isOpened = value; }

        [SerializeField] private bool _isOpened;

        [Space]
        [SerializeField] private float _openSpeed = 4;
        [SerializeField] private float _slowOpenSpeed = 4;

        [Space]
        [SerializeField] private Vector3 _openedPosition;
        [SerializeField] private Vector3 _closedPosition;

        [Space]
        [SerializeField][Button] private bool _openDoor;
        [SerializeField][Button] private bool _closeDoor;

        private void OnValidate()
        {
            if (_openedPosition == Vector3.zero) _openedPosition = transform.position + new Vector3(0, 1, 0);
            if (_closedPosition == Vector3.zero) _closedPosition = transform.position;

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                _openDoor = false;
                _closeDoor = false;
            }

            if (_openDoor)
            {
                Open();
                _openDoor = false;
            }

            if (_closeDoor)
            {
                Close();
                _closeDoor = false;
            }
#endif
        }

        public void Open(bool slow)
        {
            if (IsOpened) return;

            IsOpened = true;
            gameObject.LerpPosition(this, _openedPosition, slow ? _slowOpenSpeed : _openSpeed);
        }

        public void Close(bool slow)
        {
            if (!IsOpened) return;

            IsOpened = false;
            gameObject.LerpPosition(this, _closedPosition, slow ? _slowOpenSpeed : _openSpeed);
        }

        public void Open() => Open(false);
        public void Close() => Close(false);

        public void Use(bool isOpened)
        {
            if (isOpened) Open();
            else Close();
        }

        public void Use(bool isOpened, bool slow)
        {
            if (isOpened) Open(slow);
            else Close(slow);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmosf.DrawSphere(_openedPosition, 0.1f, new(0, 1, 0, 1));
            Gizmosf.DrawSphere(_closedPosition, 0.1f, new(1, 0, 0, 1));
            Gizmosf.DrawLine(_openedPosition, _closedPosition, new(1, 1, 1, 1), 3);
        }
    }
}
//Made by Galactspace Studios

using UnityEngine;
using Cinemachine;
using Scriptable.Camera;

namespace Core.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraBase current;
        [SerializeField] private CinemachineCameraChannelSo camChannel;

        private void OnEnable() => camChannel.Link(SetCamera);
        private void OnDisable() => camChannel.Unlink(SetCamera);

        private void Start() 
        {
            foreach (CinemachineVirtualCamera cam in FindObjectsOfType<CinemachineVirtualCamera>())
                cam.Priority = 0;

            if (HasCurrent) SetCamera(current);
        }
        
        private void SetCurrentActive(bool value) => current.gameObject.SetActive(value);
        private void SetPriority(int value) => current.Priority = value;
        
        private bool HasCurrent => current != null;

        public void SetCamera(CinemachineVirtualCameraBase camera)
        {
            if (HasCurrent)
            {
                SetPriority(0);
                SetCurrentActive(false);
            }

            current = camera;
            
            SetPriority(1);
            SetCurrentActive(true);
        }
    }
}

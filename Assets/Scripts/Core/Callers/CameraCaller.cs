//Made by Galactspace Studios

using UnityEngine;
using Cinemachine;
using Scriptable.Camera;

namespace Core.Callers
{
    public class CameraCaller : Caller
    {
        [Space]
        [SerializeField] private CinemachineCameraChannelSo camChannel;
        [SerializeField] private CinemachineVirtualCameraBase target;

        public override void Call() => camChannel.Invoke(target);
        public void Call(CinemachineVirtualCameraBase target) => camChannel.Invoke(target);
    }
}

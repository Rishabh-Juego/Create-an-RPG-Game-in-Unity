using Unity.Cinemachine;
using UnityEngine;

namespace TGL.RPG.CameraManagement
{
    public interface IActiveCameraProvider
    {
        Camera ActiveCamera { get; }
        CinemachineCamera CameraSettings { get; }
    }
}
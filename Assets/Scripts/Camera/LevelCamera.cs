using Unity.Cinemachine;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    CinemachineCamera cineMachineCamera;

    void Awake()
    {
        cineMachineCamera = GetComponentInChildren<CinemachineCamera>(true);
    }

    public void EnableCamera(bool enableCamera)
    {
        cineMachineCamera.gameObject.SetActive(enableCamera);
    }

    public void SetNewTarget(Transform target)
    {
        cineMachineCamera.Follow = target;
    }
}

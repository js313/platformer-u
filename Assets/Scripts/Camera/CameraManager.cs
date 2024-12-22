using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [Header("Camera Shake")]
    [SerializeField] private Vector2 shakeVelocity;

    CinemachineImpulseSource impulseSource;

    void Awake()
    {
        instance = this;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake()
    {
        impulseSource.DefaultVelocity = shakeVelocity;
        impulseSource.GenerateImpulse();
    }
}

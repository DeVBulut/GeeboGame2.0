using Unity.Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineCamera cinemachineVirtualCamera; 
    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineCamera>();
    } 

    private void ShakeCamera(float intensity, float time)
    {
        //CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
}

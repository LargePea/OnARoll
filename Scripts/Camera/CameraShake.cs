using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera _vCamera;
    private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

    private void Awake()
    {
        _vCamera = GetComponent<CinemachineVirtualCamera>();
        _multiChannelPerlin = _vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator Shake (float duration, float magnitude)
    {
        float elapsed = 0.0f;
        _multiChannelPerlin.m_AmplitudeGain = magnitude;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

            
    }
}

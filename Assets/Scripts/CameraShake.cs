using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachinePerlin;

    private float _shakeTime = 0f;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachinePerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    internal void Shake(float intensity, float time)
    {
        _cinemachinePerlin.m_AmplitudeGain = intensity;
        _shakeTime = time;
    }

    private void Update()
    {
        if (_shakeTime > 0f)
        {
            _shakeTime -= Time.deltaTime;
            if (_shakeTime <= 0f)
            {
                _shakeTime = 0f;
                _cinemachinePerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}

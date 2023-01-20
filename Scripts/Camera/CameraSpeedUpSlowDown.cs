using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
 
public class CameraSpeedUpSlowDown : MonoBehaviour
{
    [SerializeField] private float _speedUpOffset;
    [SerializeField] private float _slowDownOffset;
    [SerializeField, Range(0, 1)] private float _smoothness = 0.5f;

    private float _moveInput = 0;
    private float _lerpValue;
    private float _lastOffset = 0;
    private float _currentOffset = 0;
    private float _lastPathPosition;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineTrackedDolly _cinemachineTrackedDolly;


    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineTrackedDolly = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void Update()
    {
        
        if (_lerpValue == 0) return;

        if(_currentOffset == 0 && _lastOffset == _slowDownOffset)
        {
            _cinemachineTrackedDolly.m_AutoDolly.m_PositionOffset = Mathf.Clamp(_cinemachineTrackedDolly.m_AutoDolly.m_PositionOffset - (_lerpValue * Time.deltaTime), 0, _slowDownOffset);
        }
        else if (_currentOffset == 0 && _lastOffset == _speedUpOffset)
        {
            _cinemachineTrackedDolly.m_AutoDolly.m_PositionOffset = Mathf.Clamp(_cinemachineTrackedDolly.m_AutoDolly.m_PositionOffset - (_lerpValue * Time.deltaTime), _speedUpOffset, 0);
        }
        else
        {
            _cinemachineTrackedDolly.m_AutoDolly.m_PositionOffset = Mathf.Clamp(_cinemachineTrackedDolly.m_AutoDolly.m_PositionOffset + (_lerpValue * Time.deltaTime), _speedUpOffset, _slowDownOffset);
        }
    }

    private void FixedUpdate()
    {
        CameraSecondaryCheck();
    }

    private void CameraSecondaryCheck()
    {
        //if the camera doesnt move along the path for 1 frame bump it forward
        if(_lastPathPosition == _cinemachineTrackedDolly.m_PathPosition)
        {
            _cinemachineTrackedDolly.m_AutoDolly.m_PositionOffset = 0;
        }
        else
        {
            _lastPathPosition = _cinemachineTrackedDolly.m_PathPosition;
        }  
    }

    public void OffsetCamera(float moveInput)
    {
        if (_currentOffset == 0 && moveInput == 0) return;
        _lastOffset = _currentOffset;
        _moveInput = moveInput;
        if(_moveInput < 0)
        {
            _currentOffset = _slowDownOffset;
            _lerpValue = Mathf.Lerp(_lastOffset, _currentOffset, _smoothness);
        }
        else if(_moveInput > 0)
        {
            _currentOffset = _speedUpOffset;
            _lerpValue = Mathf.Lerp(_lastOffset, _currentOffset, _smoothness);
        }
        else
        {
            _currentOffset = 0f;
        } 
    }
}

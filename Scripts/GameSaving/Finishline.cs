using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Finishline : MonoBehaviour
{
    [SerializeField] private UnityEvent _victory;
    [SerializeField] private Timer _timer;
    [SerializeField] private CinemachineVirtualCamera _vCam;

    private CinemachineTrackedDolly _trackedDolly;
    private void Start()
    {
        _trackedDolly = _vCam.GetCinemachineComponent<CinemachineTrackedDolly>();   
    }

    private bool _savedGame = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();
        BoulderController boulderController = collision.GetComponentInParent<BoulderController>();

        if((boulderController != null || playerController != null) && !_savedGame)
        {
            SaveTime.SaveTimes(_timer);
            _savedGame = true;
            _victory.Invoke();
        }
    }

    private void Update()
    {
        if (!_savedGame) return;
        if (_trackedDolly.m_PathPosition > 1275.5f)
        {
            _trackedDolly.m_AutoDolly.m_Enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private Vector3 _endPoint;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Image _progressBar;

    private float _distanceCovered;
    private float _totalDistance;
    private float _progress;
    private bool _trackProgress = true;

    private void Start()
    {
        _totalDistance = (_endPoint - _startPoint).magnitude;
    }

    private void Update()
    {
        if (!_trackProgress) return;
        _distanceCovered = (_playerPosition.position.x - _startPoint.x) > 0 ? (_playerPosition.position - _startPoint).magnitude : 0;
        _progress = Mathf.Clamp01(_distanceCovered / _totalDistance);
        _progressBar.fillAmount = _progress;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_startPoint, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_endPoint, 0.5f);
    }

    public void StopTrackingProgress()
    {
        _trackProgress = false;
    }
}

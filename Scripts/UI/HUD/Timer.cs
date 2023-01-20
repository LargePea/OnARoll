using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private bool _runTimer = false;
    private float _timer = 0f;
    private float _minutes = 0f;
    private float _seconds = 0f;
    public float TotalTime => _timer;

    public float Minutes => _minutes;
    public float Seconds => _seconds;

    private void OnEnable()
    {
        _minutes = _timer / 60f;
        _minutes = Mathf.FloorToInt(_minutes);
        _seconds = _timer % 60f;
        string displayTime = string.Format("{0:00}:{1:00.00}s", _minutes, _seconds);
        _text.text = displayTime;
    }

    private void Update()
    {
        if (!_runTimer) return;

        _timer += Time.deltaTime;
        _minutes = _timer / 60f;
        _minutes = Mathf.FloorToInt(_minutes);
        _seconds = _timer % 60f;
        string displayTime = string.Format("{0:00}:{1:00.00}s", _minutes, _seconds);
        _text.text = displayTime;
    }

    public void StartTimer()
    {
        _runTimer = true;
    }

    public void ResetTimer()
    {
        _timer = 0f;
    }

    public void StopTimer()
    {
        _runTimer = false;
    }

    public void SetTimer(float time)
    {
        _timer = time;
    }
}

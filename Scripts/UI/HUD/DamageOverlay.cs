using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour
{
    [SerializeField] private Sprite[] _crackedStates;
    [SerializeField] private Image _image;

    private bool _isPlayerDamaged;
    private float _healingIntervals;
    private int _crackedState = 0;
    private float _lastHealTime;


    private void Update()
    {
        if(_isPlayerDamaged &&
            Time.time >= _lastHealTime + _healingIntervals)
        {
            _image.sprite = _crackedStates[_crackedState];
            _lastHealTime = Time.time;
            _crackedState++;
            if(_crackedState >= _crackedStates.Length)
            {
                _crackedState = 0;
                _isPlayerDamaged = false;
            }
        }
    }

    public void SetScreenCrack(float playerHealTime)
    {
        _healingIntervals = playerHealTime / _crackedStates.Length;
        _isPlayerDamaged = true;
        _lastHealTime = Time.time - _healingIntervals;
        _crackedState = 0;
    }
}

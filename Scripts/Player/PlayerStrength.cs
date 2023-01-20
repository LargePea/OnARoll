using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrength : MonoBehaviour
{
    [SerializeField] private float _maxStrength;
    [SerializeField] private float _refillRatePercentage; //Refills on a percentage based on _maxStrength per second

    private PlayerState _playerState;
    private PlayerMovement _playerMovement;

    public float _currentStrength { get; private set; }

    //public getters for strength meter ui
    public float StrengthPercentage => _currentStrength / _maxStrength;
    public float MaxStrength => _maxStrength;

    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _playerMovement = GetComponent<PlayerMovement>();
        _currentStrength = _maxStrength / 2;
    }

    private void Update()
    {
        //Regenerate strength while rolling the boulder and not stunned
        if(_playerState.State == EPlayerState.Roll && _playerMovement.IsGrounded && !_playerState.IsStunned)
        {
            _currentStrength = Mathf.Clamp(_currentStrength + _maxStrength * _refillRatePercentage * Time.deltaTime, 0, _maxStrength);
        }
    }

    public void RefillStrength(float refillPercentage = 1)
    {
        if (_currentStrength >= _maxStrength * refillPercentage) return;
        _currentStrength = _maxStrength * refillPercentage;
    }

    public float UseStrength(float strengthUsed = 1)
    {
        //if player has infinite strength don't consume any strength
        if(DebugManager.InfiniteStrength) return strengthUsed;

        //calculate how much strength to use if usage is more that what the player has
        if (_currentStrength < strengthUsed)
        {
            strengthUsed = _currentStrength;
        }

        _currentStrength = Mathf.Clamp(_currentStrength - strengthUsed, 0, _maxStrength);
        return strengthUsed;
    }

}

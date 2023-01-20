using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthBar : MonoBehaviour
{
    //Player and Bar Objects
    [SerializeField] private GameObject _player;
    [Header("UI")]
    [SerializeField] private Image _mainFill;
    [SerializeField] private Image _secondaryFill;
    [SerializeField, Range(0,1)] private float _decreaseSmoothness = 0.5f;

    private PlayerStrength _playerStrength;
    private PlayerController _playerController;

    private float _holdStartTime;
    private float _boulderChargeTime => _playerController.BoulderChargeTime;
    private float _secondarybarDecreaseRate;
    private bool _isCharging = false;

    private void Awake()
    {
        _playerController = _player.GetComponent<PlayerController>();
        _playerStrength = _player.GetComponent<PlayerStrength>();
    }

    private void Start()
    {
        _mainFill.fillAmount = _playerStrength.StrengthPercentage;
        _secondaryFill.fillAmount = _playerStrength.StrengthPercentage;
    }

    private void Update()
    {
        if(_isCharging)
        {
            //decrease main meter
            _mainFill.fillAmount = _playerStrength.StrengthPercentage - (Mathf.Clamp01((Time.timeSinceLevelLoad - _holdStartTime) / _boulderChargeTime) / _playerStrength.MaxStrength);
        }
        //Bar is static and not dynamically changing
        else if (!_isCharging && _secondaryFill.fillAmount < _playerStrength.StrengthPercentage)
        {
            //set both fills to equal player's current strength percentage
            _mainFill.fillAmount = _playerStrength.StrengthPercentage;
            _secondaryFill.fillAmount = _playerStrength.StrengthPercentage;
        }
        //Decrease secondary bar if it is not fully decreased yet
        else if (!_isCharging &&
                 _secondaryFill.fillAmount > _mainFill.fillAmount)
        {
            //decrease secondary meter with lerp
            _secondaryFill.fillAmount -= _secondarybarDecreaseRate * Time.deltaTime;
        }
    }

    public void StartCharge(float holdStartTime)
    {
        _holdStartTime = holdStartTime;
        _isCharging = true;
    }

    public void EndCharge()
    {
        _secondarybarDecreaseRate = Mathf.Lerp(_secondaryFill.fillAmount, _mainFill.fillAmount, _decreaseSmoothness);
        _isCharging = false;
    }
}

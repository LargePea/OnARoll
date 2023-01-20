using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _healingSpeed;
    [SerializeField] private UnityEvent _OnDeath;
    [SerializeField] private UnityEvent<float> _OnDamage;

    [Header("Death")]
    [SerializeField] private string _deathLayer;
    private ShaderFloatController _shaderController;
    public int CurrentHealth { get; private set; }

    private PlayerMovement _playerMovement;
    private PlayerState _playerStateController;

    //Audio variables
    private PlayerDamageAudio _playerDamageAudio;
    private PlayerDeathAudio _playerDeathAudio;
    private Coroutine _deathCo;

    private float _healTime;

    private void Awake()
    {
        CurrentHealth = _maxHealth;
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStateController = GetComponent<PlayerState>();
        _playerDamageAudio = GetComponentInChildren<PlayerDamageAudio>();
        _playerDeathAudio = GetComponentInChildren<PlayerDeathAudio>();
        _shaderController = GetComponent<ShaderFloatController>();
    }

    private void Update()
    {
        if(_healTime <= Time.timeSinceLevelLoad && CurrentHealth < _maxHealth)
        {
            CurrentHealth++;
        }
    }

    public void TakeDamage(int damage, float speedChange, float stunTime, bool playSound = true)
    {
        if (playSound && CurrentHealth > 0)
        {
            _playerDamageAudio.PlaySound();
        }
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);
        
        StartCoroutine(DamagedSpeedChange(speedChange, stunTime));

        if(CurrentHealth <= 0)
        {
            Debug.Log("Player Died");
            if(_deathCo == null)
            {
                _deathCo = StartCoroutine(Death());   
            }
            return;
        }

        _healTime = Time.timeSinceLevelLoad + _healingSpeed;
        _OnDamage.Invoke(_healingSpeed);
    }

    private IEnumerator DamagedSpeedChange(float speedChange, float stunTime)
    {
        _playerMovement.ChangeSpeed(speedChange);
        yield return StartCoroutine(_playerStateController.StunPlayer(stunTime));
        _playerMovement.ChangeSpeed();
    }

    private IEnumerator Death()
    {
        _playerStateController.ChangeStateToDead();
        StartCoroutine(_shaderController.DecreaseValue());
        _OnDeath.Invoke();
        SetAllLayers(gameObject);
        yield return _playerDeathAudio.PlaySound();
    }

    private void SetAllLayers(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(_deathLayer);

        foreach(Transform child in obj.transform)
        {
            SetAllLayers(child.gameObject);
        }
    }
}

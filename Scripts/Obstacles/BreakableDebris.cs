using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableDebris : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [Header("Debuffs")]
    [SerializeField] private float _speedDebuff;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _debuffTime;

    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _playerHealth = _player.GetComponent<PlayerHealth>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DebugManager.Invulnerability) _damage = 0;
        BoulderController boulderController = collision.gameObject.GetComponentInParent<BoulderController>();
        if (boulderController != null)
        {
            if(boulderController.State == EBoulderState.Freefall)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        _playerHealth.TakeDamage(_damage, _speedDebuff, _debuffTime);
        gameObject.SetActive(false);
        return;
    }
}

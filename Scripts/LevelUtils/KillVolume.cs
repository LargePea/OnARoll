using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    [SerializeField] PlayerHealth _playerHealth;
    private bool _isActivated;

    private void Awake()
    {
        StartCoroutine(FetchPlayerHealth());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BoulderController boulderController = collision.GetComponentInParent<BoulderController>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();

        if((boulderController != null || playerController != null) && !_isActivated)
        {
            _playerHealth.TakeDamage(_playerHealth.CurrentHealth, 0, 0, false);
            _isActivated = true;
        }
    }

    private IEnumerator FetchPlayerHealth()
    {
        while (_playerHealth == null)
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
            yield return null;
        }
    }
}

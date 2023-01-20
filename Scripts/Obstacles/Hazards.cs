using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    [SerializeField] protected int _damage = 1;
    [SerializeField] protected float _speedDebuff;
    [SerializeField] protected float _debuffDuration;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();

        //If player is invicible then dont take any damage
        if (DebugManager.Invulnerability) return;
        if(playerHealth != null)
        {
            playerHealth.TakeDamage(_damage, _speedDebuff, _debuffDuration);
        }
    }

}

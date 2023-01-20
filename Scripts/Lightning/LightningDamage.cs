using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDamage : Hazards
{
    private ZeusLaugh _zeusLaugh;
    private void Awake()
    {
        _zeusLaugh = GetComponent<ZeusLaugh>();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        _zeusLaugh.PlaySound();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrigger : MonoBehaviour
{
    [SerializeField] private float _chargeTime;

    private LightningManager _lightingManager;
    private ZeusTaunt _zeusTaunt;
    private static bool coroutineRunning;

    private void Awake()
    {
        _lightingManager = FindObjectOfType<LightningManager>();
        _zeusTaunt = GetComponent<ZeusTaunt>();
        coroutineRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();

        if (playerController != null && !coroutineRunning)
        {
            StartCoroutine(StrikeLightining(_chargeTime));
        }
    }

    private IEnumerator StrikeLightining(float chargeTime)
    {
        coroutineRunning = true; 
        yield return _zeusTaunt.PlaySound();
        _lightingManager.StrikeLighting(chargeTime);
        coroutineRunning = false;
    }
}

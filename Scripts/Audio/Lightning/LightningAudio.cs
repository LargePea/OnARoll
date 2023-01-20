using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightningAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _lightningSource;
    [SerializeField] private AudioClip _lightningCharge;
    [SerializeField] private AudioClip _lightningStrike;
    [SerializeField] private UnityEvent _cameraShake;

    private void PlayChargeSound()
    {
        _lightningSource.clip = _lightningCharge;
        _lightningSource.Play();
    }

    private void PlayStrikeSound()
    {
        _lightningSource.clip = _lightningStrike;
        _lightningSource.Play();
    }

    public IEnumerator PlaySound(float delay, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayChargeSound();
        yield return new WaitForSeconds(delay);
        PlayStrikeSound();
        _cameraShake.Invoke();
    }
}

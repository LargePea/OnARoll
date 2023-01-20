using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictorySequence : MonoBehaviour
{
    [Header("Brazier Ignition")]
    [SerializeField] private float _brazierLightDelay = 1f;
    [SerializeField] private ParticleSystem[] _brazierIgniteOrder;

    [Header("Audio")]
    [SerializeField] private float _audioFadeTime;
    [SerializeField] private AudioSource _bgAudioSource;
    [SerializeField] private AudioSource _victoryAudioSource;
    [SerializeField] private AudioSource _brazierIgnition;
    [SerializeField] private AudioSource _fireCrackle;

    public void StartVictorySequence()
    {
        AudioFade audioFadeIn = new AudioFade();
        AudioFade audioFadeOut = new AudioFade();

        StartCoroutine(audioFadeOut.FadeAudio(_bgAudioSource, _audioFadeTime, 0f));
        _victoryAudioSource.Play();
        StartCoroutine(audioFadeIn.FadeAudio(_victoryAudioSource, _audioFadeTime, .5f));
        StartCoroutine(LightBraziers());
    }

    private IEnumerator LightBraziers()
    {
        foreach(ParticleSystem fire in _brazierIgniteOrder)
        {
            fire.Play();
            _brazierIgnition.Play();
            yield return new WaitForSeconds(_brazierLightDelay);
        }
        _fireCrackle.Play();
    }
}

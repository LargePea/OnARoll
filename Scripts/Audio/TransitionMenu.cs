using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _mainMenuMusic;
    [SerializeField] private AudioSource _backgroundMusic;
    public void Transition()
    {
        AudioFade fadeOut = new AudioFade();
        AudioFade fadeIn = new AudioFade();

        _backgroundMusic.Play();
        StartCoroutine(fadeOut.FadeAudio(_mainMenuMusic, 0.5f, 0f));
        StartCoroutine(fadeIn.FadeAudio(_backgroundMusic, 0.5f, 1f));
        _mainMenuMusic.Stop();
    }
}

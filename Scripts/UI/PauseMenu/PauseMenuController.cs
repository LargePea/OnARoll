using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private AudioSource _menuMusic;

    private PlayerState _playerState;
    Coroutine _audioFadeCo;
    private float _lastTimeScale;
    private void Awake()
    {
        _lastTimeScale = 1f;
        _playerState = GetComponent<PlayerState>();

    }

    public void ActivatePauseMenu()
    {
        _lastTimeScale = Time.timeScale;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Pause();
        }
        _menuMusic.Play();
        _menuMusic.volume = 1f;
        _menuMusic.pitch = 0.8f;
        PauseGame();
    }

    public void DeactivatePauseMenu()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        _menuMusic.Stop();
        _menuMusic.volume = 0f;
        _menuMusic.pitch = 1f;
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.UnPause();
        }
        UnpauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        Debug.Log(_lastTimeScale);
        Time.timeScale = _lastTimeScale;
    }
}

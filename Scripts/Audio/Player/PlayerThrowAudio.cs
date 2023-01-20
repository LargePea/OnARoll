using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] AudioSource _playerThrowAudio;

    public void PlaySound()
    {
        int RandomSound = Random.Range(0, _audioClips.Length);

        _playerThrowAudio.clip = _audioClips[RandomSound];
        _playerThrowAudio.Play();
    }
}

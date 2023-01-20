using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] _damageAudioClips;
    [SerializeField] AudioSource _screenCrackAudio;
    [SerializeField] AudioSource _playerDamageAudio;

    //crack pitching
    [Header("Crack Pitch")]
    [SerializeField] Vector2 _randomCrackPitch = Vector2.one;

    public void PlaySound(bool playCrack = true)
    {
        int randomSound = Random.Range(0, _damageAudioClips.Length);
        _playerDamageAudio.clip = _damageAudioClips[randomSound];
        _playerDamageAudio.Play();

        if (playCrack)
        {
            _screenCrackAudio.pitch = Random.Range(_randomCrackPitch.x, _randomCrackPitch.y);
            _screenCrackAudio.Play();
            
        }
    }
}

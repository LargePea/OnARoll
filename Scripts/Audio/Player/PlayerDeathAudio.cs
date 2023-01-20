using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _deathAudio;

    public IEnumerator PlaySound()
    {
        int randomSound = Random.Range(0, _audioClips.Length);

        _deathAudio.clip = _audioClips[randomSound];

        _deathAudio.Play();
        yield return new WaitForSeconds(_deathAudio.clip.length);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _walkingSource;
    [SerializeField] private Vector2 _randomPitch = Vector2.one;
    [SerializeField] private Vector2 _randomVolume = Vector2.one;
    [SerializeField] private ParticleSystem _walkParticle;

    public void AnimEventFootstep()
    {
        _walkParticle.Play();
        _walkingSource.pitch = Random.Range(_randomPitch.x, _randomPitch.y);
        _walkingSource.volume = Random.Range(_randomVolume.x, _randomVolume.y);

        _walkingSource.Play();
    }
}

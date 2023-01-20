using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BoulderRollingAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _boulderRollAudio;
    [SerializeField] float _boulderSpeedToPitch = 1;
    [SerializeField, Range(0, 3)] float _boulderMinPitch = 0;
    [SerializeField, Range(0, 3)] float _boulderMaxPitch = 3;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }


    private void Update()
    {
        float pitch = (float) Math.Round( Mathf.Clamp(_rigidbody2D.velocity.magnitude / _boulderSpeedToPitch, _boulderMinPitch, _boulderMaxPitch), 1);
        _boulderRollAudio.pitch = pitch;
    }

    public bool PlaySound()
    {
        _boulderRollAudio.Play();
        return true;
    }

    public bool StopSound()
    {
        _boulderRollAudio.Stop();
        return false;
    }
}

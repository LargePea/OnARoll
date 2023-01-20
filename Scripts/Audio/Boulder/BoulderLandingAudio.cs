using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderLandingAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _boulderLand;

    public void PlaySound()
    {
        _boulderLand.Play();
    }
}

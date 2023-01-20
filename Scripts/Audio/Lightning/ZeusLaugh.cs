using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusLaugh : MonoBehaviour
{
    [SerializeField] private AudioSource _zeusLaughAudio;
    [SerializeField] private AudioClip[] _laughs;
    public void PlaySound()
    {
        int randomLaugh = Random.Range(0, _laughs.Length);

        _zeusLaughAudio.clip = _laughs[randomLaugh];    
        _zeusLaughAudio.Play();
    }
}

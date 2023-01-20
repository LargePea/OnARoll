using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusTaunt : MonoBehaviour
{
    [SerializeField] private AudioSource _tauntAudio;
    [SerializeField] private AudioClip[] _tauntTracks;

    public IEnumerator PlaySound()
    {
        int randomClip = Random.Range(0, _tauntTracks.Length);

        _tauntAudio.clip = _tauntTracks[randomClip];
        _tauntAudio.Play();
        yield return new WaitForSeconds(_tauntAudio.clip.length);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade
{
    float currentTime = 0;
    public IEnumerator FadeAudio(AudioSource audio, float duration, float targetVolume)
    {
        float startingVolume = audio.volume;
        while (currentTime <= duration)
        {
            currentTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(startingVolume, targetVolume, currentTime / duration);

            yield return null;
        }

        currentTime = 0;
    }
}

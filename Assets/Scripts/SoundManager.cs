using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float masterVolume;

    private static SoundManager instance;

    private List<AudioSource> audioSources = new List<AudioSource>();

    static public SoundManager Get()
    {
        return instance;
    }

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public AudioSource Play(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        foreach (var audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                Play(audioSource, clip, volume, pitch, loop);
                return audioSource;
            }
        }

        GameObject newAudioSourceObject = new GameObject("AudioSource");
        newAudioSourceObject.transform.parent = transform;

        AudioSource newAudioSource = newAudioSourceObject.AddComponent<AudioSource>();
        audioSources.Add(newAudioSource);

        Play(newAudioSource, clip, volume, pitch, loop);

        return newAudioSource;
    }

    public void FadeOut(AudioClip clip)
    {
        foreach (var audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                if (audioSource.clip == clip)
                {
                    StartCoroutine(FadeOutCR(audioSource));
                }
            }
        }
    }

    public void FadeIn(AudioClip clip, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        var audioSource = Play(clip, 0.0f, pitch, loop);

        StartCoroutine(FadeInCR(audioSource, volume));
    }

    IEnumerator FadeOutCR(AudioSource audioSource)
    {
        while (audioSource.volume > 0.0f)
        {
            yield return null;
            audioSource.volume = audioSource.volume - Time.deltaTime;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

    IEnumerator FadeInCR(AudioSource audioSource, float targetVolume)
    {
        while (audioSource.volume < targetVolume)
        {
            yield return null;
            audioSource.volume = audioSource.volume + Time.deltaTime;
        }

        audioSource.volume = targetVolume;
    }

    void Play(AudioSource audioSource, AudioClip clip, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume * masterVolume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.Play();
    }
}

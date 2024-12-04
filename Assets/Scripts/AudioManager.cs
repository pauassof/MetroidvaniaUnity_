using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource[] musicSource;
    private AudioSource ambientSource;
    private AudioSource[] sfxSource;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource = new AudioSource[2];
        for (int i = 0; i < musicSource.Length; i++)
        {
            musicSource[i] = gameObject.AddComponent<AudioSource>();
            musicSource[i].playOnAwake = false;
            musicSource[i].loop = true;
        }
        
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.playOnAwake = false;
        ambientSource.loop = true;
        sfxSource = new AudioSource[4];
        for (int i = 0; i < sfxSource.Length; i++)
        {
            sfxSource[i] = gameObject.AddComponent<AudioSource>();
            sfxSource[i].playOnAwake = false;
        }
    }

    public void PlayMusic(AudioClip song, float volume)
    {
        for (int i = 0;i < musicSource.Length; i++) 
        {
            if (musicSource[i].isPlaying)
            {
                musicSource[i].clip = song;
                musicSource[i].volume = volume;
                musicSource[i].Play();
                return;
            }
        }
        musicSource[0].clip = song;
        musicSource[0].volume = volume;
        musicSource[0].Play();

    }

    public void PlayMusicFade(AudioClip song, float volume, float fadeDuration)
    {
        AudioSource init = null;
        AudioSource next = null;
        for (int i = 0;i < musicSource.Length; i++)
        {
            if (musicSource[i].isPlaying)
            {
                init = musicSource[i];

            }
            else
            {
                musicSource[i].volume = volume;
                next = musicSource[i];
            }
        }
        next.clip = song;
        next.Play();
        StartCoroutine(FadeMusic(init, next, fadeDuration));
    }

    IEnumerator FadeMusic(AudioSource initSource, AudioSource nextSource, float fadeDuration)
    {
        
        float t = 0;
        float volumeDecrease = t / fadeDuration;
        float initVolume = initSource.volume;
        float nextVolume = nextSource.volume;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            initSource.volume = initVolume * (1 - volumeDecrease);
            nextSource.volume = nextVolume * volumeDecrease;
            volumeDecrease = t / fadeDuration;
            yield return null;
        }
        initSource.Stop();
    }

    public void PlayAmbientSound(AudioClip ambient)
    {
        ambientSource.clip = ambient;
        ambientSource.Play();
    }

    public void PlaySFX(AudioClip sFX, float volume)
    {
        for (int i = 0; i<sfxSource.Length; i++)
        {
            if (sfxSource[i].isPlaying == false)
            {
                sfxSource[i].clip = sFX;
                sfxSource[i].volume = volume;
                sfxSource[i].Play();
                break;
            }
        }
        
    }
}

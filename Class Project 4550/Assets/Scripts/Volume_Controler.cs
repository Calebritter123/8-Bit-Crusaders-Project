using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeControler : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sfxSlider;
    public AudioSource bgmSource;
    public List<AudioSource> sfxSources = new List<AudioSource>();

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;     // for Level1
    public AudioClip level2Music;   // for Level2
    public AudioClip bossMusic;     // for BossArena

    private AudioSource sfxSource;

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (sfxSources.Count > 0)
        {
            sfxSource = sfxSources[0];
            sfxSlider.value = sfxSource.volume;
        }

        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);

        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume");
            volumeSlider.value = bgmSource.volume;
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            foreach (AudioSource sfx in sfxSources)
            {
                sfx.volume = SFXVolume;
            }
            sfxSlider.value = SFXVolume;
        }
    }

    public void ChangeVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void ChangeSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        foreach (AudioSource sfx in sfxSources)
        {
            sfx.volume = volume;
        }
    }

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxSources.Count)
        {
            sfxSources[index].Play();
        }
        else
        {
            Debug.LogWarning("Invalid SFX index: " + index);
        }
    }

    public void StopBacgroundMusic()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (bgmSource == null) return;

        AudioClip clipToPlay = null;

        switch (scene.name)
        {
            case "MenuPrototype":
                clipToPlay = menuMusic;
                break;
            case "Level1":
                clipToPlay = gameMusic;
                break;
            case "Level2":
                clipToPlay = level2Music;
                break;
            case "BossArena":
                clipToPlay = bossMusic;
                break;
        }

        if (clipToPlay != null && bgmSource.clip != clipToPlay)
        {
            bgmSource.clip = clipToPlay;
            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
            bgmSource.Play();
        }
    }
}

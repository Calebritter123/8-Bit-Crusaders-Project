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

    public AudioClip gameMusic;
    public AudioClip menuMusic;

    private AudioSource sfxSource; // Single reference for an SFX source

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        
        if (sfxSources.Count > 0)
        {
            sfxSource = sfxSources[0]; // Assign first SFX source
            sfxSlider.value = sfxSource.volume;
        }


        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);

        Debug.Log("Initial volume: " + bgmSource.volume);
        if (sfxSource != null) Debug.Log("Initial SFX volume: " + sfxSource.volume);

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
        Debug.Log("Volume Changed to: " + volume);
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void ChangeSFXVolume(float volume)
    {
        Debug.Log("SFX Volume Changed to: " + volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);

        foreach (AudioSource sfx in sfxSources)
        {
            sfx.volume = volume;
        }
    }

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxSources.Count) // Ensure index is valid
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
        if (bgmSource != null){
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

    if (scene.name == "MenuPrototype")
    {
        if (bgmSource.clip != menuMusic)
        {
            bgmSource.clip = menuMusic;
            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
            bgmSource.Play();
        }
    }
    else if (scene.name == "Level1")
    {
        if (bgmSource.clip != gameMusic) // gameMusic must be defined above
        {
            bgmSource.clip = gameMusic;
            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
            bgmSource.Play();
        }
    }
}

}
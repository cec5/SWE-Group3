using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;

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
    }

    private void Update()
    {
        // Apply saved volumes on startup
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume",0.75f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume",0.75f));
        
    }

    // Music control
    public void SetMusicVolume(float volume)
    {
        if (musicSource == null)
        {
            musicSource = GameObject.FindWithTag("Music")?.GetComponent<AudioSource>();
        }

        if (musicSource != null)
        {
            musicSource.volume = volume;
        }
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public float GetMusicVolume() => PlayerPrefs.GetFloat("MusicVolume", 0.75f);

    // sfx control
    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public float GetSFXVolume() => PlayerPrefs.GetFloat("SFXVolume", 0.75f);

    

    public void PlaySFX(AudioClip audioClip, float volume = 1f)
    {
        StartCoroutine(PlaySFXCoroutine(audioClip, volume));
    }

    IEnumerator PlaySFXCoroutine(AudioClip audioClip, float volume = 1f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume * GetSFXVolume(); 
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length * 2);

        Destroy(audioSource);
    }
}
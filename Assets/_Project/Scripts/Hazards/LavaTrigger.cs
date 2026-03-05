using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RisingLava lavaObject;

    private AudioSource audioSource;
    public AudioClip earthquakeClip;
    private bool audioPlayed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (lavaObject != null)
            {
                lavaObject.StartRising();
                if (!audioPlayed) 
                {
                    audioPlayed = true;
                    PlaySFX(earthquakeClip, 0.1f);
                }
            }
            else
            {
                Debug.LogWarning("LavaTrigger.cs requires an lavaObject reference!");
            }
        }
    }

    public void PlaySFX(AudioClip audioClip, float volume = 0.1f)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
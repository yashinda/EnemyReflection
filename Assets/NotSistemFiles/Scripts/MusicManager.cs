using System;
using UnityEngine;

public enum MusicType
{
    Extraction,
    Chase,
    Failed
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    [Header("Music Library")]
    public AudioClip clipExtraction;
    public AudioClip clipChase;
    public AudioClip clipFailed;
    
    [Header("AudioSource")]
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayMusic(MusicType.Extraction);
    }

    public void PlayMusic(MusicType musicType)
    {
        switch (musicType)
        {
            case MusicType.Extraction:
                audioSource.clip = clipExtraction;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case MusicType.Chase:
                audioSource.clip = clipChase;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case MusicType.Failed:
                audioSource.clip = clipFailed;
                audioSource.loop = false;
                audioSource.Play();
                break;
        }
    }
}

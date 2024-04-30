using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public float volume = 0.5f;

    public AudioClip[] music;
    [SerializeField]
    private AudioSource audioSource;
    private AudioClip musicClip;
    private int musicIndex;

    void Start()
    {

        audioSource.volume = volume;
        musicIndex = Random.Range(0, music.Length);
        musicClip = music[musicIndex];
        audioSource.clip = musicClip;
        audioSource.Play();
    }
}

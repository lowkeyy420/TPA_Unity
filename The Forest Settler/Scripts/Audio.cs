using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Audio
{
    [HideInInspector]
    public AudioSource source;

    public AudioClip audio;

    public string audioName;

    [Range(0f, 1f)]
    public float volume;

    public bool loopAudio;
}
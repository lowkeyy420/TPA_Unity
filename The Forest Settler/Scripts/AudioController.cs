using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public Audio[] sounds;


    private float timer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;


        foreach (Audio sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audio;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loopAudio;
            sound.audioName = sound.source.clip.name;
        }
    }

    private void Update()
    {
        
        if (PlayerStatusController.instance.attack) {
            timer += Time.deltaTime;
        }
        if (timer >= 0.5f)
        {
            PlayerStatusController.instance.attack = false;
            timer = 0;
        }

    }


    public void playAudio(string audioName)
    {
        Audio sound = Array.Find(sounds, sound => sound.audioName == audioName);
        if (sound == null) return;
        sound.source.Play();
    }

    public void startBGM() => playAudio("gameBGM");
    public void startBGM2() => playAudio("BGM");
    public void mainMenu() => playAudio("mainmenu");
    public void hit1() => playAudio("hit 1");
    public void hit2() => playAudio("hit 2");

    public void swordHit1()
    {
        playAudio("sword-swoosh");
        PlayerStatusController.instance.attack = true;

    }
    public void swordHit2()
    {
        playAudio("sword hit");
        PlayerStatusController.instance.attack = true;
    }
    public void punch1()
    {
        playAudio("punch fx 1");
        PlayerStatusController.instance.attack = true;
    }
    public void punch2()
    {
        playAudio("punch 2");
        PlayerStatusController.instance.attack = true;
    }
    public void punch3()
    {
        playAudio("punch fx 3");
        PlayerStatusController.instance.attack = true;
    }


}

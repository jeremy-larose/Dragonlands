using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource soundEffect;

    public event EventHandler<OnChangeBackgroundMusicEventArgs> OnChangeBackgroundMusic;
    public event EventHandler<OnPlaySoundEffectEventArgs> OnPlaySoundEffect;

    public class OnChangeBackgroundMusicEventArgs : EventArgs
    {
        public AudioClip bgMusic;
    }

    public class OnPlaySoundEffectEventArgs : EventArgs
    {
        public AudioClip soundEffect;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad( gameObject );
    }

    public void PlayMusic(AudioClip audioClip)
    {
        StopBGM();
        backgroundMusic.clip = audioClip;
        backgroundMusic.Play();
    }

    public void PlaySound(AudioClip sound)
    {
        soundEffect.clip = sound;
        soundEffect.PlayOneShot(sound);
    }

    private void StopBGM()
    {
        backgroundMusic.Stop();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log( "Changing Background Music.");
            OnChangeBackgroundMusic?.Invoke( this, new OnChangeBackgroundMusicEventArgs{ bgMusic = GameAssets.i.townMusic });
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log( "Playing sound effect!");
            OnPlaySoundEffect?.Invoke(this, new OnPlaySoundEffectEventArgs{ soundEffect = GameAssets.i.fireCrackle });
        }
    }
}

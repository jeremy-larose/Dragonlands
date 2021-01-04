using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioSource voiceLayer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Changing Background Music.");
            OnChangeBackgroundMusic?.Invoke(this, new OnChangeBackgroundMusicEventArgs {bgMusic = GameAssets.i.townMusic});
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Playing sound effect!");
            OnPlaySoundEffect?.Invoke(this, new OnPlaySoundEffectEventArgs {soundEffect = GameAssets.i.fireCrackle});
        }
    }

    public event EventHandler<OnChangeBackgroundMusicEventArgs> OnChangeBackgroundMusic;
    public event EventHandler<OnPlaySoundEffectEventArgs> OnPlaySoundEffect;

    public void PlayMusic(AudioClip audioClip)
    {
        if (backgroundMusic != null)
            StopBGM();

        backgroundMusic.clip = audioClip;
        backgroundMusic.Play();
    }

    public void PlaySound(AudioClip sound)
    {
        soundEffect.clip = sound;
        soundEffect.PlayOneShot(sound);
    }

    public void PlayVoice(AudioClip sound)
    {
        voiceLayer.clip = sound;
        voiceLayer.PlayOneShot(sound);
    }

    private void StopBGM()
    {
        backgroundMusic.Stop();
    }

    public class OnChangeBackgroundMusicEventArgs : EventArgs
    {
        public AudioClip bgMusic;
    }

    public class OnPlaySoundEffectEventArgs : EventArgs
    {
        public AudioClip soundEffect;
    }
}
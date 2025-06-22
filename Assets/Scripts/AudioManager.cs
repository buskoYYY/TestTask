using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioClip _defaultMusic;
    [SerializeField] private AudioClip _engineSound;
    [SerializeField] private AudioClip _dangerSound;

    private void Awake()
    {
        PlayMusic(_defaultMusic);
        _musicSource.loop = true;

        _soundSource.playOnAwake = false;
        _soundSource.loop = false;
    }

    public void PlayEngineSound()
    {
        _soundSource.PlayOneShot(_engineSound);
    }

    public void PlayDadngerSound()
    {
        if (!_soundSource.isPlaying)
        {
            _soundSource.PlayOneShot(_dangerSound);
        }
    }

    public void StopDangerSound()
    {
        _soundSource.Stop();
    }

    private void PlayMusic(AudioClip clip)
    {
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip lg_resScreenClip;
    [SerializeField] private AudioClip mapBossClip;
    [SerializeField] private AudioClip normalMapClip;
    [SerializeField] private AudioClip clickClip;

    public static SoundManager instance;

    private string setOffline = Api.Instance.api + Api.Instance.routerSetOffline;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(instance.gameObject);
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SetMusicData(PlayerPrefs.GetFloat("MusicKey",1) == 1 ? true : false);
        SetSoundData(PlayerPrefs.GetFloat("SoundKey",1) == 1 ? true : false);
    }
    public void SetSoundClick()
    {
        soundSource.PlayOneShot(clickClip);
    }

    public void SetLg_ResMusic()
    {
        if (musicSource.clip == lg_resScreenClip)
            return;
        musicSource.clip = lg_resScreenClip;
        musicSource.Play();
    }

    public void SetMapBossMusic()
    {
        musicSource.clip = mapBossClip;
        musicSource.Play();
    }

    public void SetNormalMapMusic()
    {
        musicSource.clip = normalMapClip;
        musicSource.Play();
    }

    public void SetMusicData(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetFloat("MusicKey", 1);
            Debug.Log("dd");
            musicSource.Play();
        }
        else
        {
            PlayerPrefs.SetFloat("MusicKey", 0);
            musicSource.Pause();
        }
    }

    public void SetSoundData(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetFloat("SoundKey", 1);
            soundSource.mute = true;
        }
        else
        {
            PlayerPrefs.SetFloat("SoundKey", 0);
            soundSource.mute = false;
        }
    }
}

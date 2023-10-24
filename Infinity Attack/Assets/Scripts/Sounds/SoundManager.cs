using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip lg_resScreenClip;
    [SerializeField] private AudioClip mapBossClip;
    [SerializeField] private AudioClip normalMapClip;
    [SerializeField] private AudioClip clickClip_1;
    [SerializeField] private AudioClip clickClip_2;

    public static SoundManager instance;

    private AudioClip curClip;
    private string setOffline = Api.Instance.api + Api.Instance.routerSetOffline;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SetMusicData(PlayerPrefs.GetFloat("MusicKey", 1) == 1 ? true : false);
        SetSoundData(PlayerPrefs.GetFloat("SoundKey", 1) == 1 ? true : false);
    }

    private void Update()
    {
        //PlayerPrefs.SetFloat("MusicKey", 1);
        //PlayerPrefs.SetFloat("SoundKey", 1);
    }
    public void SetSoundClick()
    {
        //if(curClip == null)
        //{
        //    curClip = clickClip_1;
        //}
        //else
        //{
        //    curClip = curClip == clickClip_1 ? clickClip_2 : clickClip_1;
        //}
        soundSource.PlayOneShot(clickClip_1);
    }

    public void SetLg_ResMusic()
    {
        if (musicSource.clip == lg_resScreenClip)
            return;
        musicSource.clip = lg_resScreenClip;
        if (PlayerPrefs.GetFloat("MusicKey", 1) == 1)
        {
            musicSource.Play();
        }
    }

    public void SetMapBossMusic()
    {
        musicSource.clip = mapBossClip;
        if (PlayerPrefs.GetFloat("MusicKey", 1) == 1)
        {
            musicSource.Play();
        }
    }

    public void SetNormalMapMusic()
    {
        musicSource.clip = normalMapClip;
        if (PlayerPrefs.GetFloat("MusicKey", 1) == 1)
        {
            musicSource.Play();
        }
    }

    public void SetMusicData(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetFloat("MusicKey", 1);
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
            soundSource.mute = false;
        }
        else
        {
            PlayerPrefs.SetFloat("SoundKey", 0);
            soundSource.mute = true;
        }
    }
}

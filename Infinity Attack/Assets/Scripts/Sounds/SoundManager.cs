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

    public void SetSoundClick()
    {
        soundSource.PlayOneShot(clickClip);
    }

    public void SetLg_ResMusic()
    {
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
}

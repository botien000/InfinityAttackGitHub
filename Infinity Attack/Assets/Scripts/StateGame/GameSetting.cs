using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    [SerializeField] Button btnHome, btnResume, btnExit, btnMusic, btnSound;
    [SerializeField] private Image imgMusicOn_Off, imgSoundOn_Off;

    private void Start()
    {
        btnHome.onClick.AddListener(() => PressBtnHome());
        btnResume.onClick.AddListener(() => PressBtnResume());
        btnMusic.onClick.AddListener(() => PressBtnMusic());
        btnSound.onClick.AddListener(() => PressBtnSound());
        btnExit.onClick.AddListener(() => PressBtnExit());

        imgMusicOn_Off.gameObject.SetActive(PlayerPrefs.GetFloat("MusicKey", 1) == 1 ? false : true);
        imgSoundOn_Off.gameObject.SetActive(PlayerPrefs.GetFloat("SoundKey", 1) == 1 ? false : true);
    }

    private void PressBtnExit()
    {
        GameManager.instance.SetStateGame(GameManager.StateGame.GamePlay);
    }

    private void PressBtnResume()
    {
        GameManager.instance.SetStateGame(GameManager.StateGame.GamePlay);
    }

    private void PressBtnHome()
    {
        GameManager.instance.RemoveAllDontDestroyInGame();
        SceneManager.LoadScene("Home");
    }

    public void PressBtnMusic()
    {
        bool isActive = !imgMusicOn_Off.gameObject.activeInHierarchy;
        imgMusicOn_Off.gameObject.SetActive(isActive);
        SoundManager.instance.SetMusicData(!isActive);
    }

    public void PressBtnSound()
    {
        bool isActive = !imgSoundOn_Off.gameObject.activeInHierarchy;

        imgSoundOn_Off.gameObject.SetActive(isActive);
        SoundManager.instance.SetSoundData(!isActive);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }


}

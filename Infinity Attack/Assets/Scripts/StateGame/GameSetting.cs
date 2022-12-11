using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    [SerializeField] Button btnHome, btnResume, btnRestart,btnExit;
    [SerializeField] Slider sldMusic, sldAudio;

    private void Start()
    {
        btnHome.onClick.AddListener(() => PressBtnHome());
        btnResume.onClick.AddListener(() => PressBtnResume());
        btnRestart.onClick.AddListener(() => PressBtnRestart());
        btnExit.onClick.AddListener(() => PressBtnExit());
    }

    private void PressBtnExit()
    {
        GameManager.instance.SetStateGame(GameManager.StateGame.GamePlay);
    }

    private void PressBtnRestart()
    {
        GameManager.instance.RestartGame();
    }

    private void PressBtnResume()
    {
        GameManager.instance.SetStateGame(GameManager.StateGame.GamePlay);
    }

    private void PressBtnHome()
    {
        SceneManager.LoadScene("Home");
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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Camera oldCamera;
    private CharacterObject oldPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public enum StateGame
    {
        GamePlay, GameSetting, GameOver
    }
    [SerializeField] private StateGame curState;

    [SerializeField] private GamePlay gamePlay;
    [SerializeField] private GameSetting gameSetting;
    [SerializeField] private GameOver gameOver;

    public int typeMap;
    [SerializeField] private int numberOfEnemy;
    [SerializeField] private int goldPerEnemy;
    [SerializeField] private int gemKillBoss;

    private int curTotalGold, curTotalGem;
    private int amountKilledEnemy;
    private int amountKilledBoss;

    public void SetStateGame(StateGame state)
    {
        switch (state)
        {
            case StateGame.GamePlay:

                gameSetting.gameObject.SetActive(false);
                break;
            case StateGame.GameSetting:
                gameSetting.gameObject.SetActive(true);
                break;
            case StateGame.GameOver:
                gameOver.gameObject.SetActive(true);
                break;
        }
    }

    public void BtnSetting()
    {
        SetStateGame(StateGame.GameSetting);
    }

    public void RestartGame()
    {
        if (oldPlayer != null)
        {
            Destroy(oldPlayer.gameObject);
        }
        if (oldCamera != null)
        {
            Destroy(oldCamera.gameObject);
        }
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetPlayerDontDestroy(CharacterObject player)
    {
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Camera.main);
        oldPlayer = player;
        oldCamera = Camera.main;
    }
}
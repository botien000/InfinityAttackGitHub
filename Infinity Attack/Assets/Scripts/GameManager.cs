using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private List<Camera> listCamera;
    private Camera oldCamera;
    private CharacterObject oldPlayer;

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

    private void Start()
    {
        if(SoundManager.instance != null)
        SoundManager.instance.SetNormalMapMusic();
    }
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
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetPlayerDontDestroy(CharacterObject player)
    {
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(this);
        listCamera = FindObjectsOfType<Camera>().ToList();
        if(listCamera.Count > 2)
        {
            foreach (var camera in listCamera)
            {
                if(camera.gameObject.name == "Main Camera")
                {
                    DestroyImmediate(camera.gameObject);
                    listCamera.Remove(camera);
                    break;
                }
            }
        }
        oldPlayer = player;
        oldCamera = Camera.main;
        DontDestroyOnLoad(Camera.main);
    }

    public void RemoveAllDontDestroyInGame()
    {
        Destroy(oldPlayer.gameObject);
        Destroy(oldCamera.gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// Count enemy is killed
    /// </summary>
    /// <param name="count"></param>
    public void CountKilledEnemy(int count)
    {
        amountKilledEnemy += count;
        curTotalGold = amountKilledEnemy * goldPerEnemy;
    }

    /// <summary>
    /// Count boss is killed
    /// </summary>
    /// <param name="count"></param>
    public void CountKilledBoss(int count)
    {
        amountKilledBoss += count;
        curTotalGem = amountKilledBoss * gemKillBoss;
    }

    /// <summary>
    /// Get total gem
    /// </summary>
    /// <returns></returns>
    public int GetTotalGem()
    {
        return curTotalGem;
    }

    /// <summary>
    /// Get total gold
    /// </summary>
    /// <returns></returns>
    public int GetTotalGold()
    {
        return curTotalGold;
    }

    /// <summary>
    /// Get amount enemy is killed
    /// </summary>
    /// <returns></returns>
    public int GetKilledEnemy()
    {
        return amountKilledEnemy;
    }

    /// <summary>
    /// Get amount boss is killed
    /// </summary>
    /// <returns></returns>
    public int GetKilledBoss()
    {
        return amountKilledBoss;
    }

}
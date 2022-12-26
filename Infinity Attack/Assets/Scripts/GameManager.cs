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
    public Camera oldMinimap;
    public GameObject EventSystem;
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
                if (SystemData.instance.map == 3)
                {
                    if (SystemData.instance.amountBossMap_3 == 0)
                    {
                        gameOver.gameObject.SetActive(true);
                    }
                }
                else
                {
                    gameOver.gameObject.SetActive(true);
                }
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
        DontDestroyOnLoad(oldMinimap);
        DontDestroyOnLoad(EventSystem);
    }

    public void RemoveAllDontDestroyInGame()
    {
        Destroy(oldPlayer.gameObject);
        Destroy(oldCamera.gameObject);
        Destroy(oldMinimap.gameObject);
        Destroy(EventSystem.gameObject);
        Destroy(gameObject);
    }
}
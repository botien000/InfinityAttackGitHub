using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private List<Camera> listCamera;
    private Camera oldCamera;
    public Camera oldMinimap;
    public GameObject EventSystem;
    private CharacterObject oldPlayer;
    private string curCharName;
    private bool isBossMap;

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

    [SerializeField] private ConversationUI conversationUI;
    [SerializeField] private GamePlay gamePlay;
    [SerializeField] private GameSetting gameSetting;
    [SerializeField] private GameOver gameOver;

    public int typeMap;

    private float amountBossInMap;

    public bool IsBossMap { get => isBossMap; set => isBossMap = value; }

    private void Start()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.SetNormalMapMusic();

        if (typeMap == 3)
            amountBossInMap = 2;
        else
            amountBossInMap = 1;
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

    public void SetPlayerDontDestroy(CharacterObject player)
    {
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(this);
        listCamera = FindObjectsOfType<Camera>().ToList();
        if (listCamera.Count > 2)
        {
            foreach (var camera in listCamera)
            {
                if (camera.gameObject.name == "Main Camera")
                {
                    Destroy(camera.gameObject);
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
        SpellSingleton.Instance.SetAllSpellNull();
        Destroy(oldPlayer.gameObject);
        Destroy(oldCamera.gameObject);
        Destroy(oldMinimap.gameObject);
        Destroy(EventSystem.gameObject);
        Destroy(gameObject);
    }

    public void CheckBossDie()
    {
        amountBossInMap--;
        SystemData.instance.flagBoss += 1;
        if (amountBossInMap == 0)
        {
            // Game over
            SetStateGame(StateGame.GameOver);
        }
    }

    internal void SetImageToPlayer(Image img)
    {
        oldPlayer.SetImage(img);
    }

    internal void SetNameCharacter(string charUsingName)
    {
        curCharName = charUsingName;
        SentInfoToConver();
    }

    internal void CheckBossMap(bool checkMap)
    {
        isBossMap = checkMap;
    }

    public void SentInfoToConver()
    {
        conversationUI.SetSptCharacter(curCharName);
        conversationUI.SetBossMap(isBossMap);
        conversationUI.SetActiveConversation(true);
    }

    public bool GetActiveConversation()
    {
        return conversationUI.gameObject.activeSelf;
    }

    public void SetPlayerFreeze(bool isFreeze)
    {
        oldPlayer.SetFreeze(isFreeze);
    }

    public void SetPlayerActive(bool isActive)
    {
        oldPlayer.gameObject.SetActive(isActive);
    }

    public CharacterObject GetPlayer()
    {
        return oldPlayer;
    }
}
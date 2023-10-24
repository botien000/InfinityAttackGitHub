using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtLogKilledEnemy, txtLogKilledBoss;
    [SerializeField] TextMeshProUGUI txtPlusGem, txtPlusGold;
    [SerializeField] TextMeshProUGUI txtStateOver;
    [SerializeField] Button btnHome;

    private void Start()
    {
        SystemData.instance.updateAchieved();
        PlayerPrefs.SetInt("Play", 1);
        SetPlus();
        SetLogKilledEnemy();
        SetLogKilledBoss();
        OnShow();
        btnHome.onClick.AddListener(() => PressBtnHome());
    }

    private void PressBtnHome()
    {
        GameManager.instance.RemoveAllDontDestroyInGame();
        SceneManager.LoadScene("Home");
    }

    public void SetPlus()
    {
        txtPlusGold.text = SystemData.instance.curTotalGold + "";
        txtPlusGem.text = SystemData.instance.curTotalGem + "";
    }

    public void SetLogKilledEnemy()
    {
        txtLogKilledEnemy.text = "Number of enemies was killed: " + SystemData.instance.flagEnemy + "/" + SystemData.instance.totalEnemy;
    }

    public void SetLogKilledBoss()
    {
        txtLogKilledBoss.text = "Number of bosses was killed: " + SystemData.instance.flagBoss;
    }

    public void OnShow()
    {
        if (SystemData.instance.map == 3)
        {
            if (SystemData.instance.amountBossMap_3 == 2)
            {
                txtStateOver.text = "VICTORY";

            }
            else
            {
                txtStateOver.text = "DEFEAT";
            }
            return;
        }
        if (SystemData.instance.flagBoss == 1)
        {
            txtStateOver.text = "VICTORY";
        }
        else
        {
            txtStateOver.text = "DEFEAT";
        }
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

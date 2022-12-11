using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtLogKilledEnemy, txtLogKilledBoss;
    [SerializeField] TextMeshProUGUI txtPlusGem,txtPlusGold;
    [SerializeField] TextMeshProUGUI txtStateOver;
    [SerializeField] Button btnHome;

    private void Start()
    {
        btnHome.onClick.AddListener(() => PressBtnHome());
    }

    private void PressBtnHome()
    {
        SystemData.instance.updateAchieved();
        SceneManager.LoadScene("Home");
    }

    public void SetTextLog(string enemy,string boss)
    {
        txtLogKilledEnemy.text = enemy;
        txtLogKilledBoss.text = boss;
    }

    public void SetPlus(string gold,string gem)
    {
        txtPlusGold.text = gold;
        txtPlusGem.text = gem;
    }
}

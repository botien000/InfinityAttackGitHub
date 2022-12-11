using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class SpellShow : MonoBehaviour
{
    [SerializeField] private Spell[] spellOwnList;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private int sceneNumber;
    private Api instanceIP;
    private SpellSingleton spellSingleton;
    private Sprite fireSprite;
    private Sprite chaosSprite;
    private Sprite speedUpSprite;
    private Sprite healingSprite;
    private Sprite utilmateRemakeSprite;
    private string path;
    private int indexArr;
    private bool choosenFire = false, choosenChaos = false, choosenHealing = false, choosenSpeed = false, choosenUlti = false;
    void Start()
    {
        instanceIP = Api.Instance;
        spellSingleton = SpellSingleton.Instance;
        LoadAvatars();
        if (PlayerPrefs.HasKey("UID"))
        {
            string userID = removeQuotes(PlayerPrefs.GetString("UID"));
            StartCoroutine(GetSpellOwnData(instanceIP.api + instanceIP.routerPostSpellsOwn, userID));
        }
    }

    IEnumerator GetSpellOwnData(string address, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        UnityWebRequest www = UnityWebRequest.Post(address, form);
        loadingPanel.SetActive(true);
        yield return www.SendWebRequest();
        loadingPanel.SetActive(false);
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            string res = www.downloadHandler.text;
            getSpellOwn(res);
            www.Dispose();
        }
    }

    void getSpellOwn(string rawResponse)
    {
        var _spellOwn = JsonConvert.DeserializeObject<Spell[]>(rawResponse);
        spellOwnList = _spellOwn;

        for(int i = 0; i < 3; i++)
        {
            if(spellSingleton.GetSpell(i).name == "Fire")
            {
                choosenFire = true;
            }
            else if(spellSingleton.GetSpell(i).name == "Chaos")
            {
                choosenChaos = true;
            }
            else if (spellSingleton.GetSpell(i).name == "Healing")
            {
                choosenHealing = true;
            }
            else if (spellSingleton.GetSpell(i).name == "SpeedUp")
            {
                choosenSpeed = true;
            }
            else if (spellSingleton.GetSpell(i).name == "UtilmateRemake")
            {
                choosenUlti = true;
            }
        }
        GameObject g;
        GameObject item = transform.GetChild(0).gameObject;
        for (int i = 0; i < spellOwnList.Length; i++)
        {
            g = Instantiate(item, transform);
            string name = spellOwnList[i].spellID.name;
            int amount = spellOwnList[i].amount;
            if(amount <= 0)
            {
                g.SetActive(false);
            }
            else
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amount + "/10";
                if (name == "Chaos" && !choosenChaos)
                {
                    g.transform.GetChild(0).GetComponent<Image>().sprite = chaosSprite;
                }
                else if (name == "Chaos" && choosenChaos)
                {
                    g.SetActive(false);
                }
                else if (name == "Fire" && !choosenFire)
                {
                    g.transform.GetChild(0).GetComponent<Image>().sprite = fireSprite;
                }
                else if (name == "Fire" && choosenFire)
                {
                    g.SetActive(false);
                }
                else if (name == "Healing" && !choosenHealing)
                {
                    g.transform.GetChild(0).GetComponent<Image>().sprite = healingSprite;
                }
                else if (name == "Healing" && choosenHealing)
                {
                    g.SetActive(false);
                }
                else if (name == "SpeedUp" && !choosenSpeed)
                {
                    g.transform.GetChild(0).GetComponent<Image>().sprite = speedUpSprite;
                }
                else if (name == "SpeedUp" && choosenSpeed)
                {
                    g.SetActive(false);
                }
                else if (name == "UtilmateRemake" && !choosenUlti)
                {
                    g.transform.GetChild(0).GetComponent<Image>().sprite = utilmateRemakeSprite;
                }
                else if (name == "UtilmateRemake" && choosenUlti)
                {
                    g.SetActive(false);
                }
                g.GetComponent<Button>().AddEventListener(i, ItemClicked);
            }  
        }
        Destroy(item);
    }

    void ItemClicked(int itemIndex)
    {
        string name = spellOwnList[itemIndex].spellID.name;
        int amount = spellOwnList[itemIndex].amount;
        int cooldown = spellOwnList[itemIndex].spellID.cooldown;
        SpellType spellType = 0;
        if (name == "Chaos")
        {
            path = "Spells/chaos";
            spellType = SpellType.Chaos;
        }
        else if (name == "Fire")
        {
            path = "Spells/fire";
            spellType = SpellType.Fire;
        }
        else if (name == "Healing")
        {
            path = "Spells/healing";
            spellType = SpellType.Healing;
        }
        else if (name == "SpeedUp")
        {
            path = "Spells/speedup";
            spellType = SpellType.Speedup;
        }
        else if (name == "UtilmateRemake")
        {

            path = "Spells/utilmateRemake";
            spellType = SpellType.UtilmateRemake;
        }
        spellSingleton.SetSpell(indexArr, name, path, amount,(int) spellType, 2);
        SceneManager.LoadScene(sceneNumber);
    }

    public void BackToHomeSceen(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void LoadAvatars()
    {
        fireSprite = Resources.Load<Sprite>("Spells/fire");
        chaosSprite = Resources.Load<Sprite>("Spells/chaos");
        healingSprite = Resources.Load<Sprite>("Spells/healing");
        speedUpSprite = Resources.Load<Sprite>("Spells/speedup");
        utilmateRemakeSprite = Resources.Load<Sprite>("Spells/utilmateRemake");
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }
    public void setPlacePosition(int position)
    {
        this.indexArr = position;
    }
}

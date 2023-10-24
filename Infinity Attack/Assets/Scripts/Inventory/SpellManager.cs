using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class SpellManager : MonoBehaviour
{
    [SerializeField] private SpellID[] spellList;
    [SerializeField] private Spell[] spellOwnList;
    [SerializeField] private TextMeshProUGUI nameSpell;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI cd;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image avatarDetail;
    private Api instanceIP;

    private Sprite fireSprite;
    private Sprite chaosSprite;
    private Sprite speedUpSprite;
    private Sprite healingSprite;
    private Sprite utilmateRemakeSprite;

    private int clicked = 0;
    private int fire = 0, chaos = 0, speedUp = 0, healing = 0, utilmateRemake = 0;
    private int amountFire = 0, amountSpeedUp = 0, amountChaos = 0, amountHealing = 0, amountUtilmateRemake = 0;
    void Start()
    {
        instanceIP = Api.Instance;
        //avatarDetail.gameObject.SetActive(false);
        LoadAvatars();
        if (PlayerPrefs.HasKey("UID"))
        {
            string userID = removeQuotes(PlayerPrefs.GetString("UID"));
            StartCoroutine(GetSpellOwnData(instanceIP.api + instanceIP.routerPostSpellsOwn, 0, 0, 0, 0, 0, 0, 0
           , 0, 0, 0, userID));
        }
    }
    private void LoadAvatars()
    {
        fireSprite = Resources.Load<Sprite>("Spells/fire");
        chaosSprite = Resources.Load<Sprite>("Spells/chaos");
        healingSprite = Resources.Load<Sprite>("Spells/healing");
        speedUpSprite = Resources.Load<Sprite>("Spells/speedup");
        utilmateRemakeSprite = Resources.Load<Sprite>("Spells/utilmateRemake");
    }

    void GetAllSpells(string rawResponse)
    {
        var _spell = JsonConvert.DeserializeObject<SpellID[]>(rawResponse);
        spellList = _spell;
        LoadFlag();
        LoadAmount();

        GameObject g;
        GameObject item = transform.GetChild(0).gameObject;

        for (int i = 0; i < spellList.Length; i++)
        {
            g = Instantiate(item, transform);
            string name = spellList[i].name;
            //check name spell to set amount 
            if (name == "Chaos" && chaos == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountChaos + "/10";
            }
            else if (name == "Chaos" && chaos == 0)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (name == "Fire" && fire == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountFire + "/10";
            }
            else if (name == "Fire" && fire == 0)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (name == "Healing" && healing == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountHealing + "/10";
            }
            else if (name == "Healing" && healing == 0)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (name == "SpeedUp" && speedUp == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountSpeedUp + "/10";
            }
            else if (name == "SpeedUp" && speedUp == 0)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (name == "UtilmateRemake" && utilmateRemake == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountUtilmateRemake + "/10";
            }
            else if (name == "UtilmateRemake" && utilmateRemake == 0)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }

            // check name spell to set avt
            if (name == "Chaos")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = chaosSprite;
            }
            else if (name == "Fire")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = fireSprite;
            }
            else if (name == "Healing")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = healingSprite;
            }
            else if (name == "SpeedUp")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = speedUpSprite;
            }
            else if (name == "UtilmateRemake")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = utilmateRemakeSprite;
            }
            g.GetComponent<Button>().AddEventListener(i, ItemClicked);
            g.SetActive(true);
            //update detail auto hien thi spell dau tien
            avatarDetail.sprite = fireSprite;
            nameSpell.text = spellList[0].name;
            description.text = spellList[0].description;
            cd.text = "Cooldown: " + spellList[0].cooldown;
        }
        Destroy(item);
    }

    void ItemClicked(int itemIndex)
    {
        clicked = 1;
        SaveClicked(clicked);
        StartCoroutine(GetAllSpells(instanceIP.api + instanceIP.routerGetSpells, clicked));
        Debug.Log("click id: " + spellList[itemIndex]._id);
        string name = spellList[itemIndex].name;
        // check name spell to set avt
        if (name == "Chaos")
        {
            avatarDetail.sprite = chaosSprite;
        }
        else if (name == "Fire")
        {
            avatarDetail.sprite = fireSprite;
        }
        else if (name == "Healing")
        {
            avatarDetail.sprite = healingSprite;
        }
        else if (name == "SpeedUp")
        {
            avatarDetail.sprite = speedUpSprite;
        }
        else if (name == "UtilmateRemake")
        {
            avatarDetail.sprite = utilmateRemakeSprite;
        }
        avatarDetail.gameObject.SetActive(true);
        nameSpell.text = name;
        description.text = spellList[itemIndex].description;
        cd.text = "Cooldown: " + spellList[itemIndex].cooldown;
    }

    void GetSpellOwn(string rawResponse, int chaos, int fire, int healing, int speedUp, int utilmateRemake,
        int amountChaos, int amountFire, int amountHealing, int amountSpeedUp, int amountUtilmateRemake)
    {
        var _spellOwn = JsonConvert.DeserializeObject<Spell[]>(rawResponse);
        Debug.Log("SpellOwn" + _spellOwn);
        spellOwnList = _spellOwn;

        if(spellOwnList.Length == 0)
        {
            SaveFlag(chaos, fire, healing, speedUp, utilmateRemake);
            SaveAmount(amountChaos, amountFire, amountHealing, amountSpeedUp, amountUtilmateRemake);
        }
        else
        {
            for (int i = 0; i < spellOwnList.Length; i++)
            {
                string name = spellOwnList[i].spellID.name;
                int amount = spellOwnList[i].amount;
                //Debug.Log("Spell Own List -- Name: " + name + " amount: " + amount);
                if (name == "Chaos")
                {
                    chaos = 1;
                    amountChaos = amount;
                }
                else if (name == "Fire")
                {
                    fire = 1;
                    amountFire = amount;
                }
                else if (name == "Healing")
                {
                    healing = 1;
                    amountHealing = amount;
                }
                else if (name == "SpeedUp")
                {
                    speedUp = 1;
                    amountSpeedUp = amount;
                }
                else if (name == "UtilmateRemake")
                {
                    utilmateRemake = 1;
                    amountUtilmateRemake = amount;
                }
                SaveFlag(chaos, fire, healing, speedUp, utilmateRemake);
                SaveAmount(amountChaos, amountFire, amountHealing, amountSpeedUp, amountUtilmateRemake);
            }
        }
        
        StartCoroutine(GetAllSpells(instanceIP.api + instanceIP.routerGetSpells, 0));
    }

    IEnumerator GetAllSpells(string address, int clicked)
    {
        WWWForm form = new WWWForm();
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
            LoadClicked();
            if (clicked == 1)
            {
                clicked = 0;
                string res = www.downloadHandler.text;
                var _spell = JsonConvert.DeserializeObject<SpellID[]>(res);
                spellList = _spell;
            }
            else
            {
                string res = www.downloadHandler.text;
                GetAllSpells(res);
                www.Dispose();
            }

        }
    }
    private void LoadClicked()
    {
        clicked = PlayerPrefs.GetInt("Clicked");
    }

    private void SaveClicked(int clicked)
    {
        PlayerPrefs.SetInt("Updated", clicked);
    }

    private void LoadFlag()
    {
        chaos = PlayerPrefs.GetInt("chaos");
        fire = PlayerPrefs.GetInt("fire");
        healing = PlayerPrefs.GetInt("healing");
        speedUp = PlayerPrefs.GetInt("speedUp");
        utilmateRemake = PlayerPrefs.GetInt("utilmateRemake");
    }

    private void SaveFlag(int chaos, int fire, int healing, int speedUp, int utilmateRemake)
    {
        PlayerPrefs.SetInt("chaos", chaos);
        PlayerPrefs.SetInt("fire", fire);
        PlayerPrefs.SetInt("healing", healing);
        PlayerPrefs.SetInt("speedUp", speedUp);
        PlayerPrefs.SetInt("utilmateRemake", utilmateRemake);
    }

    private void LoadAmount()
    {
        amountChaos = PlayerPrefs.GetInt("amountChaos");
        amountFire = PlayerPrefs.GetInt("amountFire");
        amountHealing = PlayerPrefs.GetInt("amountHealing");
        amountSpeedUp = PlayerPrefs.GetInt("amountSpeedUp");
        amountUtilmateRemake = PlayerPrefs.GetInt("amountUtilmateRemake");
    }

    private void SaveAmount(int amountChaos, int amountFire, int amountHealing, int amountSpeedUp, int amountUtilmateRemake)
    {
        PlayerPrefs.SetInt("amountChaos", amountChaos);
        PlayerPrefs.SetInt("amountFire", amountFire);
        PlayerPrefs.SetInt("amountHealing", amountHealing);
        PlayerPrefs.SetInt("amountSpeedUp", amountSpeedUp);
        PlayerPrefs.SetInt("amountUtilmateRemake", amountUtilmateRemake);
    }
    IEnumerator GetSpellOwnData(string address, int chaos, int fire, int healing, int speedUp, int utilmateRemake,
         int amountChaos, int amountFire, int amountHealing, int amountSpeedUp, int amountUtilmateRemake, string userID)
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
            GetSpellOwn(res, chaos, fire, healing, speedUp, utilmateRemake,
                amountChaos, amountFire, amountHealing, amountSpeedUp, amountUtilmateRemake);
            www.Dispose();
        }


    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }

}
    public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(param);
        });
    }
}




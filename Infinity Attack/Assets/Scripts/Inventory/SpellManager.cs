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
    private string addressGetAllSpells = "http://localhost:3000/inventoryClient/get-spells";
    private string addressGetSpellsOwn = "http://localhost:3000/inventoryClient/post-spells-own";
    [SerializeField] private SpellID[] spellList;
    [SerializeField] private Spell[] spellOwnList;
    [SerializeField] private TextMeshProUGUI nameSpell;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI cd;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image avatarDetail;


    private Sprite fireSprite;
    private Sprite armorSprite;
    private Sprite smiteSprite;
    private Sprite healingSprite;
    private Sprite frozenSprite;
    private Sprite boomSprite;
    private Sprite soloQSprite;

    private int clicked = 0;
    private int fire = 0, armor = 0, smite = 0, healing = 0, frozen = 0, boom = 0, soloQ = 0;
    private int amountFire = 0, amountArmor = 0, amountSmite = 0, amountHealing = 0, amountFrozen = 0, amountBoom = 0, amountSoloQ = 0;
    void Start()
    {
        
        LoadAvatars();
        StartCoroutine(GetSpellOwnData(addressGetSpellsOwn, 0, 0, 0, 0, 0, 0, 0
            , 0, 0, 0, 0, 0, 0, 0));
        
    }
    private void LoadAvatars()
    {
        fireSprite = Resources.Load<Sprite>("Spell/fire");
        armorSprite = Resources.Load<Sprite>("Spell/armor");
        smiteSprite = Resources.Load<Sprite>("Spell/smite");
        healingSprite = Resources.Load<Sprite>("Spell/healing");
        frozenSprite = Resources.Load<Sprite>("Spell/frozen");
        boomSprite = Resources.Load<Sprite>("Spell/boom");
        soloQSprite = Resources.Load<Sprite>("Spell/soloq");
    }

    
    void getAllSpells(string rawResponse)
    {
        var _spell = JsonConvert.DeserializeObject<SpellID[]>(rawResponse);
        spellList = _spell;
        LoadFlag();
        LoadAmount();
        //Debug.Log("flag fire: " + fire + " amount: " + amountFire);
        //Debug.Log("flag armor: " + armor + " amount: " + amountArmor);
        //Debug.Log("flag smite: " + smite + " amount: " + amountSmite);
        //Debug.Log("flag healing: " + healing + " amount: " + amountHealing);
        //Debug.Log("flag frozen: " + frozen + " amount: " + amountFrozen);
        //Debug.Log("flag boom: " + boom + " amount: " + amountBoom);
        //Debug.Log("flag soloq: " + soloQ + " amount: " + amountSoloQ);

        GameObject g;
        GameObject item = transform.GetChild(0).gameObject;

        for (int i = 0; i < spellList.Length; i++)
        {
            g = Instantiate(item, transform);
            string name = spellList[i].name;
            //check name spell to set amount 
            if (name == "Fire" && fire == 1)
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
            else if (name == "Armor" && armor == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountArmor + "/10";
            }
            else if (name == "Armor" && armor == 0)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (name == "Smite" && smite == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountSmite + "/10";
            }
            else if (name == "Smite" && smite == 0)
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
            else if (name == "Frozen" && frozen == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountFrozen + "/10";
            }
            else if (name == "Frozen" && frozen == 0)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (name == "Boom" && boom == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountBoom + "/10";
            }
            else if (name == "Boom" && boom == 0)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/10";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.grey;
                g.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
                item.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);

            }
            else if (name == "SoloQ" && soloQ == 1)
            {
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amountSoloQ + "/10";
            }
            else if (name == "SoloQ" && soloQ == 0)
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
            if (name == "Fire")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = fireSprite;
            }
            else if (name == "Armor")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = armorSprite;
            }
            else if (name == "Smite")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = smiteSprite;
            }
            else if (name == "Healing")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = healingSprite;
            }
            else if (name == "Frozen")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = frozenSprite;
            }
            else if (name == "Boom")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = boomSprite;
            }
            else if (name == "SoloQ")
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = soloQSprite;
            }
            g.GetComponent<Button>().AddEventListener(i, ItemClicked);

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
        StartCoroutine(GetAllSpells(addressGetAllSpells, clicked));
        Debug.Log("click id: " + spellList[itemIndex]._id);
        string name = spellList[itemIndex].name;
        // check name spell to set avt
        if (name == "Fire")
        {
            avatarDetail.sprite = fireSprite;
        }
        else if (name == "Armor")
        {
            avatarDetail.sprite = armorSprite;
        }
        else if (name == "Smite")
        {
            avatarDetail.sprite = smiteSprite;
        }
        else if (name == "Healing")
        {
            avatarDetail.sprite = healingSprite;
        }
        else if (name == "Frozen")
        {
            avatarDetail.sprite = frozenSprite;
        }
        else if (name == "Boom")
        {
            avatarDetail.sprite = boomSprite;
        }
        else if (name == "SoloQ")
        {
            avatarDetail.sprite = soloQSprite;
        }
        nameSpell.text = name;
        description.text = spellList[itemIndex].description;
        cd.text = "Cooldown: " + spellList[itemIndex].cooldown;
    }

    void getSpellOwn(string rawResponse, int fire, int armor, int smite, int healing, int frozen, int boom, int soloQ,
        int amountFire, int amountArmor, int amountSmite, int amountHealing, int amountFrozen, int amountBoom, int amountSoloQ)
    {
        var _spellOwn = JsonConvert.DeserializeObject<Spell[]>(rawResponse);
        Debug.Log("SpellOwn" + _spellOwn);
        spellOwnList = _spellOwn;
        
        for(int i = 0; i < spellOwnList.Length; i++)
        {
            string name = spellOwnList[i].spellID.name;
            int amount = spellOwnList[i].amount;
            //Debug.Log("Spell Own List -- Name: " + name + " amount: " + amount);
            if (name == "Fire")
            {
                fire = 1;
                amountFire = amount;
            }
            else if(name == "Armor"){
                armor = 1;
                amountArmor = amount;
            }
            else if (name == "Smite")
            {
                smite = 1;
                amountSmite = amount;
            }
            else if (name == "Healing")
            {
                healing = 1;
                amountHealing = amount;
            }
            else if (name == "Frozen")
            {
                frozen = 1;
                amountFrozen = amount;
            }
            else if (name == "Boom")
            {
                boom = 1;
                amountBoom = amount;
            }
            else if (name == "SoloQ")
            {
                soloQ = 1;
                amountSoloQ = amount;
            }
            SaveFlag(fire, armor, smite, healing, frozen, boom, soloQ);
            SaveAmount(amountFire, amountArmor, amountSmite, amountHealing, amountFrozen, amountBoom, amountSoloQ);
        }
        StartCoroutine(GetAllSpells(addressGetAllSpells, 0));
    }

    IEnumerator GetAllSpells(string address, int clicked)
    {
        UnityWebRequest www = UnityWebRequest.Get(address);
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
                getAllSpells(res);
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
        fire = PlayerPrefs.GetInt("fire");
        armor = PlayerPrefs.GetInt("armor");
        smite = PlayerPrefs.GetInt("smite");
        healing = PlayerPrefs.GetInt("healing");
        frozen = PlayerPrefs.GetInt("frozen");
        boom = PlayerPrefs.GetInt("boom");
        soloQ = PlayerPrefs.GetInt("soloQ");
    }

    private void SaveFlag(int fire, int armor, int smite, int healing, int frozen, int boom, int soloQ)
    {
        PlayerPrefs.SetInt("fire", fire);
        PlayerPrefs.SetInt("armor", armor);
        PlayerPrefs.SetInt("smite", smite);
        PlayerPrefs.SetInt("healing", healing);
        PlayerPrefs.SetInt("frozen", frozen);
        PlayerPrefs.SetInt("boom", boom);
        PlayerPrefs.SetInt("soloQ", soloQ);
    }

    private void LoadAmount()
    {
        amountFire = PlayerPrefs.GetInt("amountFire");
        amountArmor = PlayerPrefs.GetInt("amountArmor");
        amountSmite = PlayerPrefs.GetInt("amountSmite");
        amountHealing = PlayerPrefs.GetInt("amountHealing");
        amountFrozen = PlayerPrefs.GetInt("amountFrozen");
        amountBoom = PlayerPrefs.GetInt("amountBoom");
        amountSoloQ = PlayerPrefs.GetInt("amountSoloQ");
    }

    private void SaveAmount(int amountFire, int amountArmor, int amountSmite, int amountHealing, int amountFrozen, int amountBoom, int amountSoloQ)
    {
        PlayerPrefs.SetInt("amountFire", amountFire);
        PlayerPrefs.SetInt("amountArmor", amountArmor);
        PlayerPrefs.SetInt("amountSmite", amountSmite);
        PlayerPrefs.SetInt("amountHealing", amountHealing);
        PlayerPrefs.SetInt("amountFrozen", amountFrozen);
        PlayerPrefs.SetInt("amountBoom", amountBoom);
        PlayerPrefs.SetInt("amountSoloQ", amountSoloQ);
    }
    IEnumerator GetSpellOwnData(string address, int fire, int armor, int smite, int healing, int frozen, int boom, int soloQ,
         int amountFire, int amountArmor, int amountSmite, int amountHealing, int amountFrozen, int amountBoom, int amountSoloQ)
    {
        //string userID = PlayerPrefs.GetString("uID");
        //WWWForm form = new WWWForm();
        //form.AddField("userID", userID);
        //UnityWebRequest www = UnityWebRequest.Post(address,form);

        string userID = "6345a02f1d8f5da83dc48826";
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
            getSpellOwn(res,fire, armor, smite, healing, frozen, boom, soloQ,
                amountFire,amountArmor,amountSmite,amountHealing,amountFrozen,amountBoom,amountSoloQ);
            www.Dispose();
        }


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
public class Spell
{
    public int amount;
    public SpellID spellID;

    public class SpellID
    {
        public string _id;
        public string name;
    }
}
public class SpellID
{
    public string _id;
    public string name;
    public string description;
    public int cooldown;
}



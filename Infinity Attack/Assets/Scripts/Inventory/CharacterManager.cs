using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class CharacterManager : MonoBehaviour
{
    private Api instanceIP;
    [SerializeField] private Character[] charList;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI lv;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button btUpdateLv;
    [SerializeField] private TextMeshProUGUI btUpdateLvText;
    [SerializeField] private Image avatar;
    private int selectedOption = 0;
    private int updated = 0;
    private Sprite fire_knightsprite;
    private Sprite ground_monksprite;
    private Sprite leaf_rangersprite;
    private Sprite water_priestesssprite;
    private Sprite metal_bladekeepersprite;
    private Sprite wind_hashashinsprite;

    private void Awake()
    {

    }
    void Start()
    {
        instanceIP = Api.Instance;
        LoadAvatars();
        StartCoroutine(GetCharacterOwnData(instanceIP.api + instanceIP.routerPostCharactersOwn));    
    }

    private void loadChar()
    {
        if (PlayerPrefs.HasKey("SelectedOption"))
        {
            Load();
        }
        else
        {
            selectedOption = 0;
        }
        Debug.Log("selected: " + selectedOption);
        UpdateCharacterSelected(selectedOption);
    }
    void ProcessServerResponse(string rawResponse)
    {
        var _char = JsonConvert.DeserializeObject<Character[]>(rawResponse);
        charList = _char;
        LoadUpdated();
        Debug.Log("updated: " + updated);
        if (updated == 1)
        {
            updated = 0;
            SaveUpdated(updated);
            loadChar();
        }
        else
        {
            LoadStatusFromServer();
            loadChar();
        }
       
    }
    IEnumerator GetCharacterOwnData(string address)
    {
        //string userID = PlayerPrefs.GetString("uID");
        //WWWForm form = new WWWForm();
        //form.AddField("userID", userID);
        //UnityWebRequest www = UnityWebRequest.Post(address,form);

        string userID = "6345a02f1d8f5da83dc48826";
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        UnityWebRequest www = UnityWebRequest.Post(address, form);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            string res = www.downloadHandler.text;
            ProcessServerResponse(res);
            www.Dispose();
        }
    }

    public void NextOption()
    {
        selectedOption++;
        if(selectedOption >= charList.Length)
        {
            selectedOption = 0;
        }
        Debug.Log(selectedOption);
        Save(selectedOption);
        UpdateCharacterSelected(selectedOption);
    }

    public void BackOption()
    {
        selectedOption--;
        if(selectedOption < 0)
        {
            selectedOption = charList.Length - 1;
        }
        Save(selectedOption);
        UpdateCharacterSelected(selectedOption);
    }

    private void LoadAvatars()
    {
        fire_knightsprite = Resources.Load<Sprite>("CharacterOwns/fire_knight");
        ground_monksprite = Resources.Load<Sprite>("CharacterOwns/ground_monk");
        leaf_rangersprite = Resources.Load<Sprite>("CharacterOwns/leaf_ranger");
        water_priestesssprite = Resources.Load<Sprite>("CharacterOwns/water_priestess");
        metal_bladekeepersprite = Resources.Load<Sprite>("CharacterOwns/metal_bladekeeper");
        wind_hashashinsprite = Resources.Load<Sprite>("CharacterOwns/wind_hashashin");
    }

    private void UpdateCharacterSelected(int selectedOption)
    {
        Character character = charList[selectedOption];
        nameText.text = character.characterID.name;
        description.text = character.characterID.description;
        lv.text = "Level: " + character.levelID.level;
        hp.text = "HP: " + character.levelID.hp;
        damage.text = "Damage: " + character.levelID.damage;
        costText.text = character.levelID.cost + " Gold";
        int cost = character.levelID.cost;
        int gold = character.userID.gold;
        if (gold < cost)
        {
            btUpdateLv.interactable = false;
            btUpdateLvText.color = Color.gray;
            costText.color = Color.red;
        }
        else
        {
            btUpdateLv.interactable = true;
            btUpdateLvText.color = Color.white;
            costText.color = Color.yellow;
        }
        if (character.levelID.level  == 10)
        {
            btUpdateLv.interactable = false;
            btUpdateLvText.color = Color.gray;
            costText.color = Color.red;
            costText.text = "Max";
        }
        string name = character.characterID.name;
        if (name == "Fire Knight")
        {
            avatar.sprite = fire_knightsprite;
            avatar.GetComponent<RectTransform>().anchoredPosition = new Vector2(460, 0);
        }
        else if (name == "Ground Monk")
        {
            avatar.sprite = ground_monksprite;
            avatar.GetComponent<RectTransform>().anchoredPosition = new Vector2(510, 0);
        }
        else if (name == "Leaf Ranger")
        {
            avatar.sprite = leaf_rangersprite;
            avatar.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 0);
        }
        else if (name == "Metal Bladekeeper")
        {
            avatar.sprite = metal_bladekeepersprite;
            avatar.GetComponent<RectTransform>().anchoredPosition = new Vector2(520, 0);
        }
        else if (name == "Water Priestess")
        {
            avatar.sprite = water_priestesssprite;
            avatar.GetComponent<RectTransform>().anchoredPosition = new Vector2(510, 0);
        }
        else if (name == "Wind Hashashin")
        {
            avatar.sprite = wind_hashashinsprite;
            avatar.GetComponent<RectTransform>().anchoredPosition = new Vector2(530, 0);
        }
    }

    public void UpdateLevelCharacter()
    {
        int updated = 1;
        SaveUpdated(updated);
        StartCoroutine(updateLevelCharacter(instanceIP.api + instanceIP.routerUpdateCharacterOwn, selectedOption));
    }

    IEnumerator updateGoldAfterUpdate(string address, int selectedOption)
    {
        Character character = charList[selectedOption];
        int cost = character.levelID.cost;
        int gold = character.userID.gold;
        string name = character.userID.name;
        int gold_after_update = gold - cost;
        WWWForm formUpdateGold = new WWWForm();
        formUpdateGold.AddField("name", name);
        formUpdateGold.AddField("gold", gold_after_update);
        
        UnityWebRequest www = UnityWebRequest.Post(address, formUpdateGold);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            StartCoroutine(GetCharacterOwnData(instanceIP.api + instanceIP.routerPostCharactersOwn));                    
        }
    }

    IEnumerator updateLevelCharacter(string address, int selectedOption)
    {
        Character character = charList[selectedOption];
        int lv_now = character.levelID.level;
        int lvUpdate = lv_now + 1;
        string characterID = character.characterID._id;
        string characterOwnID = character._id;
        WWWForm formCharacterUpdate = new WWWForm();
        formCharacterUpdate.AddField("level", lvUpdate);
        formCharacterUpdate.AddField("characterID", characterID);
        formCharacterUpdate.AddField("characterOwnID", characterOwnID);
        UnityWebRequest www = UnityWebRequest.Post(address, formCharacterUpdate);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            StartCoroutine(updateGoldAfterUpdate(instanceIP.api + instanceIP.routerGoldUser, selectedOption));
        }
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("SelectedOption");
    }

    private void Save(int selectedOption)
    {
        PlayerPrefs.SetInt("SelectedOption", selectedOption);
    }

    private void LoadUpdated()
    {
        updated = PlayerPrefs.GetInt("Updated");
    }

    private void SaveUpdated(int updated)
    {
        PlayerPrefs.SetInt("Updated", updated);
    }

    private void LoadStatusFromServer()
    {
        for (int i = 0; i < charList.Length; i++)
        {
            if (charList[i].status == 1)
            {
                PlayerPrefs.SetInt("SelectedOption", i);
                break;
            }
        }
    }
    public void BackToHomeSceen(int screenNumber)
    {
        StartCoroutine(updateStatusCharacter(instanceIP.api + instanceIP.routerUpdateStatusCharacterOwn, selectedOption,screenNumber));
    }

    IEnumerator updateStatusCharacter(string address, int selectedOption,int screenNumber)
    {
        Character character = charList[selectedOption];
        string characterOwnIDOld = null;
        string characterOwnIDNew = character._id;
        for (int i = 0; i <= charList.Length; i++)
        {
            if(charList[i].status == 1)
            {
                characterOwnIDOld = charList[i]._id;
                break;
            }
        }
        WWWForm formUpdateStatus = new WWWForm();
        formUpdateStatus.AddField("characterOwnIDOld", characterOwnIDOld);
        formUpdateStatus.AddField("characterOwnIDNew", characterOwnIDNew);
       
        UnityWebRequest www = UnityWebRequest.Post(address, formUpdateStatus);
        var handler = www.SendWebRequest();
        while (!handler.isDone)
        {
            yield return null;
        }
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            SceneManager.LoadScene(screenNumber);
        }
        www.Dispose();
    }
}

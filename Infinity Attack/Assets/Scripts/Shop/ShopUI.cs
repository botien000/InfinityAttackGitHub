using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public enum StateShop
    {
        Gold, Gem, Pay
    }

    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private RectTransform goldItemsGO, gemItemsGO;
    [SerializeField] private Image imgGold, imgGem;
    [SerializeField] private Color colorNot;
    [SerializeField] private ButtonShopCharacter buttonShopCharacterOrigin;
    [SerializeField] private ButtonShopSpell buttonShopSpellOrigin;
    [SerializeField] private GameObject loadingGO;
    [SerializeField] private Transform transfCenterShop;
    [SerializeField] private TextMeshProUGUI txtGold;
    [SerializeField] private TextMeshProUGUI txtGem;

    public ConfirmUI confirmUI;

    private Api instanceIP;
    private CharacterUtility[] characters = null;
    private CharacterOwn[] charactersOwn = null;
    private SpellUtility[] spells = null;
    private SpellOwnUtility[] spellOwns = null;
    private List<GameObject> curList = new List<GameObject>();
    private List<CharacterUtility> characterNotOwnList = new List<CharacterUtility>();
    private List<CharacterUtility> charactersNotOwn;
    private StateShop curStateShop;

    public static ShopUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        instanceIP = Api.Instance;
        StartCoroutine(IEGetUser());
        StartCoroutine(IEGetAllSpell());
        curStateShop = StateShop.Pay;
        SetState(0);
    }
    private void OnDisable()
    {
        foreach (var gObj in curList)
        {
            Destroy(gObj);
        }
        curList.Clear();
    }
    public void SetAnimationLoading(bool state)
    {
        loadingGO.SetActive(state);
    }
    public void SetTextGem(string text)
    {
        txtGem.text = text;
    }
    public void SetTextGold(string text)
    {
        txtGold.text = text;
    }
    public void BtnShop(int type)
    {
        SetState((StateShop)type);
    }
    private void SetState(StateShop state)
    {
        if (curStateShop == state)
            return;
        curStateShop = state;
        SetCenterParent();
        switch (state)
        {
            case StateShop.Gold:
                scrollView.content = goldItemsGO;
                imgGold.color = Color.white;
                imgGem.color = colorNot;
                goldItemsGO.gameObject.SetActive(true);
                gemItemsGO.gameObject.SetActive(false);
                break;
            case StateShop.Gem:
                scrollView.content = gemItemsGO;
                imgGem.color = Color.white;
                imgGold.color = colorNot;
                gemItemsGO.gameObject.SetActive(true);
                goldItemsGO.gameObject.SetActive(false);
                break;
        }
    }
    private void SetCenterParent()
    {
        goldItemsGO.transform.position = transfCenterShop.position;
        gemItemsGO.transform.position = transfCenterShop.position;
    }
    private IEnumerator IEGetAllCharacter()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(instanceIP.api + instanceIP.routerGetCharacters, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            SetAnimationLoading(true);
            yield return null;
        }
        SetAnimationLoading(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEGetAllCharacter:   " + json);
            if (json != "[]")
            {
                characters = JsonConvert.DeserializeObject<CharacterUtility[]>(json);
            }
            else
            {
                Debug.Log("No existing characters");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
        StartCoroutine(IEGetCharacterOwn());
    }
    private IEnumerator IEGetCharacterOwn()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID",User.Instance.user._id);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(instanceIP.api + instanceIP.routerCharacterOwn, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            SetAnimationLoading(true);
            yield return null;
        }
        SetAnimationLoading(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEGetCharacterOwn:   " + json);
            charactersOwn = JsonConvert.DeserializeObject<CharacterOwn[]>(json);
            FindChacterNotOwn();
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    private IEnumerator IEGetAllSpell()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(instanceIP.api + instanceIP.routerGetSpells, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            SetAnimationLoading(true);
            yield return null;
        }
        SetAnimationLoading(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEGetAllSpell:   " + json);
            spells = JsonConvert.DeserializeObject<SpellUtility[]>(json);
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
        StartCoroutine(IEGetSpellOwns());
    }
    private IEnumerator IEGetSpellOwns()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", User.Instance.user._id);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(instanceIP.api + instanceIP.routerGetSpellOwn, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            SetAnimationLoading(true);
            yield return null;
        }
        SetAnimationLoading(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEGetSpellOwns:   " + json);
            spellOwns = JsonConvert.DeserializeObject<SpellOwnUtility[]>(json);
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    private void FindChacterNotOwn()
    {
        bool isOwned;
        characterNotOwnList.Clear();
        if (characters == null || charactersOwn == null || characters.Length == 0)
            return;
        for (int i = 0; i < characters.Length; i++)
        {
            isOwned = false;
            for (int j = 0; j < charactersOwn.Length; j++)
            {
                if (characters[i]._id == charactersOwn[j].characterID)
                {
                    isOwned = true;
                    break;
                }
            }
            if (!isOwned)
            {
                characterNotOwnList.Add(characters[i]);
            }
        }
        GetComponent<ShopUI>().Init(characterNotOwnList);
    }
    private IEnumerator IEGetUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", User.Instance.user.name);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(instanceIP.api + instanceIP.routerGetUserByName, form);
        var handler = unityWebRequest.SendWebRequest(); 
        while (!handler.isDone)
        {
            SetAnimationLoading(true);
            yield return null;
        }
        SetAnimationLoading(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEGetUser:   " + json);
            if (json != "[]")
            {
                User.Instance.user = JsonConvert.DeserializeObject<User>(json);
            }
            else
            {
                Debug.Log("No existing characters");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
        StartCoroutine(IEGetAllCharacter());
    }
    public void Init(List<CharacterUtility> characters)
    {
        if (characters == null)
            return;
        charactersNotOwn = characters;
        SpawnCharacterToBuy();
        SpawnSpellToBuy();
    }
    private void SpawnCharacterToBuy()
    {
        foreach (var character in charactersNotOwn)
        {
            ButtonShopCharacter btnShopCharacter = Instantiate(buttonShopCharacterOrigin, gemItemsGO);
            btnShopCharacter.Init(character._id, character.name, character.price);
            curList.Add(btnShopCharacter.gameObject);
        }
    }
    
    private void SpawnSpellToBuy()
    {
        foreach (var spell in spells)
        {
            ButtonShopSpell btnShopSpell = Instantiate(buttonShopSpellOrigin, goldItemsGO);
            btnShopSpell.Init(spell._id, spell.name, spell.price,spell.total);
            curList.Add(btnShopSpell.gameObject);
        }
    }

    internal int CheckCurAmountSpell(string id)
    {
        foreach (var spellOwn in spellOwns)
        {
            if(spellOwn.spellID == id)
            {
                return spellOwn.amount;
            }
        }
        return 0;
    }
    public string CheckIdSpellOwn(string idSpell)
    {
        foreach (var spellOwn in spellOwns)
        {
            if (spellOwn.spellID == idSpell)
            {
                return spellOwn._id;
            }
        }
        return "";
    }

    internal void UpdateSpellOwnIfNotExisting(SpellOwnUtility spellOwnUtility)
    {
        bool check = false;
        foreach (var spellOwn in spellOwns)
        {
            if (spellOwn._id == spellOwnUtility._id)
            {
                check = true;
                break;
            }
        }
        if (!check)
        {
            List<SpellOwnUtility> list = new List<SpellOwnUtility>();
            foreach (var spellOwn in spellOwns)
            {
                list.Add(spellOwn);
            }
            list.Add(spellOwnUtility);
            spellOwns = list.ToArray();
        }
    }

    public void BackToHomeScene()
    {
        SavePlay(1);
        SceneManager.LoadScene("Home");
    }

    private void SavePlay(int play)
    {
        PlayerPrefs.SetInt("Play", play);
    }
}
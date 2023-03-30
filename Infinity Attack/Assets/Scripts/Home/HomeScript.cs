using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour
{
    private UserID user;
    private Top5Users[] top5Users;
    public Character[] charList;

    private string addressGetUsingCharName = Api.Instance.api+Api.Instance.routerGetUsingCharNameById;
    private string addressGetUser = Api.Instance.api + Api.Instance.routerGetUserById;

    [SerializeField] private TMP_Text GoldText;
    [SerializeField] private TMP_Text GemText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private Image avatar;
    [SerializeField] private GameObject Character;
    [SerializeField] private GameObject panelLoading;
    [SerializeField] private TMP_Text txtID;


    [SerializeField] FindResponeFriend findResponeFriend;

    private Sprite fire_knightsprite;
    private Sprite ground_monksprite;
    private Sprite leaf_rangersprite;
    private Sprite water_priestesssprite;
    private Sprite metal_bladekeepersprite;
    private Sprite wind_hashashinsprite;

    private Sprite c_fire_knight;
    private Sprite c_ground_monk;
    private Sprite c_leaf_ranger;
    private Sprite c_water_priestess;
    private Sprite c_metal_bladekeeper;
    private Sprite c_wind_hashashin;

    private int CharInt = 0;
    public string charUsingName;
    public static HomeScript instance;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetData());
        SoundManager.instance.SetLg_ResMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UserID UserJson(string a)
    {
        var user = JsonConvert.DeserializeObject<UserID>(a);
        return user;
    }

    public IEnumerator GetData()
    {
        // tìm thông tin chi tiết user và gán 
        WWWForm form_getUser = new WWWForm();
        string uid = removeQuotes(PlayerPrefs.GetString("UID"));
        form_getUser.AddField("id", uid);
        using (UnityWebRequest www = UnityWebRequest.Post(addressGetUser, form_getUser))
        {
            panelLoading.SetActive(true);
            yield return www.SendWebRequest();
            panelLoading.SetActive(false);
            if (www.result != UnityWebRequest.Result.Success)
            {
                panelLoading.SetActive(true);
            }
            else
            {
                user = UserJson(www.downloadHandler.text);
                User.Instance.user = new User();
                User.Instance.user._id = user._id.ToString();
                User.Instance.user.name = user.name;
                User.Instance.user.gold = user.gold;
                User.Instance.user.gem = user.gem;
                txtID.SetText("ID: " + user._id);
                GoldText.SetText("" + user.gold);
                GemText.SetText("" + user.gem);
                NameText.SetText("" + user.name);
            }
        }

        // lấy thông tin tướng đang sử dụng
        WWWForm form_getChar = new WWWForm();
        form_getChar.AddField("userID", uid);

        using (UnityWebRequest www = UnityWebRequest.Post(addressGetUsingCharName, form_getChar))
        {
            var handler = www.SendWebRequest();
            if (!handler.isDone)
            {
                avatar.gameObject.SetActive(false);
                Character.SetActive(false);
            }
            panelLoading.SetActive(true);
            yield return handler;
            panelLoading.SetActive(false);

            if (www.result != UnityWebRequest.Result.Success)
            {
                avatar.gameObject.SetActive(false);
                Character.SetActive(false);
                panelLoading.SetActive(true);
            }
            else
            {
                LoadAvatars();
                LoadChar();
                var name = removeQuotes(www.downloadHandler.text);
                charUsingName = name;
                if (name == "Fire Knight")
                {
                    avatar.sprite = fire_knightsprite;
                    Character.GetComponent<Image>().sprite = c_fire_knight;
                    CharInt = 0;
                }
                else if (name == "Ground Monk")
                {
                    avatar.sprite = ground_monksprite;
                    Character.GetComponent<Image>().sprite = c_ground_monk;
                    CharInt = 1;
                }
                else if (name == "Leaf Ranger")
                {
                    avatar.sprite = leaf_rangersprite;
                    Character.GetComponent<Image>().sprite = c_leaf_ranger;
                    CharInt = 2;
                }
                else if (name == "Metal Bladekeeper")
                { 
                    avatar.sprite = metal_bladekeepersprite;
                    Character.GetComponent<Image>().sprite = c_metal_bladekeeper;
                    CharInt = 3;
                }
                else if (name == "Water Priestess")
                {
                    avatar.sprite = water_priestesssprite;
                    Character.GetComponent<Image>().sprite = c_water_priestess;
                    CharInt = 4;
                }
                else if (name == "Wind Hashashin")
                {
                    avatar.sprite = wind_hashashinsprite;
                    Character.GetComponent<Image>().sprite = c_wind_hashashin;
                    CharInt = 5;
                }
                avatar.gameObject.SetActive(true);
                Character.SetActive(true);
                Animator anim = Character.GetComponent<Animator>();
                anim.SetInteger("CharInt", CharInt);

                //find respone friend
                findResponeFriend.Find();
            }
            www.Dispose();
        }
    }

    private void LoadAvatars()
    {
        fire_knightsprite = Resources.Load<Sprite>("Avatars/Fire Knight");
        ground_monksprite = Resources.Load<Sprite>("Avatars/Ground Monk");
        leaf_rangersprite = Resources.Load<Sprite>("Avatars/Leaf Ranger");
        water_priestesssprite = Resources.Load<Sprite>("Avatars/Water Priestess");
        metal_bladekeepersprite = Resources.Load<Sprite>("Avatars/Metal Bladekeeper");
        wind_hashashinsprite = Resources.Load<Sprite>("Avatars/Wind Hashashin");
    }

    private void LoadChar()
    {
        c_fire_knight = Resources.Load<Sprite>("CharacterIdle/Idle_Fire_Knight");
        c_ground_monk = Resources.Load<Sprite>("CharacterIdle/Idle_Ground_Monk");
        c_leaf_ranger = Resources.Load<Sprite>("CharacterIdle/Idle_Leaf_Ranger");
        c_water_priestess = Resources.Load<Sprite>("CharacterIdle/Idle_Water_Priestess");
        c_metal_bladekeeper = Resources.Load<Sprite>("CharacterIdle/Idle_Metal_Bladekeeper");
        c_wind_hashashin = Resources.Load<Sprite>("CharacterIdle/Idle_Wind_Hashashin");
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }

    public void LogOut()
    {
        PlayerPrefs.SetString("token", null);
        SceneManager.LoadScene("Login_Register");
    }
}

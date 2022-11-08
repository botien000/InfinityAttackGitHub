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

    private string addressGetUsingCharName = "http://localhost:3000/api/getUsingCharNameById";
    private string addressGetUser = "http://localhost:3000/api/getUserById";
    private string addressGetTop10Users = "http://localhost:3000/api/getTop5Users";

    [SerializeField] private TMP_Text GoldText;
    [SerializeField] private TMP_Text GemText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private Image avatar;
    [SerializeField] private GameObject Character;
    [SerializeField] private TMP_Text Top1Name;
    [SerializeField] private TMP_Text Top1Count;
    [SerializeField] private TMP_Text Top2Name;
    [SerializeField] private TMP_Text Top2Count;
    [SerializeField] private TMP_Text Top3Name;
    [SerializeField] private TMP_Text Top3Count;
    [SerializeField] private TMP_Text Top4Name;
    [SerializeField] private TMP_Text Top4Count;
    [SerializeField] private TMP_Text Top5Name;
    [SerializeField] private TMP_Text Top5Count;

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetData());
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
        string id = removeQuotes(PlayerPrefs.GetString("UID"));
        form_getUser.AddField("id", id);
        using (UnityWebRequest www = UnityWebRequest.Post(addressGetUser, form_getUser))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                user = UserJson(www.downloadHandler.text);
                User.Instance.user = new User();
                User.Instance.user._id = user._id.ToString();
                User.Instance.user.name = user.name;
                User.Instance.user.gold = user.gold;
                User.Instance.user.gem = user.gem;
                GoldText.SetText("" + user.gold);
                GemText.SetText("" + user.gem);
                NameText.SetText("" + user.name);
            }
        }

        // lấy thông tin tướng đang sử dụng
        WWWForm form_getChar = new WWWForm();
        form_getChar.AddField("id", id);

        using (UnityWebRequest www = UnityWebRequest.Post(addressGetUsingCharName, form_getChar))
        {
            var handler = www.SendWebRequest();
            if (!handler.isDone)
            {
                avatar.gameObject.SetActive(false);
                Character.SetActive(false);
            }
            yield return handler;

            if (www.result != UnityWebRequest.Result.Success)
            {
                avatar.gameObject.SetActive(false);
                Character.SetActive(false);
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                LoadAvatars();
                LoadChar();
                var name = removeQuotes(www.downloadHandler.text);
                Debug.Log("Name: " + www.downloadHandler.text);
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
                else if (name == "Metal BladeKeeper")
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
        }

        //using (UnityWebRequest webRequest = UnityWebRequest.Get(addressGetTop10Users))
        //{
        //    // Request and wait for the desired page.
        //    yield return webRequest.SendWebRequest();

        //    if (webRequest.result != UnityWebRequest.Result.Success)
        //    {
        //        Debug.Log(webRequest.error);
        //    }
        //    else
        //    {
        //        Debug.Log("Form upload complete!");
        //        Debug.Log("Json: " + webRequest.downloadHandler.text);
        //        top5Users = JsonConvert.DeserializeObject<Top5Users[]>(webRequest.downloadHandler.text);

        //        Top1Name.SetText("" + top5Users[0].name);
        //        Top1Count.SetText("" + top5Users[0].count);
        //        Top2Name.SetText("" + top5Users[1].name);
        //        Top2Count.SetText("" + top5Users[1].count);
        //        Top3Name.SetText("" + top5Users[2].name);
        //        Top3Count.SetText("" + top5Users[2].count);
        //        Top4Name.SetText("" + top5Users[3].name);
        //        Top4Count.SetText("" + top5Users[3].count);
        //        Top5Name.SetText("" + top5Users[4].name);
        //        Top5Count.SetText("" + top5Users[4].count);
        //    }
        //}
    }

    private void LoadAvatars()
    {
        fire_knightsprite = Resources.Load<Sprite>("Avatars/fire_knight");
        ground_monksprite = Resources.Load<Sprite>("Avatars/ground_monk");
        leaf_rangersprite = Resources.Load<Sprite>("Avatars/leaf_ranger");
        water_priestesssprite = Resources.Load<Sprite>("Avatars/water_priestess");
        metal_bladekeepersprite = Resources.Load<Sprite>("Avatars/metal_bladekeeper");
        wind_hashashinsprite = Resources.Load<Sprite>("Avatars/wind_hashashin");
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
        PlayerPrefs.SetString("UID", "");
        SceneManager.LoadScene(0);
    }
}

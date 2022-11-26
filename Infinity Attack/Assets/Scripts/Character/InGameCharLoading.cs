using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Cinemachine;

public class InGameCharLoading : MonoBehaviour
{
    [SerializeField] private GameObject Fire_Knight;
    [SerializeField] private GameObject Water_Priestess;
    [SerializeField] private GameObject Metal_Bladekeeper;
    [SerializeField] private GameObject Wind_Hashashin;
    [SerializeField] private Image HealthBar;

    [SerializeField] private CinemachineVirtualCamera camera;

    private string addressGetUsingCharName = Api.Instance.api + Api.Instance.routerGetUsingCharNameById;
    private string addressGetLevelByCharNameAndUid = Api.Instance.api + Api.Instance.routerGetLevelByCharNameAndUid;

    private string charUsingName;
    public int damage;
    private int hp;
    public int curHp;



    public static InGameCharLoading instance;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetData());
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public IEnumerator GetData()
    {
        // l?y thông tin t??ng ?ang s? d?ng
        string uid = removeQuotes(PlayerPrefs.GetString("UID"));
        WWWForm form_getChar = new WWWForm();
        form_getChar.AddField("userID", uid);

        using (UnityWebRequest www = UnityWebRequest.Post(addressGetUsingCharName, form_getChar))
        {
            var handler = www.SendWebRequest();
            if (!handler.isDone)
            {
            }
            yield return handler;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                var name = removeQuotes(www.downloadHandler.text);
                Debug.Log("Name: " + www.downloadHandler.text);
                charUsingName = name;
                if (name == "Fire Knight")
                {
                    Fire_Knight.SetActive(true);
                }
                else if (name == "Ground Monk")
                {
                    
                }
                else if (name == "Leaf Ranger")
                {
                    
                }
                else if (name == "Metal BladeKeeper")
                {
                    
                }
                else if (name == "Water Priestess")
                {
                    Water_Priestess.SetActive(true);
                }
                else if (name == "Wind Hashashin")
                {
                    Wind_Hashashin.SetActive(true);
                    camera.Follow = Wind_Hashashin.GetComponent<Transform>();
                }
            }
        }

        WWWForm form_getLevel = new WWWForm();
        form_getChar.AddField("uid", uid);
        form_getChar.AddField("charName", charUsingName);

        using (UnityWebRequest www = UnityWebRequest.Post(addressGetLevelByCharNameAndUid, form_getChar))
        {
            var handler = www.SendWebRequest();
            if (!handler.isDone)
            {
            }
            yield return handler;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                LevelID level = LevelJson(www.downloadHandler.text);
                Debug.Log("level: " + level);
                hp = level.hp;
                damage = level.damage;
                curHp = hp;
                Debug.Log("hp: " + level.hp + " damage: " + level.damage);
                HealthBar.fillAmount = 1;
            }
        }

    }

    public LevelID LevelJson(string a)
    {
        var level = JsonConvert.DeserializeObject<LevelID>(a);
        return level;
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }


    public void Damage(int damage)
    {
        curHp -= damage;
        Debug.Log("hp: " + curHp);
        HealthBar.fillAmount = (float)curHp/hp;
        Debug.Log("hp1: " + curHp / hp);
    }
}

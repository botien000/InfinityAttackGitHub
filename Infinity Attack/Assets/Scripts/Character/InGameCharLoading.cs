using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Cinemachine;
using TMPro;

public class InGameCharLoading : MonoBehaviour
{
    [SerializeField] private GameObject CharacterSpawnPosition;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image Avatar;
    [SerializeField] private TMP_Text HealthText;

    private string addressGetUsingCharName = Api.Instance.api + Api.Instance.routerGetUsingCharNameById;
    private string addressGetLevelByCharNameAndUid = Api.Instance.api + Api.Instance.routerGetLevelByCharNameAndUid;

    private string charUsingName;
    public int damage;
    private int hp;
    public int curHp;

    private Sprite fire_knightsprite;
    private Sprite ground_monksprite;
    private Sprite leaf_rangersprite;
    private Sprite water_priestesssprite;
    private Sprite metal_bladekeepersprite;
    private Sprite wind_hashashinsprite;

    [SerializeField] private Button btnCooldown;

    [SerializeField] private CinemachineVirtualCamera camera;
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
        curHp = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator GetData()
    {
        // l?y th�ng tin t??ng ?ang s? d?ng
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
                LoadAvatars();
                if (name == "Fire Knight")
                {
                    Avatar.GetComponent<Image>().sprite = fire_knightsprite;
                    GameObject fire_knight = (GameObject)Instantiate(Resources.Load("Prefabs/Character/Fire_Knight"), CharacterSpawnPosition.transform.position, Quaternion.identity);
                    camera.Follow = fire_knight.transform;
                    fire_knight.GetComponent<CharacterObject>().insertBtnCooldown(btnCooldown);
                }
                else if (name == "Ground Monk")
                {
                    Avatar.GetComponent<Image>().sprite = fire_knightsprite;
                    GameObject fire_knight = (GameObject)Instantiate(Resources.Load("Prefabs/Character/Fire_Knight"), CharacterSpawnPosition.transform.position, Quaternion.identity);
                    camera.Follow = fire_knight.transform;
                    fire_knight.GetComponent<CharacterObject>().insertBtnCooldown(btnCooldown);
                    //b? d�ng n�y sau khi thi?t k? xong 2 nh�n v?t cu?i
                    charUsingName = "Fire Knight";
                }
                else if (name == "Leaf Ranger")
                {
                    Avatar.GetComponent<Image>().sprite = fire_knightsprite;
                    GameObject fire_knight = (GameObject)Instantiate(Resources.Load("Prefabs/Character/Fire_Knight"), CharacterSpawnPosition.transform.position, Quaternion.identity);
                    camera.Follow = fire_knight.transform;
                    fire_knight.GetComponent<CharacterObject>().insertBtnCooldown(btnCooldown);
                    //b? d�ng n�y sau khi thi?t k? xong 2 nh�n v?t cu?i
                    charUsingName = "Fire Knight";
                }
                else if (name == "Metal Bladekeeper")
                {
                    Avatar.GetComponent<Image>().sprite = metal_bladekeepersprite;
                    GameObject metal_bladekeeper = (GameObject)Instantiate(Resources.Load("Prefabs/Character/Metal_Bladekeeper"), CharacterSpawnPosition.transform.position, Quaternion.identity);
                    camera.Follow = metal_bladekeeper.transform;
                    metal_bladekeeper.GetComponent<CharacterObject>().insertBtnCooldown(btnCooldown);
                }
                else if (name == "Water Priestess")
                {
                    Avatar.GetComponent<Image>().sprite = water_priestesssprite;
                    GameObject water_priestess = (GameObject)Instantiate(Resources.Load("Prefabs/Character/Water_Priestess"), CharacterSpawnPosition.transform.position, Quaternion.identity);
                    camera.Follow = water_priestess.transform;
                    water_priestess.GetComponent<CharacterObject>().insertBtnCooldown(btnCooldown);
                }
                else if (name == "Wind Hashashin")
                {
                    Avatar.GetComponent<Image>().sprite = wind_hashashinsprite;
                    GameObject wind_hashashin = (GameObject)Instantiate(Resources.Load("Prefabs/Character/Wind_Hashashin"), CharacterSpawnPosition.transform.position, Quaternion.identity);
                    camera.Follow = wind_hashashin.transform;
                    wind_hashashin.GetComponent<CharacterObject>().insertBtnCooldown(btnCooldown);
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
                HealthText.text = "" + curHp + "/" + hp;
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
        if(CharacterObject.instance.isUltimate == false && curHp>0)
        {
            if (curHp > 0)
            {
                curHp -= damage;
                Debug.Log("hp: " + curHp);
                HealthBar.fillAmount = (float)curHp / hp;
                Debug.Log("hp1: " + curHp / hp);
                HealthText.text = "" + curHp + "/" + hp;
            } else if(curHp <= 0)
            {
                curHp = 0;
                HealthText.text = "0" + "/" + hp;
            }
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
}

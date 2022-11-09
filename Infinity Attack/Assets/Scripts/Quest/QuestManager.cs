using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    private Api instanceIP;    

    private int login = 0, single3time = 0, multi1time = 0, kill50enemy = 0, kill5boss = 0, use3spell = 0;
    private int totalGift = 0;
    private Sprite successSprite;
    private Sprite processSprite;

    [SerializeField] private Quest[] questOwnList;
   
    void Start()
    {
        instanceIP = Api.Instance;
        LoadAvatars();
        StartCoroutine(GetQuestOwnData(instanceIP.api + instanceIP.routerPostQuestsOwn));
    }
    private void LoadAvatars()
    {
        successSprite = Resources.Load<Sprite>("Quest/success");
        processSprite = Resources.Load<Sprite>("Quest/process");
    }
    void ProcessServerResponse(string rawResponse)
    {
        var _quest = JsonConvert.DeserializeObject<Quest[]>(rawResponse);
        questOwnList = _quest;
        GameObject g;
        GameObject item = transform.GetChild(0).gameObject;
        item.SetActive(true);
        for (int i = 0; i < questOwnList.Length; i++)
        {
            g = Instantiate(item, transform);
            string name = questOwnList[i].questID.name;
            int point = questOwnList[i].questID.point;
            string des = questOwnList[i].questID.description;
            int challengeAchieved = questOwnList[i].challengeAchieved;
            int challenge = questOwnList[i].questID.challenge;
            int status = questOwnList[i].status;
            if (status == 0 && challengeAchieved < challenge)
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = processSprite;
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = point + "";
                g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = des;
                g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = challengeAchieved + "/" + challenge;
                g.transform.GetChild(4).GetComponent<Button>().interactable = false;
                g.transform.GetChild(4).GetComponent<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
            }
            else if (status == 0 && challengeAchieved >= challenge)
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = processSprite;
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = point + "";
                g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = des;
                g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = challenge + "/" + challenge;
                g.transform.GetChild(4).GetComponent<Button>().AddEventListener(i, Claim);
            }
            else if (status == 1)
            {
                g.transform.GetChild(0).GetComponent<Image>().sprite = successSprite;
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = point + "";
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.gray;
                g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = des;
                g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.gray;
                g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = challenge + "/" + challenge;
                g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.gray;
                g.transform.GetChild(4).GetComponent<Button>().interactable = false;
                g.transform.GetChild(4).GetComponent<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
            }
            if (name == "login")
            {
                login = point;
            }
            else if (name == "single3time")
            {
                single3time = point;
            }
            else if (name == "multi1time")
            {
                multi1time = point;
            }
            else if (name == "kill50enemy")
            {
                kill50enemy = point;
            }
            else if (name == "kill5boss")
            {
                kill5boss = point;
            }
            else if (name == "use3spell")
            {
                use3spell = point;
            }
        }
        SavePoint(login, single3time, multi1time, kill50enemy, kill5boss, use3spell);
        LoadPoint();
        totalGift = 0;
        SaveTotal(totalGift);
        LoadTotal();
        for (int a = 0; a < questOwnList.Length; a++)
        {
            string name = questOwnList[a].questID.name;
            if (questOwnList[a].status == 1)
            {
                if (name == "login")
                {
                    totalGift += login;
                }
                else if (name == "single3time")
                {
                    totalGift += single3time;
                }
                else if (name == "multi1time")
                {
                    totalGift += multi1time;
                }
                else if (name == "kill50enemy")
                {
                    totalGift += kill50enemy;
                }
                else if (name == "kill5boss")
                {
                    totalGift += kill5boss;
                }
                else if (name == "use3spell")
                {
                    totalGift += use3spell;
                }
            }
            SaveTotal(totalGift);
            //Debug.Log("total gift: " + totalGift);
        }
        item.SetActive(false);
    }

    void Claim(int itemIndex)
    {   
        Quest quest = questOwnList[itemIndex];
        string id = quest._id;
        StartCoroutine(ChangeStatusQuestOwn(instanceIP.api + instanceIP.routerUpdateStatusQuestOwn, id));
    }

    private void LoadPoint()
    {
        login = PlayerPrefs.GetInt("loginPoint");
        single3time = PlayerPrefs.GetInt("single3time");
        multi1time = PlayerPrefs.GetInt("multi1time");
        kill50enemy = PlayerPrefs.GetInt("kill50enemy");
        kill5boss = PlayerPrefs.GetInt("kill5boss");
        use3spell = PlayerPrefs.GetInt("use3spell");
    }

    private void SavePoint(int login, int single3time, int multi1time, int kill50enemy, int kill5boss, int use3spell)
    {
        PlayerPrefs.SetInt("loginPoint", login);
        PlayerPrefs.SetInt("single3time", single3time);
        PlayerPrefs.SetInt("multi1time", multi1time);
        PlayerPrefs.SetInt("kill50enemy", kill50enemy);
        PlayerPrefs.SetInt("kill5boss", kill5boss);
        PlayerPrefs.SetInt("use3spell", use3spell);
    }

    private void LoadTotal()
    {
        totalGift = PlayerPrefs.GetInt("totalGift");
    }

    private void SaveTotal(int totalGift)
    {
        PlayerPrefs.SetInt("totalGift", totalGift);
    }
    IEnumerator ChangeStatusQuestOwn(string address, string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("questOwnID", id);
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
            GameObject[] gclone = GameObject.FindGameObjectsWithTag("Mission");
            foreach(GameObject go in gclone)
            {
               if(go.name.Equals("Mission(Clone)"))
               {
                  Destroy(go);
               }
            }
            StartCoroutine(GetQuestOwnData(instanceIP.api + instanceIP.routerPostQuestsOwn));
        }
    }
    IEnumerator GetQuestOwnData(string address)
    {
        //string userID = PlayerPrefs.GetString("uID");
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
            ProcessServerResponse(res);
            Debug.Log("Quest res: " + res);
            www.Dispose();
        }
    }


    
}



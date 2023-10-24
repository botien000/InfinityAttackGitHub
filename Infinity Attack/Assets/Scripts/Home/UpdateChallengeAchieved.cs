using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class UpdateChallengeAchieved : MonoBehaviour
{
    private int play = 0;

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject notiQuest;
    [SerializeField] private GameObject notiGift;
    [SerializeField] private GameObject notiAchieve;
    private Api instanceIP;
    private int login = 0, single3time = 0, multi1time = 0, kill50enemy = 0, kill5boss = 0, use3spell = 0;
    private int challengeAchievedLogIn = 0, challengeAchievedSingle3Time = 0, challengeAchievedMulti1Time = 0,
      challengeAchievedKill50Enemy = 0, challengeAchievedKill5Boss = 0, challengeAchievedUse3Spell = 0;
    private int totalGift = 0;

    private int characterown = 0, killenemy = 0, killboss = 0, singleplay = 0, multiplay = 0, addfriend = 0;
    private Character[] charList;
    private Quest[] questOwnList;
    private Achievement[] achievementOwnList;
    private Gift[] giftOwnList;
    private FriendID[] friendList;

    private bool finishQuest = false;
    private bool finishAchieve = false;
    private bool moc1 = false;
    private bool moc2 = false;
    private bool moc3 = false;
    private bool moc4 = false;
    // Start is called before the first frame update
    void Start()
    {
        instanceIP = Api.Instance;
        if (PlayerPrefs.HasKey("Play"))
        {
            LoadPlay();
            if (play == 1)
            {                             
                if (PlayerPrefs.HasKey("UID"))
                {
                    string userID = removeQuotes(PlayerPrefs.GetString("UID"));
                    LoadChallengeAchievedLogIn();
                    if (challengeAchievedLogIn == 0)
                    {
                        challengeAchievedLogIn = 1;
                        SaveChallengeAchievedLogIn(challengeAchievedLogIn);
                        StartCoroutine(UpdateAllChallengeAchievedQuest(instanceIP.api + instanceIP.routerUpdateAllChallengeAchievedQuestByName, userID));
                    }
                    else
                    {
                        StartCoroutine(UpdateAllChallengeAchievedQuest(instanceIP.api + instanceIP.routerUpdateAllChallengeAchievedQuestByName, userID));
                    }
                }
            }
            else // not do yet anything in game 
            {
                if (PlayerPrefs.HasKey("UID"))
                {
                    string userID = removeQuotes(PlayerPrefs.GetString("UID"));
                    StartCoroutine(GetQuestOwnData(instanceIP.api + instanceIP.routerPostQuestsOwn, userID));
                }
            }
        }
    }

    IEnumerator UpdateAllChallengeAchievedQuest(string address, string userID)
    {
        LoadChallengeAchievedLogIn();
        LoadChallengeAchievedSingle3Time();
        LoadChallengeAchievedMulti1Time();
        LoadChallengeAchievedKill50Enemy();
        LoadChallengeAchievedKill5Boss();
        LoadChallengeAchievedUse3Spell();
        string loginName = "login";
        string single3timeName = "single3time";
        string multi1timeName = "multi1time";
        string kill50enemyName = "kill50enemy";
        string kill5bossName = "kill5boss";
        string use3spellName = "use3spell";
        WWWForm form = new WWWForm();
        form.AddField("login", loginName);
        form.AddField("single3time", single3timeName);
        form.AddField("multi1time", multi1timeName);
        form.AddField("kill50enemy", kill50enemyName);
        form.AddField("kill5boss", kill5bossName);
        form.AddField("use3spell", use3spellName);

        Debug.Log($"challengeAchievedKill5Boss: {challengeAchievedKill5Boss}   challengeAchievedKill50Enemy: {challengeAchievedKill50Enemy}   challengeAchievedUse3Spell: {challengeAchievedUse3Spell}");
        form.AddField("challengeAchievedLogIn", challengeAchievedLogIn);
        form.AddField("challengeAchievedSingle3Time", challengeAchievedSingle3Time);
        form.AddField("challengeAchievedMulti1Time", challengeAchievedMulti1Time);
        form.AddField("challengeAchievedKill50Enemy", challengeAchievedKill50Enemy);
        form.AddField("challengeAchievedKill5Boss", challengeAchievedKill5Boss);
        form.AddField("challengeAchievedUse3Spell", challengeAchievedUse3Spell);
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
            StartCoroutine(GetCharacterOwnData(instanceIP.api + instanceIP.routerPostCharactersOwn, userID));
            www.Dispose();
        }
    }

    IEnumerator GetCharacterOwnData(string address, string userID)
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
            AchievementCharacterOwn(res, userID);
            www.Dispose();
        }
    }

    void AchievementCharacterOwn(string rawResponse, string userID)
    {
        var _char = JsonConvert.DeserializeObject<Character[]>(rawResponse);
        charList = _char;
        characterown = charList.Length;
        StartCoroutine(GetAchievementAddFriendOwn(instanceIP.api + instanceIP.routerGetAllFriends, characterown, userID));
    }

    IEnumerator GetAchievementAddFriendOwn(string address, int characterown, string userID)
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
            AchievementAddFriendOwn(res, characterown, userID);
            www.Dispose();
        }
    }
    void AchievementAddFriendOwn(string rawResponse,int characterown, string userID)
    {
        var _friend = JsonConvert.DeserializeObject<FriendID[]>(rawResponse);
        friendList = _friend;
        addfriend = friendList.Length;
        Debug.Log("FriendList: " + addfriend);
        StartCoroutine(UpdateAllChallengeAchievedAchievement(instanceIP.api + instanceIP.routerUpdateAllChallengeAchievedAchievementByName, characterown, addfriend, userID));
    }
    IEnumerator UpdateAllChallengeAchievedAchievement(string address, int characterown, int addfriend, string userID)
    {
        LoadChallengeAchievedKillEnemy();
        LoadChallengeAchievedKillBoss();
        LoadChallengeAchievedSinglePlay();
        LoadChallengeAchievedMultiPlay();
        string characterownName = "characterown";
        string killenemyName = "killenemy";
        string killbossName = "killboss";
        string singleplayName = "singleplay";
        string multiplayName = "multiplay";
        string addfriendName = "addfriend";
        WWWForm form = new WWWForm();
        form.AddField("characterown", characterownName);
        form.AddField("killenemy", killenemyName);
        form.AddField("killboss", killbossName);
        form.AddField("singleplay", singleplayName);
        form.AddField("multiplay", multiplayName);
        form.AddField("addfriend", addfriendName);

        form.AddField("challengeAchievedCharacterOwn", characterown);
        form.AddField("challengeAchievedKillEnemy", killenemy);
        form.AddField("challengeAchievedKillBoss", killboss);
        form.AddField("challengeAchievedSinglePlay", singleplay);
        form.AddField("challengeAchievedMultiPlay", multiplay);
        form.AddField("challengeAchievedAddFriend", addfriend);
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
            Debug.Log("Update challengeAchieved completed!");
            StartCoroutine(GetQuestOwnData(instanceIP.api + instanceIP.routerPostQuestsOwn, userID));          
            www.Dispose();
        }
    }

    IEnumerator GetQuestOwnData(string address, string userID)
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
            ProcessQuestResponse(res, userID);
            www.Dispose();
        }
    }

    void ProcessQuestResponse(string rawResponse, string userID)
    {
        var _quest = JsonConvert.DeserializeObject<Quest[]>(rawResponse);
        questOwnList = _quest;      
        for (int i = 0; i < questOwnList.Length; i++)
        {
            string name = questOwnList[i].questID.name;
            int point = questOwnList[i].questID.point;
            int challengeAchieved = questOwnList[i].challengeAchieved;
            int challenge = questOwnList[i].questID.challenge;
            int status = questOwnList[i].status;

            if (status == 0 && challengeAchieved >= challenge)
            {
                finishQuest = true;
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
        totalGift = 0;
        SaveTotal(totalGift);
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
        }
            StartCoroutine(GetAchievementOwnData(instanceIP.api + instanceIP.routerPostAchievementsOwn, userID));
    }

    IEnumerator GetAchievementOwnData(string address, string userID)
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
            ProcessAchieveResponse(res, userID);
            www.Dispose();
        }
    }

    void ProcessAchieveResponse(string rawResponse, string userID)
    {
        var _achievement = JsonConvert.DeserializeObject<Achievement[]>(rawResponse);
        achievementOwnList = _achievement;
        for (int i = 0; i < achievementOwnList.Length; i++)
        {            
            int challengeAchieved = achievementOwnList[i].challengeAchieved;
            int challenge = achievementOwnList[i].achievementLevelID.challenge;
            if (challengeAchieved >= challenge)
            {
                finishAchieve =  true;
            }
        }
        StartCoroutine(GetGiftsOwnData(instanceIP.api + instanceIP.routerPostGiftsOwn, userID));
    }

    IEnumerator GetGiftsOwnData(string address, string userID)
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
            GiftOwnDataRespond(res);
            www.Dispose();
        }
    }

    void GiftOwnDataRespond(string rawResponse)
    {
        var _gift = JsonConvert.DeserializeObject<Gift[]>(rawResponse);
        giftOwnList = _gift;
        for (int i = 0; i < giftOwnList.Length; i++)
        {
            string name = giftOwnList[i].giftID.name;
            int status = giftOwnList[i].status;
            if(status == 0)
            {
                if (PlayerPrefs.HasKey("totalGift"))
                {
                    if (name == "moc1" && totalGift < 25)
                    {
                        moc1 = false;
                    }
                    else if (name == "moc1" && totalGift >= 25)
                    {
                        moc1 = true;
                    }
                    else if (name == "moc2" && totalGift < 50)
                    {
                        moc2 = false;
                    }
                    else if (name == "moc2" && totalGift >= 50)
                    {
                        moc2 = true;
                    }
                    else if (name == "moc3" && totalGift < 75)
                    {
                        moc3 = false;
                    }
                    else if (name == "moc3" && totalGift >= 75)
                    {
                        moc3 = true;
                    }
                    else if (name == "moc4" && totalGift < 100)
                    {
                        moc4 = false;
                    }
                    else if (name == "moc4" && totalGift == 100)
                    {
                        moc4 = true;
                    }
                }
            }           
        }
        checkAndFinishLoad();
    }
    private void checkAndFinishLoad()
    {
        if (finishQuest)
        {
            notiQuest.SetActive(true);
        }
        else
        {
            notiQuest.SetActive(false);
        }

        if(moc1 || moc2 || moc3 || moc4)
        {
            notiGift.SetActive(true);
        }
        else if(!moc1 && !moc2 && !moc3 && !moc4)
        {
            notiGift.SetActive(false);
        }

        if (finishAchieve)
        {
            notiAchieve.SetActive(true);
        }
        else
        {
            notiAchieve.SetActive(false);
        }

        SavePlay(0);
        Debug.Log("Load Finish Completed!");
    }
    private void LoadPlay()
    {
        play = PlayerPrefs.GetInt("Play");
    }

    private void SavePlay(int play)
    {
        PlayerPrefs.SetInt("Play", play);
    }
    
    private void LoadChallengeAchievedLogIn()
    {
        challengeAchievedLogIn = PlayerPrefs.GetInt("challengeAchievedLogIn");
    }

    private void SaveChallengeAchievedLogIn(int challengeAchievedLogIn)
    {
        PlayerPrefs.SetInt("challengeAchievedLogIn", challengeAchievedLogIn);
    }

    private void LoadChallengeAchievedSingle3Time()
    {
        challengeAchievedSingle3Time = PlayerPrefs.GetInt("challengeAchievedSingle3Time");
    }

    private void LoadChallengeAchievedMulti1Time()
    {
        challengeAchievedMulti1Time = PlayerPrefs.GetInt("challengeAchievedMulti1Time");
    }

    private void LoadChallengeAchievedKill50Enemy()
    {
        challengeAchievedKill50Enemy = PlayerPrefs.GetInt("challengeAchievedKill50Enemy");
    }

    private void LoadChallengeAchievedKill5Boss()
    {
        challengeAchievedKill5Boss = PlayerPrefs.GetInt("challengeAchievedKill5Boss");
    }

    private void LoadChallengeAchievedUse3Spell()
    {
        challengeAchievedUse3Spell = PlayerPrefs.GetInt("challengeAchievedUse3Spell");
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }

    private void LoadChallengeAchievedKillEnemy()
    {
        killenemy = PlayerPrefs.GetInt("killenemy");
    }

    private void LoadChallengeAchievedKillBoss()
    {
        killboss = PlayerPrefs.GetInt("killboss");
    }

    private void LoadChallengeAchievedSinglePlay()
    {
        singleplay = PlayerPrefs.GetInt("singleplay");
    }

    private void LoadChallengeAchievedMultiPlay()
    {
        multiplay = PlayerPrefs.GetInt("multiplay");
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

    private void SaveTotal(int totalGift)
    {
        PlayerPrefs.SetInt("totalGift", totalGift);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class LoginScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private GameObject AlertPannel;
    [SerializeField] private GameObject RegisterPannel;
    [SerializeField] private GameObject FlagPannel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private TMP_Text AlertText;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Toggle tglLogin;

    [SerializeField] private Quest[] questOwnList;
    [SerializeField] private Achievement[] achievementOwnList;
    private Api instanceIP;
    private string loginApi = Api.Instance.api + Api.Instance.routerLogin;
    private string generateTokenApi = Api.Instance.api + Api.Instance.routerGenerateRememberToken;
    private int challengeAchievedLogIn = 0, challengeAchievedSingle3Time = 0, challengeAchievedMulti1Time = 0,
      challengeAchievedKill50Enemy = 0, challengeAchievedKill5Boss = 0, challengeAchievedUse3Spell = 0;

    private int killenemy = 0, killboss = 0, singleplay = 0, multiplay = 0, addfriend = 0;

    public void OnLoginClick()
    {
        SoundManager.instance.SetSoundClick();
        //createPlayerPref();
        instanceIP = Api.Instance;
        StartCoroutine(TryLogin());
    }

    //private void createPlayerPref()
    //{
    //    SaveChallengeAchievedLogIn(0);
    //    SaveChallengeAchievedSingle3Time(0);
    //    SaveChallengeAchievedMulti1Time(0);
    //    SaveChallengeAchievedKill50Enemy(0);
    //    SaveChallengeAchievedKill5Boss(0);
    //    SaveChallengeAchievedUse3Spell(0);

    //    SaveChallengeAchievedKillEnemy(0);
    //    SaveChallengeAchievedKillBoss(0);
    //    SaveChallengeAchievedSinglePlay(0);
    //    SaveChallengeAchievedMultiPlay(0);
    //    SaveChallengeAchievedAddFriend(0);
    //}
    private IEnumerator TryLogin()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (username.Trim() == "" || password.Trim() == "")
        {
            AlertText.text = "Username and password cannot be empty";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);

            using (UnityWebRequest www = UnityWebRequest.Post(loginApi, form))
            {
                LoadingPanel.SetActive(true);
                yield return www.SendWebRequest();
                LoadingPanel.SetActive(false);
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                    string respone = removeQuotes(www.downloadHandler.text);
                    if (respone == "Account or password error")
                    {
                        AlertText.text = respone;
                        AlertPannel.gameObject.SetActive(true);
                        FlagPannel.gameObject.SetActive(true);
                    }
                    else
                    {
                        PlayerPrefs.SetString("UID", www.downloadHandler.text);
                        string userID = removeQuotes(PlayerPrefs.GetString("UID"));
                        CheckSavePref(userID);
                        StartCoroutine(GetQuestOwnData(instanceIP.api + instanceIP.routerPostQuestsOwn, userID));
                    }
                }
            }
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
            ProcessQuestOwnResponse(res);
            Debug.Log("Quest res: " + res);
            www.Dispose();
        }
    }

    void ProcessQuestOwnResponse(string rawResponse)
    {
        var _quest = JsonConvert.DeserializeObject<Quest[]>(rawResponse);
        questOwnList = _quest;
        string userID = questOwnList[0].userID._id.ToString();
        for (int i = 0; i < questOwnList.Length; i++)
        {
            string name = questOwnList[i].questID.name;
            int challengeAchieved = questOwnList[i].challengeAchieved;

            if (name == "login")
            {
                challengeAchievedLogIn = challengeAchieved;
                SaveChallengeAchievedLogIn(challengeAchievedLogIn);
            }
            else if (name == "single3time")
            {
                challengeAchievedSingle3Time = challengeAchieved;
                SaveChallengeAchievedSingle3Time(challengeAchievedSingle3Time);
            }
            else if (name == "multi1time")
            {
                challengeAchievedMulti1Time = challengeAchieved;
                SaveChallengeAchievedMulti1Time(challengeAchievedMulti1Time);
            }
            else if (name == "kill50enemy")
            {
                challengeAchievedKill50Enemy = challengeAchieved;
                SaveChallengeAchievedKill50Enemy(challengeAchievedKill50Enemy);
            }
            else if (name == "kill5boss")
            {
                challengeAchievedKill5Boss = challengeAchieved;
                SaveChallengeAchievedKill5Boss(challengeAchievedKill5Boss);
            }
            else if (name == "use3spell")
            {
                challengeAchievedUse3Spell = challengeAchieved;
                SaveChallengeAchievedUse3Spell(challengeAchievedUse3Spell);
            }
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
            ProcessAchievementResponse(res);
            Debug.Log("Achievement res: " + res);
            www.Dispose();
        }
    }

    void ProcessAchievementResponse(string rawResponse)
    {
        var _achievement = JsonConvert.DeserializeObject<Achievement[]>(rawResponse);
        achievementOwnList = _achievement;

        for (int i = 0; i < achievementOwnList.Length; i++)
        {
            string name = achievementOwnList[i].achievementID.name;
            int challengeAchieved = achievementOwnList[i].challengeAchieved;

            if (name == "killenemy")
            {
                killenemy = challengeAchieved;
                SaveChallengeAchievedKillEnemy(killenemy);
            }
            else if (name == "killboss")
            {
                killboss = challengeAchieved;
                SaveChallengeAchievedKillBoss(killboss);
            }
            else if (name == "singleplay")
            {
                singleplay = challengeAchieved;
                SaveChallengeAchievedSinglePlay(singleplay);
            }
            else if (name == "multiplay")
            {
                multiplay = challengeAchieved;
                SaveChallengeAchievedMultiPlay(multiplay);
            }
            else if (name == "addfriend")
            {
                addfriend = challengeAchieved;
                SaveChallengeAchievedAddFriend(addfriend);
            }
        }
        ResetSpellChosen();
        StartCoroutine(IELoadingScreen(1));
    }
    private void ResetSpellChosen()
    {
        for (int i = 0; i < 3; i++)
        {
            SpellSingleton.Instance.SetSpell(i, null, null, 0, 0, 0,null);
        }
    }
    private void CheckSavePref(string uid)
    {
        if (tglLogin.isOn)
        {
            StartCoroutine(GenerateToken(uid));
        }
        else
        {
            PlayerPrefs.SetString("token", "");
        }
    }
    IEnumerator GenerateToken(string uid)
    {
        WWWForm form = new WWWForm();
        form.AddField("uid", uid);
        UnityWebRequest www = UnityWebRequest.Post(generateTokenApi, form);
        loadingPanel.SetActive(true);
        yield return www.SendWebRequest();
        loadingPanel.SetActive(false);
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Something went wrong: " + www.error);
        }
        else
        {
            if (removeQuotes(www.downloadHandler.text) == null)
            {

            }
            else
            {
                string token = removeQuotes(www.downloadHandler.text);
                PlayerPrefs.SetString("token", token);
                Debug.Log(token);
                www.Dispose();
            }
        }
    }
    IEnumerator IELoadingScreen(int buildIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(buildIndex);
        while (!asyncOperation.isDone)
        {
            LoadingPanel.SetActive(true);
            yield return null;
        }
    }
    public void resetInputField()
    {
        SoundManager.instance.SetSoundClick();
        usernameField.text = "";
        passwordField.text = "";
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
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

    private void SaveChallengeAchievedSingle3Time(int challengeAchievedSingle3Time)
    {
        PlayerPrefs.SetInt("challengeAchievedSingle3Time", challengeAchievedSingle3Time);
    }
    private void LoadChallengeAchievedMulti1Time()
    {
        challengeAchievedMulti1Time = PlayerPrefs.GetInt("challengeAchievedMulti1Time");
    }

    private void SaveChallengeAchievedMulti1Time(int challengeAchievedMulti1Time)
    {
        PlayerPrefs.SetInt("challengeAchievedMulti1Time", challengeAchievedMulti1Time);
    }
    private void LoadChallengeAchievedKill50Enemy()
    {
        challengeAchievedKill50Enemy = PlayerPrefs.GetInt("challengeAchievedKill50Enemy");
    }

    private void SaveChallengeAchievedKill50Enemy(int challengeAchievedKill50Enemy)
    {
        PlayerPrefs.SetInt("challengeAchievedKill50Enemy", challengeAchievedKill50Enemy);
    }
    private void LoadChallengeAchievedKill5Boss()
    {
        challengeAchievedKill5Boss = PlayerPrefs.GetInt("challengeAchievedKill5Boss");
    }

    private void SaveChallengeAchievedKill5Boss(int challengeAchievedKill5Boss)
    {
        PlayerPrefs.SetInt("challengeAchievedKill5Boss", challengeAchievedKill5Boss);
    }

    private void LoadChallengeAchievedUse3Spell()
    {
        challengeAchievedUse3Spell = PlayerPrefs.GetInt("challengeAchievedUse3Spell");
    }

    private void SaveChallengeAchievedUse3Spell(int challengeAchievedUse3Spell)
    {
        PlayerPrefs.SetInt("challengeAchievedUse3Spell", challengeAchievedUse3Spell);
    }


    private void LoadChallengeAchievedKillEnemy()
    {
        killenemy = PlayerPrefs.GetInt("killenemy");
    }

    private void SaveChallengeAchievedKillEnemy(int killenemy)
    {
        PlayerPrefs.SetInt("killenemy", killenemy);
    }
    private void LoadChallengeAchievedKillBoss()
    {
        killboss = PlayerPrefs.GetInt("killboss");
    }

    private void SaveChallengeAchievedKillBoss(int killboss)
    {
        PlayerPrefs.SetInt("killboss", killboss);
    }
    private void LoadChallengeAchievedSinglePlay()
    {
        singleplay = PlayerPrefs.GetInt("singleplay");
    }

    private void SaveChallengeAchievedSinglePlay(int singleplay)
    {
        PlayerPrefs.SetInt("singleplay", singleplay);
    }
    private void LoadChallengeAchievedMultiPlay()
    {
        multiplay = PlayerPrefs.GetInt("multiplay");
    }

    private void SaveChallengeAchievedMultiPlay(int multiplay)
    {
        PlayerPrefs.SetInt("multiplay", multiplay);
    }
    private void LoadChallengeAchievedAddFriend()
    {
        addfriend = PlayerPrefs.GetInt("addfriend");
    }

    private void SaveChallengeAchievedAddFriend(int addfriend)
    {
        PlayerPrefs.SetInt("addfriend", addfriend);
    }
}

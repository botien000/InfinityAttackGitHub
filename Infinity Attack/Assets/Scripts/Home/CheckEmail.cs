using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class CheckEmail : MonoBehaviour
{
    private UserID user;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject AlertPanel;
    [SerializeField] private TMP_Text AlertText;
    [SerializeField] private GameObject FlagConfirmPanel;

    [SerializeField] private GameObject AddEmailPanel;
    [SerializeField] private GameObject ChangeEmailPanel;
    [SerializeField] private Image imgMusicOn_Off, imgSoundOn_Off;

    private string addressGetUser = Api.Instance.api + Api.Instance.routerGetUserById;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetData());

        imgMusicOn_Off.gameObject.SetActive(PlayerPrefs.GetFloat("MusicKey", 1) == 1 ? false : true);
        imgSoundOn_Off.gameObject.SetActive(PlayerPrefs.GetFloat("SoundKey", 1) == 1 ? false : true);
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerPrefs.SetFloat("MusicKey", 1);
        //PlayerPrefs.SetFloat("SoundKey", 1);
    }

    public void getDataAgain()
    {
        StartCoroutine(GetData());
    }

    public UserID UserJson(string a)
    {
        var user = JsonConvert.DeserializeObject<UserID>(a);
        return user;
    }

    private IEnumerator GetData()
    {
        // tìm thông tin chi tiết user và gán 
        WWWForm form_getUser = new WWWForm();
        string uid = removeQuotes(PlayerPrefs.GetString("UID"));
        form_getUser.AddField("id", uid);
        using (UnityWebRequest www = UnityWebRequest.Post(addressGetUser, form_getUser))
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
                user = UserJson(www.downloadHandler.text);
            }
        }
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }

    public void PressBtnMusic()
    {
        bool isActive = !imgMusicOn_Off.gameObject.activeInHierarchy;
        imgMusicOn_Off.gameObject.SetActive(isActive);
        SoundManager.instance.SetMusicData(!isActive);
    }

    public void PressBtnSound()
    {
        bool isActive = !imgSoundOn_Off.gameObject.activeInHierarchy;

        imgSoundOn_Off.gameObject.SetActive(isActive);
        SoundManager.instance.SetSoundData(!isActive);
    }
}

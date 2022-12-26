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
    [SerializeField] private TMP_Text btn_text;
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

    }

    public void getDataAgain()
    {
        StartCoroutine(GetData());
    }

    public void onEmailClick()
    {
        if(btn_text.text == "Add Email")
        {
            AddEmailPanel.gameObject.SetActive(true);
        } else if(btn_text.text == "Change Email")
        {
            ChangeEmailPanel.gameObject.SetActive(true);
        }
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
                if(user.email == null)
                {
                    btn_text.text = "Add Email";
                } else
                {
                    btn_text.text = "Change Email";
                }
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
        imgMusicOn_Off.gameObject.SetActive(!imgMusicOn_Off.gameObject.activeInHierarchy);
        SoundManager.instance.SetMusicData(!imgMusicOn_Off.gameObject.activeInHierarchy);
    }

    public void PressBtnSound()
    {
        imgSoundOn_Off.gameObject.SetActive(!imgSoundOn_Off.gameObject.activeInHierarchy);
        SoundManager.instance.SetSoundData(!imgSoundOn_Off.gameObject.activeInHierarchy) ;
    }
}

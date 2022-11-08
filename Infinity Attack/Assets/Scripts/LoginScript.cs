using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private GameObject AlertPannel;
    [SerializeField] private GameObject RegisterPannel;
    [SerializeField] private GameObject FlagPannel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private TMP_Text AlertText;

    public void OnLoginClick()
    {
        StartCoroutine(TryLogin());
    }
    private IEnumerator TryLogin()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (username.Trim() == "" || password.Trim() == "")
        {
            AlertText.text = "Không được bỏ trống";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/api/login", form))
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
                    if (respone == "Sai tài khoản hoặc mật khẩu")
                    {
                        AlertText.text = respone;
                        AlertPannel.gameObject.SetActive(true);
                        FlagPannel.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Player ID: " + respone);
                        //SceneManager.LoadScene(1);
                        StartCoroutine(IELoadingScreen(1));
                        PlayerPrefs.SetString("UID", www.downloadHandler.text);
                    }
                }
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
        usernameField.text = "";
        passwordField.text = "";
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }
}

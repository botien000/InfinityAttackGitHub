using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Net.Mail;
using System;

public class ForgotChangePassword : MonoBehaviour
{
    private string forgotChangePassApi = Api.Instance.api + Api.Instance.routerForgotChangePass;
    [SerializeField] private TMP_InputField tokenField;
    [SerializeField] private TMP_InputField newPassField;
    [SerializeField] private TMP_InputField confirmField;
    [SerializeField] private GameObject AlertPannel;
    [SerializeField] private GameObject FlagPannel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private TMP_Text AlertText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changePassClick()
    {
        StartCoroutine(ChangePass());
    }

    private IEnumerator ChangePass()
    {
        string token = tokenField.text;
        string newPass = newPassField.text;
        string confirmPass = confirmField.text;

        if (token.Trim() == "" || newPass.Trim() == "" || confirmPass.Trim() == "")
        {
            AlertText.text = "Fields cannot be empty";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else if (newPass.Length < 6 || newPass.Length > 24)
        {
            AlertText.text = "Password must have 6-24 characters";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else if (newPass != confirmPass)
        {
            AlertText.text = "Passwords do not match";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form_changePass = new WWWForm();
            form_changePass.AddField("token", token);
            form_changePass.AddField("newPass", newPass);

            using (UnityWebRequest www = UnityWebRequest.Post(forgotChangePassApi, form_changePass))
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
                    Debug.Log("respone: " + respone);
                    if (respone == "Code was expired or incorrect")
                    {
                        AlertText.text = respone;
                        AlertPannel.gameObject.SetActive(true);
                        FlagPannel.gameObject.SetActive(true);
                    } else if (respone == "Change password successfully")
                    {
                        ResetField();
                        AlertText.text = respone;
                        AlertPannel.gameObject.SetActive(true);
                        FlagPannel.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void ResetField()
    {
        tokenField.text = "";
        tokenField.text = "";
        tokenField.text = "";
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }
}

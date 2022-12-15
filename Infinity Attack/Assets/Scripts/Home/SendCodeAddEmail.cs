using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net.Mail;
using System;

public class SendCodeAddEmail : MonoBehaviour
{
    [SerializeField] private GameObject AlertPanel;
    [SerializeField] private TMP_Text AlertText;
    [SerializeField] private GameObject FlagConfirmPanel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject AddEmailPanel;
    [SerializeField] private GameObject SendCodeAddEmailPanel;

    [SerializeField] private TMP_InputField emailField;


    private string addressSendCodeAddEmail = Api.Instance.api + Api.Instance.routerSendCodeAddEmail;

    public bool IsEmail(string email)
    {
        try
        {
            MailAddress m = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    public void SendCodeAddEmailClick()
    {
        StartCoroutine(SendCode());
    }
    private IEnumerator SendCode()
    {
        string uid = removeQuotes(PlayerPrefs.GetString("UID"));
        string email = emailField.text;

        if (email.Trim() == "")
        {
            AlertText.text = "Field cannot be empty";
            AlertPanel.gameObject.SetActive(true);
            FlagConfirmPanel.gameObject.SetActive(true);
        }
        else if (IsEmail(email) == false)
        {
            AlertText.text = "Email invalidate";
            AlertPanel.gameObject.SetActive(true);
            FlagConfirmPanel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form_sendCode = new WWWForm();
            form_sendCode.AddField("uid", uid);
            form_sendCode.AddField("email", email);

            using (UnityWebRequest www = UnityWebRequest.Post(addressSendCodeAddEmail, form_sendCode))
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
                    Debug.Log(respone);
                    if (respone == "Email has already been used by another user")
                    {
                        AlertText.text = respone;
                        AlertPanel.gameObject.SetActive(true);
                        FlagConfirmPanel.gameObject.SetActive(true);
                    }
                    else if (respone == "Add email code was sent to your email address")
                    {
                        AddEmailPanel.gameObject.SetActive(true);
                        SendCodeAddEmailPanel.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }
}

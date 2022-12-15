using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Net.Mail;
using System;

public class ChangeEmail : MonoBehaviour
{
    [SerializeField] private GameObject AlertPanel;
    [SerializeField] private TMP_Text AlertText;
    [SerializeField] private GameObject FlagConfirmPanel;
    [SerializeField] private GameObject LoadingPanel;

    [SerializeField] private TMP_InputField codeField;
    [SerializeField] private TMP_InputField newEmailField;


    private string addressChangeEmail = Api.Instance.api + Api.Instance.routerChangeEmail;
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
    public void ChangeEmailClick()
    {
        StartCoroutine(Change());
    }
    private IEnumerator Change()
    {
        string token = codeField.text;
        string newEmail = newEmailField.text;

        if (token.Trim() == "")
        {
            AlertText.text = "Field cannot be empty";
            AlertPanel.gameObject.SetActive(true);
            FlagConfirmPanel.gameObject.SetActive(true);
        }
        else if (IsEmail(newEmail) == false)
        {
            AlertText.text = "Email invalidate";
            AlertPanel.gameObject.SetActive(true);
            FlagConfirmPanel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form_changeEmail = new WWWForm();
            form_changeEmail.AddField("token", token);
            form_changeEmail.AddField("newEmail", newEmail);

            using (UnityWebRequest www = UnityWebRequest.Post(addressChangeEmail, form_changeEmail))
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
                    if (respone == "Email has already been used by another user" || respone == "Code was expired or incorrect")
                    {
                        AlertText.text = respone;
                        AlertPanel.gameObject.SetActive(true);
                        FlagConfirmPanel.gameObject.SetActive(true);
                    } else if (respone == "Change email successfully")
                    {
                        AlertText.text = respone;
                        AlertPanel.gameObject.SetActive(true);
                        FlagConfirmPanel.gameObject.SetActive(true);
                        codeField.text = "";
                        newEmailField.text = "";
                    }
                    else
                    {
                        AlertText.text = "Something went wrong";
                        AlertPanel.gameObject.SetActive(true);
                        FlagConfirmPanel.gameObject.SetActive(true);
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

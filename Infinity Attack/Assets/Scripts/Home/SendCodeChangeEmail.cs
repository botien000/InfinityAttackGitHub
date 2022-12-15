using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net.Mail;
using System;

public class SendCodeChangeEmail : MonoBehaviour
{
    [SerializeField] private GameObject AlertPanel;
    [SerializeField] private TMP_Text AlertText;
    [SerializeField] private GameObject FlagConfirmPanel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject ChangeEmailPanel;
    [SerializeField] private GameObject SendCodeChangeEmailPanel;


    private string addressSendCodeChangeEmail = Api.Instance.api + Api.Instance.routerSendCodeChangeEmail;

    public void SendCodeChangeEmailClick()
    {
        StartCoroutine(SendCode());
    }
    private IEnumerator SendCode()
    {
        string uid = removeQuotes(PlayerPrefs.GetString("UID"));

            WWWForm form_sendCode = new WWWForm();
            form_sendCode.AddField("uid", uid);

            using (UnityWebRequest www = UnityWebRequest.Post(addressSendCodeChangeEmail, form_sendCode))
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
                    if (respone == "Change email code was sent to your email address")
                    {
                        ChangeEmailPanel.gameObject.SetActive(true);
                        SendCodeChangeEmailPanel.gameObject.SetActive(false);
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

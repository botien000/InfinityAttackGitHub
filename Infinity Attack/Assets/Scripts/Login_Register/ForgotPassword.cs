using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Net.Mail;
using System;

public class ForgotPassword : MonoBehaviour
{
    private string forgotApi = Api.Instance.api + Api.Instance.routerSendCodeForgot;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private GameObject AlertPannel;
    [SerializeField] private GameObject FlagPannel;
    [SerializeField] private GameObject ForgotPanel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private GameObject ForgotChangePassPannel;
    [SerializeField] private TMP_Text AlertText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void forgotClick()
    {
        StartCoroutine(SendCodeForgot());
    }

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

    private IEnumerator SendCodeForgot()
    {
        string email = emailField.text;

        if (email.Trim() == "")
        {
            AlertText.text = "Field cannot be empty";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else if (IsEmail(email) == false)
        {
            AlertText.text = "Email invalidate";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);

            using (UnityWebRequest www = UnityWebRequest.Post(forgotApi, form))
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
                    if (respone == "Account cannot be found with this email")
                    {
                        AlertText.text = respone;
                        AlertPannel.gameObject.SetActive(true);
                        FlagPannel.gameObject.SetActive(true);
                    }
                    else if (respone == "Change password code was sent to your email address")
                    {
                        ForgotChangePassPannel.gameObject.SetActive(true);
                        ForgotPanel.SetActive(false);
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

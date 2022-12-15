using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class AddEmail : MonoBehaviour
{
    [SerializeField] private GameObject AlertPanel;
    [SerializeField] private TMP_Text AlertText;
    [SerializeField] private GameObject FlagConfirmPanel;
    [SerializeField] private GameObject LoadingPanel;

    [SerializeField] private TMP_InputField codeField;


    private string addressAddEmail = Api.Instance.api + Api.Instance.routerAddEmail;
    public void AddEmailClick()
    {
        StartCoroutine(Add());
    }
    private IEnumerator Add()
    {
        string uid = removeQuotes(PlayerPrefs.GetString("UID"));
        string token = codeField.text;

        if (token.Trim() == "")
        {
            AlertText.text = "Field cannot be empty";
            AlertPanel.gameObject.SetActive(true);
            FlagConfirmPanel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form_addEmail = new WWWForm();
            form_addEmail.AddField("token", token);

            using (UnityWebRequest www = UnityWebRequest.Post(addressAddEmail, form_addEmail))
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
                    } else if (respone == "Add email successfully")
                    {
                        AlertText.text = respone;
                        AlertPanel.gameObject.SetActive(true);
                        FlagConfirmPanel.gameObject.SetActive(true);
                        codeField.text = "";
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

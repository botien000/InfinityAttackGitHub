using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChangePassword : MonoBehaviour
{
    [SerializeField] private GameObject AlertPanel;
    [SerializeField] private TMP_Text AlertText;
    [SerializeField] private GameObject FlagConfirmPanel;
    [SerializeField] private GameObject LoadingPanel;

    [SerializeField] private TMP_InputField passField;
    [SerializeField] private TMP_InputField newPassField;
    [SerializeField] private TMP_InputField confirmPassField;


    private string addressChangePassword = Api.Instance.api + Api.Instance.routerChangePassword;

    public void ChangePasswordClick()
    {
        StartCoroutine(ChangePass());
    }
    private IEnumerator ChangePass()
    {
        string uid = removeQuotes(PlayerPrefs.GetString("UID"));
        string pass = passField.text;
        string newPass = newPassField.text;
        string confirmPass = confirmPassField.text;

        if (pass.Trim() == "" || newPass.Trim() == "" || confirmPass.Trim() == "")
        {
            AlertText.text = "Fields cannot be empty";
            AlertPanel.gameObject.SetActive(true);
            FlagConfirmPanel.gameObject.SetActive(true);
        }
        else if (newPass.Length < 6 || newPass.Length > 24)
        {
            AlertText.text = "Password must have 6-24 characters";
            AlertPanel.gameObject.SetActive(true);
            FlagConfirmPanel.gameObject.SetActive(true);
        }
        else if (newPass != confirmPass)
        {
            AlertText.text = "Passwords do not match";
            AlertPanel.gameObject.SetActive(true);
            FlagConfirmPanel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form_changePass = new WWWForm();
            form_changePass.AddField("uid", uid);
            form_changePass.AddField("pass", pass);
            form_changePass.AddField("newPass", newPass);

            using (UnityWebRequest www = UnityWebRequest.Post(addressChangePassword, form_changePass))
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
                    if (respone == "Old password is not correct")
                    {
                        Debug.Log("Alo");
                        AlertText.text = respone;
                        AlertPanel.gameObject.SetActive(true);
                        FlagConfirmPanel.gameObject.SetActive(true);
                    }
                    else if (respone == "Change password successfully")
                    {
                        AlertText.text = respone;
                        AlertPanel.gameObject.SetActive(true);
                        FlagConfirmPanel.gameObject.SetActive(true);
                        passField.text = "";
                        newPassField.text = "";
                        confirmPassField.text = "";
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

    public void resetInputField()
    {
        passField.text = "";
        newPassField.text = "";
        confirmPassField.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField confirmpasswordField;
    [SerializeField] private GameObject AlertPannel;
    [SerializeField] private GameObject LoginPannel;
    [SerializeField] private GameObject RegisterPannel;
    [SerializeField] private GameObject FlagPannel;
    [SerializeField] private GameObject LoadingPanel;
    [SerializeField] private TMP_Text AlertText;

    private string registerApi = Api.Instance.api + Api.Instance.routerRegister;

    public void OnRegisterClick()
    {
        StartCoroutine(TryRegister());
    }
    private IEnumerator TryRegister()
    {
        string username = usernameField.text;
        string name = nameField.text;
        string password = passwordField.text;
        string confirm_password = confirmpasswordField.text;
        if (username.Trim() == "" || name.Trim() == "" || password.Trim() == "" || confirm_password.Trim() == "")
        {
            AlertText.text = "Fields cannot be empty";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else if (username.Length < 6 || username.Length > 24)
        {
            AlertText.text = "Username must have 6-24 characters";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else if (password.Length < 6 || password.Length > 24)
        {
            AlertText.text = "Password must have 6-24 characters";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else if (password != confirm_password)
        {
            AlertText.text = "Passwords do not match";
            AlertPannel.gameObject.SetActive(true);
            FlagPannel.gameObject.SetActive(true);
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("name", name);
            form.AddField("password", password);
            form.AddField("confirm_password", confirm_password);

            using (UnityWebRequest www = UnityWebRequest.Post(registerApi, form))
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
                    if (respone == "Username already exists")
                    {
                        AlertText.text = respone;
                        AlertPannel.gameObject.SetActive(true);
                        FlagPannel.gameObject.SetActive(true);
                    } else if (respone == "Name already exists")
                    {
                        AlertText.text = respone;
                        AlertPannel.gameObject.SetActive(true);
                        FlagPannel.gameObject.SetActive(true);
                    }
                    else if (respone == "Sign up successfully")
                    {
                        resetInputField();
                        BackToLogin();
                    }
                }
            }
        }
    }

    public void resetInputField()
    {
        usernameField.text = "";
        nameField.text = "";
        passwordField.text = "";
        confirmpasswordField.text = "";
    }
    public void BackToLogin()
    {
        LoginPannel.gameObject.SetActive(true);
        RegisterPannel.gameObject.SetActive(false);
    }
    private string removeQuotes(string a)
    {
        string b = a.Substring(1, a.Length - 2);
        return b;
    }
}

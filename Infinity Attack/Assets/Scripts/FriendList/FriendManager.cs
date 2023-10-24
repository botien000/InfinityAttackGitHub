using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class FriendManager : MonoBehaviour
{
    [SerializeField] Friend friendPrefab;
    [SerializeField] Transform transfContentFriend;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] TMP_InputField txtInputID;
    [SerializeField] AnnouncementFriendList announcement;

    private List<Friend> friends = new List<Friend>();
    bool checkConnectServer = true;

    public List<FriendOfUser> friendOfUserList = new List<FriendOfUser>();
    public FriendEntity[] friendEntities;
    void OnEnable()
    {
        StartCoroutine(GetAllFriends());
    }
    void OnDisable()
    {
        txtInputID.text = "";
        friendOfUserList.Clear();
        foreach (var friend in friends)
        {
            DestroyImmediate(friend.gameObject);
        }
        friends.Clear();
    }
    IEnumerator GetAllFriends()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", User.Instance.user._id);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerGetAllFriends, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            loadingPanel.SetActive(true);
            yield return null;
        }
        loadingPanel.SetActive(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log($"IEGetAllFriends:{json}");
            if (json != "[]")
            {
                friendEntities = JsonConvert.DeserializeObject<FriendEntity[]>(json);
                GetFriendIDList();
            }
            else
            {
                Debug.Log("You haven't any friends");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    IEnumerator IEGetCharacterOwnSelected()
    {
        for (int i = 0; i < friendOfUserList.Count; i++)
        {
            WWWForm form = new WWWForm();
            form.AddField("userID", friendOfUserList[i].userID);
            UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerGetUsingCharNameById, form);
            var handler = unityWebRequest.SendWebRequest();
            while (!handler.isDone)
            {
                loadingPanel.SetActive(true);
                yield return null;
            }
            loadingPanel.SetActive(false);
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                string json = unityWebRequest.downloadHandler.text;
                Debug.Log($"IEGetCharacterOwnSelected:{json}");
                if (json != null)
                {
                    friendOfUserList[i].nameCharacter = JsonConvert.DeserializeObject<string>(json);
                }
            }
            else
            {
                checkConnectServer = false;
                Debug.Log("Failed to connecting server");
            }
            unityWebRequest.Dispose();
            if (!checkConnectServer)
                break;
        }
        SpawnFriend();
    }
    private IEnumerator IEGetUser()
    {
        for (int i = 0; i < friendOfUserList.Count; i++)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", friendOfUserList[i].userID);
            UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerGetUserById, form);
            var handler = unityWebRequest.SendWebRequest();
            while (!handler.isDone)
            {
                loadingPanel.SetActive(true);
                yield return null;
            }
            loadingPanel.SetActive(false);
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                checkConnectServer = true;
                string json = unityWebRequest.downloadHandler.text;
                Debug.Log("IEGetUser:   " + json);
                if (json != null)
                {
                    UserID user = JsonConvert.DeserializeObject<UserID>(json);
                    friendOfUserList[i].nameUser = user.name;
                }
                else
                {
                    Debug.Log("No User");
                }
            }
            else
            {
                checkConnectServer = false;
                Debug.Log("Failed to connecting server");
            }
            unityWebRequest.Dispose();
            if (!checkConnectServer)
                break;
        }
        if (checkConnectServer)
            StartCoroutine(IEGetCharacterOwnSelected());
    }
    private IEnumerator IEDeleteAnFriend(string id, Friend friendGO)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerDeleteAFriend, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            loadingPanel.SetActive(true);
            yield return null;
        }
        loadingPanel.SetActive(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            checkConnectServer = true;
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEDeleteAnFriend:   " + json);
            StatusDeleteFriend status = JsonConvert.DeserializeObject<StatusDeleteFriend>(json);
            if (status.acknowledged && status.deletedCount > 0)
            {
                announcement.gameObject.SetActive(true);
                announcement.Init("Deleted sucessfully", 1);
                friends.Remove(friendGO);
                DestroyImmediate(friendGO.gameObject);
                Debug.Log("Delete Sucessfully");
            }
            else
            {
                Debug.Log("Failed to delete");
            }
        }
        else
        {
            checkConnectServer = false;
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    private IEnumerator IECheckFriend(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", id);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerCheckExistingFriend, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            loadingPanel.SetActive(true);
            yield return null;
        }
        loadingPanel.SetActive(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            checkConnectServer = true;
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IECheckFriend:   " + json);
            if (json != "null")
            {
                StartCoroutine(IEFriendRequest(id));
            }
            else
            {
                announcement.gameObject.SetActive(true);
                announcement.Init("Non existing player", 1);
            }
        }
        else
        {
            checkConnectServer = false;
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    private IEnumerator IEFriendRequest(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("userReq", User.Instance.user._id);
        form.AddField("userRes", id);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerRequestAnUser, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            loadingPanel.SetActive(true);
            yield return null;
        }
        loadingPanel.SetActive(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            checkConnectServer = true;
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEFriendRequest:   " + json);
            if (json != "{}")
            {
                FriendEntity friendEntity = JsonConvert.DeserializeObject<FriendEntity>(json);
                if (friendEntity.status == 1)
                {
                    announcement.gameObject.SetActive(true);
                    announcement.Init("Player was your friend", 1);
                    Debug.Log("Already made friends");
                }
                else
                {
                    announcement.gameObject.SetActive(true);
                    announcement.Init("Request sent sucessfully", 1);
                    Debug.Log("Sent request");
                }
            }
            else
            {
                announcement.gameObject.SetActive(true);
                announcement.Init("Request has been sent before. Wait for the player to respond", 1);
                Debug.Log("Request sent before");
            }
        }
        else
        {
            checkConnectServer = false;
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }

    private void GetFriendIDList()
    {
        foreach (var friend in friendEntities)
        {
            if (friend.userReq != User.Instance.user._id)
            {
                friendOfUserList.Add(new FriendOfUser { userID = friend.userReq });
            }
            else
            {
                friendOfUserList.Add(new FriendOfUser { userID = friend.userRes });
            }
        }
        StartCoroutine(IEGetUser());
    }
    private void SpawnFriend()
    {
        if (!checkConnectServer)
        {
            return;
        }
        for (int i = 0; i < friendOfUserList.Count; i++)
        {
            Friend friend = Instantiate(friendPrefab, transfContentFriend);
            friend.Init(friendEntities[i], friendOfUserList[i], this);
            friends.Add(friend);
        }
    }
    public void DeleteAnFriend(string id, Friend friendGO)
    {
        StartCoroutine(IEDeleteAnFriend(id, friendGO));
    }
    public void BtnFindAddFriend()
    {
        string txtID = txtInputID.text;
        if (txtID.Length != 24)
        {
            announcement.gameObject.SetActive(true);
            announcement.Init("Player ID consists of 24 characters", 1);
            Debug.LogError("ID have 24 characters");
            return;
        }
        else if(txtID == User.Instance.user._id)
        {
            announcement.gameObject.SetActive(true);
            announcement.Init("ID ERROR", 1);
            Debug.LogError("ID ERROR");
            return;
        }
        StartCoroutine(IECheckFriend(txtID));
    }

    public void BackToHomeScene()
    {
        SavePlay(1);
        SceneManager.LoadScene("Home");
    }

    private void SavePlay(int play)
    {
        PlayerPrefs.SetInt("Play", play);
    }
}
[System.Serializable]
public class FriendEntity
{
    public string _id;
    public string userReq;
    public string userRes;
    public int status;
}
[System.Serializable]
public class FriendOfUser
{
    public string userID;
    public string nameUser;
    public string nameCharacter;
}
public class StatusDeleteFriend
{
    public bool acknowledged;
    public int deletedCount;
}

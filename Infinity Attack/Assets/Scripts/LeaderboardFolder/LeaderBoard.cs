using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] LeaderboardDetail leaderBoardDetailPrefab;
    [SerializeField] Transform transfContentLBFriend, transfContentLBGlobal;
    [SerializeField] ScrollRect scrollRectLB;
    [SerializeField] Button btnGlobal, btnFriend;

    FriendEntity[] friendEntities;
    public List<User> userIDList = new List<User>();
    public List<User> globalUsersList;

    List<LeaderboardDetail> lbDetailGOList = new List<LeaderboardDetail>();
    bool checkConnectServer;

    private void OnEnable()
    {
        btnFriend.interactable = false;
        btnGlobal.interactable = false;
        StartCoroutine(GetAllFriends());
        StartCoroutine(IEGetUsers());
    }
    private void OnDisable()
    {
        foreach (var lb in lbDetailGOList)
        {
            DestroyImmediate(lb.gameObject);
        }
        lbDetailGOList.Clear();
        userIDList.Clear();
        globalUsersList.Clear();
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
    private void GetFriendIDList()
    {
        foreach (var friend in friendEntities)
        {
            if (friend.userReq != User.Instance.user._id)
            {
                userIDList.Add(new User { _id = friend.userReq });
            }
            else
            {
                userIDList.Add(new User { _id = friend.userRes });
            }
        }
        StartCoroutine(IEGetUser());
    }
    private IEnumerator IEGetUser()
    {
        for (int i = 0; i < userIDList.Count; i++)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", userIDList[i]._id.ToString());
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
                    User user = JsonConvert.DeserializeObject<User>(json);
                    userIDList[i] = user;
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
            SortingFriendsGem();
        //    StartCoroutine(IEGetCharacterOwnSelected());
    }
    private IEnumerator IEGetUsers()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerGetUsers, form);
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
            Debug.Log("IEGetUsers:   " + json);
            if (json != "[]")
            {
                User[] users = JsonConvert.DeserializeObject<User[]>(json);
                globalUsersList = users.ToList();
                SortingGlobalUsersGem();
            }
            else
            {
                Debug.Log("No Users");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    void SortingFriendsGem()
    {
        userIDList.Add(User.Instance.user);
        userIDList.Sort((a, b) => a.gem.CompareTo(b.gem));
        userIDList.Reverse();
        userIDList = SetPlayerToTop(userIDList);
        ShowRank(userIDList, transfContentLBFriend);
    }
    void SortingGlobalUsersGem()
    {
        globalUsersList.Sort((a, b) => a.gem.CompareTo(b.gem));
        globalUsersList.Reverse();
        globalUsersList = SetPlayerToTop(globalUsersList);
        ShowRank(globalUsersList, transfContentLBGlobal);
    }
    List<User> SetPlayerToTop(List<User> userIDs)
    {
        for (int i = userIDs.FindIndex((user) => user._id == User.Instance.user._id); i >= 0; i--)
        {
            if (i == 0)
                break;
            if (userIDs[i].gem < userIDs[i - 1].gem)
            {
                break;
            }
            else if (userIDs[i].gem == userIDs[i - 1].gem)
            {
                User userid = userIDs[i];
                userIDs[i] = userIDs[i - 1];
                userIDs[i - 1] = userid;
            }
        }
        return userIDs;
    }

    void ShowRank(List<User> users, Transform parent)
    {
        int top = 1;
        for (int i = 0; i < users.Count; i++)
        {
            if (i != 0)
            {
                if (users[i].gem != users[i - 1].gem)
                {
                    top++;
                }
            }
            LeaderboardDetail detail = Instantiate(leaderBoardDetailPrefab, parent);
            detail.Init(top, users[i].name, users[i]._id, users[i].gem, true ? users[i]._id == User.Instance.user._id : false);
            lbDetailGOList.Add(detail);
            BtnGlobal();
        }
    }
    public void BtnGlobal()
    {
        scrollRectLB.content = (RectTransform)transfContentLBGlobal;
        transfContentLBGlobal.gameObject.SetActive(true);
        transfContentLBFriend.gameObject.SetActive(false);
        btnGlobal.interactable = false;
        btnFriend.interactable = true;
    }
    public void BtnFriend()
    {
        scrollRectLB.content = (RectTransform)transfContentLBFriend;
        transfContentLBFriend.gameObject.SetActive(true);
        transfContentLBGlobal.gameObject.SetActive(false);
        btnFriend.interactable = false;
        btnGlobal.interactable = true;
    }
}

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FindResponeFriend : MonoBehaviour
{
    [SerializeField] AnnouncementFriendList announcementFriendList;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject confirmPanel;
    [SerializeField] TextMeshProUGUI txtConfirm;

    bool isGetUser, statusAnnouncement;
    FriendEntity friendEntity;

    public void Find()
    {
        StartCoroutine(IEFindRespone(User.Instance.user._id));
    }
    private IEnumerator IEFindRespone(string userResponeID)
    {
        while (true)
        {
            WWWForm form = new WWWForm();
            form.AddField("userRes", userResponeID);
            UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerFindRespone, form);
            var handler = unityWebRequest.SendWebRequest();
            while (!handler.isDone)
            {
                yield return null;
            }
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                string json = unityWebRequest.downloadHandler.text;
                Debug.Log("IEFindRespone:   " + json);
                if (json != "null")
                {
                    friendEntity = JsonConvert.DeserializeObject<FriendEntity>(json);
                    StartCoroutine(IEGetUser(friendEntity.userReq));
                }
                else
                {
                    Debug.Log("No any request is sent to you");
                }
            }
            else
            {
                Debug.Log("Failed to connecting server");
            }
            unityWebRequest.Dispose();
            yield return new WaitUntil(() => !isGetUser && !statusAnnouncement);
            yield return new WaitForSeconds(2);
        }
    }
    private IEnumerator IEGetUser(string id)
    {
        isGetUser = true;
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerGetUserById, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            yield return null;
        }
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEGetUser:   " + json);
            if (json != null)
            {
                UserID user = JsonConvert.DeserializeObject<UserID>(json);
                //announcement
                AnnouncementAddFriend(user.name);
            }
            else
            {
                Debug.Log("No User");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        isGetUser = false;
        unityWebRequest.Dispose();
    }
    private IEnumerator IEDeleteAnFriend(string id)
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
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log("IEDeleteAnFriend:   " + json);
            StatusDeleteFriend status = JsonConvert.DeserializeObject<StatusDeleteFriend>(json);
            if (status.acknowledged && status.deletedCount > 0)
            {
                confirmPanel.SetActive(true);
                txtConfirm.text = "Huỷ thành công";
                Debug.Log("Delete Sucessfully");
            }
            else
            {
                Debug.Log("Failed to delete");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        statusAnnouncement = false;
        unityWebRequest.Dispose();
    }
    private IEnumerator IEAddTrueFriend(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerAddTrueFriend, form);
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
            Debug.Log("IEAddTrueFriend:   " + json);
            if (json != null)
            {
                confirmPanel.SetActive(true);
                txtConfirm.text = "Kết bạn thành công";
                friendEntity = JsonConvert.DeserializeObject<FriendEntity>(json);

            }
            else
            {
                Debug.Log("Make failed");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        statusAnnouncement = false;
        unityWebRequest.Dispose();

    }
    void AnnouncementAddFriend(string nameUserReq)
    {
        statusAnnouncement = true;
        announcementFriendList.gameObject.SetActive(true);
        announcementFriendList.Init(nameUserReq + " muốn kết bạn với bạn", 2);
    }
    public void Decline()
    {
        StartCoroutine(IEDeleteAnFriend(friendEntity._id));
    }
    public void Accept()
    {
        StartCoroutine(IEAddTrueFriend(friendEntity._id));
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    [SerializeField] Image imgAvatar;
    [SerializeField] TextMeshProUGUI txtNameUser;
    [SerializeField] Button btnDelete;

    private FriendManager friendManager;
    private FriendOfUser friendOfUser;
    private FriendEntity friendEntity;
    private void OnEnable()
    {
        btnDelete.onClick.AddListener(() => { friendManager.DeleteAnFriend(friendEntity._id,this); });
    }
    public void Init(FriendEntity friend, FriendOfUser user, FriendManager manager)
    {
        friendManager = manager;
        friendOfUser = user;
        friendEntity = friend;
        txtNameUser.text = friendOfUser.nameUser;
        if (friendOfUser.nameCharacter == null)
        {
            imgAvatar.gameObject.SetActive(false);
        }
        else
        {
            imgAvatar.gameObject.SetActive(true);
            imgAvatar.sprite = Resources.Load<Sprite>("Avatars/" + friendOfUser.nameCharacter);
        }
    }
}

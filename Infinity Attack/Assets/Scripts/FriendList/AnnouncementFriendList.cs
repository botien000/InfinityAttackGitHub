using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementFriendList : MonoBehaviour
{
    [SerializeField] Button btnLeft,btnRight;
    [SerializeField] TextMeshProUGUI txtAnnouncement,txtLeft,txtRight;
    [SerializeField] FindResponeFriend findResponeFriend;

    // Start is called before the first frame update
    void Start()
    {
        btnLeft.onClick.AddListener(BtnLeft);
        btnRight.onClick.AddListener(BtnRight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(string text,int type)
    {
        txtAnnouncement.text = text;
        if(type == 1)
        {
            btnLeft.gameObject.SetActive(true);
            btnRight.gameObject.SetActive(false);
            txtLeft.text = "Confirm";
        }
        else
        {
            btnLeft.gameObject.SetActive(true);
            btnRight.gameObject.SetActive(true);
            txtLeft.text = "Deline";
            txtRight.text = "Accept";
        }
    }
    public void BtnRight()
    {
        findResponeFriend.Accept();
    }
    public void BtnLeft()
    {
        if(txtLeft.text == "Decline")
        {
            findResponeFriend.Decline();
        }
        gameObject.SetActive(false);
    }
}

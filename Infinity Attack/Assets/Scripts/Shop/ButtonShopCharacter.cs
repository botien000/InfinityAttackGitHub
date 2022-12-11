using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonShopCharacter : MonoBehaviour
{
    [SerializeField] private Image imgAvatar;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Button btnBuy;
    [SerializeField] private GameObject gemGO;
   
    private ConfirmUI confirmUI;
    private Vector3 vecOriginTxtPrice;
    private ShopUI shopUI;
    private int price;
    private string id;
    private string nameCharacter;
    // Start is called before the first frame update
    void Start()
    {
        vecOriginTxtPrice = txtPrice.transform.position;
        shopUI = ShopUI.Instance;
        confirmUI = shopUI.confirmUI;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Btn()
    {
        confirmUI.gameObject.SetActive(true);
        confirmUI.InitBoardCFChar(name, price, imgAvatar.sprite, this);
        //CheckGemUser(User.Instance.user.gem);
    }
    //private void CheckGemUser(int amountUserGem)
    //{
    //    int curAmountUserGem = amountUserGem;
    //    if (curAmountUserGem < price)
    //    {
    //        Debug.Log("Not enough gems to buy");
    //        return;
    //    }
    //    else
    //    {
    //        Debug.Log("Enough gems to buy.Loading...");
    //        curAmountUserGem -= price;
    //    }
    //}
    public void ChangeGem(int price)
    {
        StartCoroutine(IEUpdateGemUser(price));
    }
    public IEnumerator IEUpdateGemUser(int price)
    {
        WWWForm form = new WWWForm();
        form.AddField("_id", User.Instance.user._id);
        form.AddField("gem", price);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerGemUser, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            shopUI.SetAnimationLoading(true);
            yield return null;
        }
        shopUI.SetAnimationLoading(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log(json);
            if (json != "[]")
            {
                UpdateGem updateGem = JsonConvert.DeserializeObject<UpdateGem>(json);
                if (updateGem.acknowledged)
                {
                    Debug.Log("Update gem successfully");
                    User.Instance.user.gem = price;
                    shopUI.SetTextGem(User.Instance.user.gem.ToString());
                    StartCoroutine(AddNewCharacter());
                }
                else
                {
                    Debug.Log("Update gem failed");
                }
            }
            else
            {
                Debug.Log("No");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    private IEnumerator AddNewCharacter()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", User.Instance.user._id);
        form.AddField("characterID", id);
        form.AddField("status", 0);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerAddNewCharacter, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            shopUI.SetAnimationLoading(true);
            yield return null;
        }
        shopUI.SetAnimationLoading(false);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log(json);
            if (json != "[]")
            {
                CharacterOwn characterOwn = JsonConvert.DeserializeObject<CharacterOwn>(json);
                confirmUI.SetTextLog("Mua thành công");
                Debug.Log("Add character successfully");
                btnBuy.interactable = false;
                txtPrice.text = "Owned";
                txtPrice.transform.position = btnBuy.transform.position;
                gemGO.SetActive(false);
            }
            else
            {
                Debug.Log("Add character failed");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    internal void Init(string _id, string _name, int _price)
    {
        id = _id;
        nameCharacter = _name;
        name = _name;
        price = _price;
        imgAvatar.sprite = Resources.Load<Sprite>("Avatars/" + name);
        txtName.text = name;
        txtPrice.text = price.ToString();
        if (txtPrice.transform.position == btnBuy.transform.position)
            txtPrice.transform.position = vecOriginTxtPrice;
        btnBuy.interactable = true;
    }

    class UpdateGem
    {
        public bool acknowledged = false;
    }
}

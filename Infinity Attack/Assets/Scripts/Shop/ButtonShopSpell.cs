using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ButtonShopSpell : MonoBehaviour
{
    [SerializeField] private Image imgAvatar;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Button btnBuy;
    [SerializeField] private GameObject goldGO;

    private ConfirmUI confirmUI;
    private Vector3 vecOriginTxtPrice;
    private ShopUI shopUI;
    private int price;
    private int total, curAmountSpell;
    private string id;
    private string nameSpell;
    // Start is called before the first frame update
    void Start()
    {
        shopUI = ShopUI.Instance;
        confirmUI = shopUI.confirmUI;
    }

    // Update is called once per frame
    void Update()
    {

    }
    internal void Init(string _id, string _name, int _price, int _total)
    {
        id = _id;
        nameSpell = _name;
        name = _name;
        price = _price;
        total = _total;
        imgAvatar.sprite = Resources.Load<Sprite>("Spells/" + name);
        txtName.text = name;
        txtPrice.text = price.ToString();
        if (txtPrice.transform.position == btnBuy.transform.position)
            txtPrice.transform.position = vecOriginTxtPrice;
        btnBuy.interactable = true;
    }
    public void Btn()
    {
        curAmountSpell = shopUI.CheckCurAmountSpell(id);
        confirmUI.InitSpellDetail(nameSpell, imgAvatar.sprite, price, curAmountSpell,total, this);
    }
    public IEnumerator IEUpdateGoldUser(int price,int amount)
    {
        WWWForm form = new WWWForm();
        form.AddField("_id", User.Instance.user._id);
        form.AddField("gold", price);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerGoldUser, form);
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
                UpdateGold updateGold = JsonConvert.DeserializeObject<UpdateGold>(json);
                if (updateGold.acknowledged)
                {
                    Debug.Log("Update gold successfully");
                    User.Instance.user.gold = price;
                    shopUI.SetTextGold(User.Instance.user.gold.ToString());
                    StartCoroutine(IEUpdateAmount(amount));
                }
                else
                {
                    Debug.Log("Update gold failed");
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
    IEnumerator IEUpdateAmount(int amount)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest unityWebRequest;
        if (curAmountSpell == 0)
        {
            curAmountSpell += amount;
            form.AddField("userID", User.Instance.user._id);
            form.AddField("spellID", id);
            form.AddField("amount", curAmountSpell);
            unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerAddSpellOwn, form);
        }
        else
        {
            curAmountSpell += amount;
            form.AddField("_id", shopUI.CheckIdSpellOwn(id));
            form.AddField("amount", curAmountSpell);
            unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerUpdateAmount, form);
        }
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
                Debug.Log("Update Amount Sucessfully");
                confirmUI.InitBoard("Mua thành công");
                SpellOwnUtility spellOwnUtility = JsonConvert.DeserializeObject<SpellOwnUtility>(json);
                shopUI.UpdateSpellOwnIfNotExisting(spellOwnUtility);
                confirmUI.SetTextAmountSpell(curAmountSpell, total);
            }
            else
            {
                Debug.Log("Failed Update");
            }
        }
        else
        {
            curAmountSpell -= amount;
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }
    public void HandleBtnBuyOne()
    {
        CheckGoldUser(User.Instance.user.gold);
    }

    public void HandleBtnBuyMax()
    {
        bool check = false;
        int totalRemaining = total - curAmountSpell;
        for (int amount = totalRemaining; amount >= 1; amount--)
        {
            if(User.Instance.user.gold >= price * amount)
            {
                check = true;
                StartCoroutine(IEUpdateGoldUser(User.Instance.user.gold - price * amount,amount));
                break;
            }
        }
        if (!check)
        {
            confirmUI.InitBoard("Không đủ vàng để mua");
        }

    }
    private void CheckGoldUser(int amountUserGold)
    {
        int curAmountUserGold = amountUserGold;
        if (curAmountUserGold < price)
        {
            Debug.Log("Not enough golds to buy");
            confirmUI.InitBoard("Không đủ vàng để mua");
        }
        else
        {
            Debug.Log("Enough golds to buy.Loading...");
            curAmountUserGold -= price;
            StartCoroutine(IEUpdateGoldUser(curAmountUserGold,1));
        }
    }
    class UpdateGold
    {
        public bool acknowledged;
    }
}

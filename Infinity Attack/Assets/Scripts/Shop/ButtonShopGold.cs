using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ButtonShopGold : MonoBehaviour
{
    [SerializeField] private int priceGem;
    [SerializeField] private int amountGold;


    private ConfirmUI confirmUI;
    private ShopUI shopUI;
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
    public void Btn()
    {
        CheckGemUser(User.Instance.user.gem);
    }
    private void CheckGemUser(int amountUserGem)
    {
        int curAmountUserGem = amountUserGem;
        if (curAmountUserGem < priceGem)
        {
            confirmUI.gameObject.SetActive(true);
            confirmUI.InitBoard("Không đủ gem để mua");
            Debug.Log("Not enough gems to buy");
            return;
        }
        else
        {
            Debug.Log("Enough gems to buy.Loading...");
            curAmountUserGem -= priceGem;
            StartCoroutine(UpdateGemUser(curAmountUserGem));
        }
    }
    public IEnumerator UpdateGemUser(int price)
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
                    User.Instance.user.gem = price;
                    Debug.Log("Update gem successfully");
                    StartCoroutine(UpdateGoldUser(User.Instance.user.gold + amountGold));
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
    public IEnumerator UpdateGoldUser(int price)
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
                    User.Instance.user.gold = price;
                    Debug.Log("Update gold successfully");
                    shopUI.SetTextGem(User.Instance.user.gem.ToString());
                    shopUI.SetTextGold(User.Instance.user.gold.ToString());
                    confirmUI.gameObject.SetActive(true);
                    confirmUI.InitBoard("Mua thành công");
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
    class UpdateGem
    {
        public bool acknowledged = false;
    }
    class UpdateGold
    {
        public bool acknowledged = false;
    }

}

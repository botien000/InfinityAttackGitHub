using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ConfirmUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtLogCharacter, txtNameCharacter, txtPrice, txtLog;
    [SerializeField] TextMeshProUGUI txtNameSpell, txtPriceSpell, txtAmountSpell;
    [SerializeField] Image imgCharacter, imgSpell;
    [SerializeField] GameObject gemGO;
    [SerializeField] Button btnBuy, btnBuyOneSpell, btnBuyMaxSpell;
    [SerializeField] GameObject boardCharacterGO, boardGO, boardSpellDetail;


    int priceCharacter;
    ButtonShopCharacter btnShopCharacter;
    ButtonShopSpell btnShopSpell;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitBoard(string log)
    {
        gameObject.SetActive(true);
        boardCharacterGO.SetActive(false);
        boardGO.SetActive(true);
        txtLog.text = log;
    }
    public void InitBoardCFChar(string name, int _price, Sprite avatarChar, ButtonShopCharacter btn)
    {
        boardGO.SetActive(false);
        boardCharacterGO.SetActive(true);
        SetTextLog("Bạn muốn mua nhân vật này?");
        btnBuy.interactable = true;
        priceCharacter = _price;
        btnShopCharacter = btn;
        txtNameCharacter.text = name;
        txtPrice.text = _price.ToString();
        imgCharacter.sprite = avatarChar;
        txtPrice.gameObject.SetActive(true);
        gemGO.gameObject.SetActive(true);
    }
    public void BtnBuyCharacter()
    {
        if (txtPrice.gameObject.activeSelf)
            CheckGemUser(User.Instance.user.gem);
        else
        {
            Debug.Log("Equip");
            btnBuy.interactable = false;
        }
    }
    private void CheckGemUser(int amountUserGem)
    {
        int curAmountUserGem = amountUserGem;
        if (curAmountUserGem < priceCharacter)
        {
            Debug.Log("Không đủ Gem để mua");
            SetTextLog("Không đủ Gem để mua");
            return;
        }
        else
        {
            Debug.Log("Enough gems to buy.Loading...");
            curAmountUserGem -= priceCharacter;
            btnShopCharacter.ChangeGem(curAmountUserGem);
        }
    }
    public void SetTextLog(string log)
    {
        txtLogCharacter.text = log;
        if (log == "Mua thành công")
        {
            txtPrice.gameObject.SetActive(false);
            gemGO.gameObject.SetActive(false);
        }
    }

    internal void InitSpellDetail(string nameSpell, Sprite sprite, int price, int current, int total, ButtonShopSpell _btnShopSpell)
    {

        btnBuyOneSpell.interactable = current < total;
        btnBuyMaxSpell.interactable = current < total;
        btnShopSpell = _btnShopSpell;
        boardSpellDetail.SetActive(true);
        txtNameSpell.text = nameSpell;
        txtPriceSpell.text = price.ToString();
        imgSpell.sprite = sprite;
        txtAmountSpell.text = current + "/" + total;
    }
    public void BtnBuyOneSpell()
    {
        btnShopSpell.HandleBtnBuyOne();
    }
    public void BtnBuyMax()
    {
        btnShopSpell.HandleBtnBuyMax();
    }
    internal void SetTextAmountSpell(int curAmountSpell, int total)
    {
        txtAmountSpell.text = curAmountSpell + "/" + total;
        if (curAmountSpell == total)
        {
            btnBuyOneSpell.interactable = false;
            btnBuyMaxSpell.interactable = false;
        }
    }
}

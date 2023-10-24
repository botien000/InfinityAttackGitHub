using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConversationUI : MonoBehaviour
{
    [SerializeField] private SctblObjConversation[] conversations;
    [SerializeField] private Image imgCharacterAvatar;
    [SerializeField] private Image[] imgBossesAvatar;
    [SerializeField] private GameObject[] goBossesTalkPanel;
    [SerializeField] private GameObject goCharacterTalkPanel;
    [SerializeField] private Sprite[] sptCharacters, sptBosses;
    [SerializeField] private TextMeshProUGUI txtCharacter, txtBoss;

    private Sprite curSptCharacter;
    private int mapNumberIndex;
    private int countConversation, countTurnInConver;
    private bool isCharacterTalking;

    private void OnEnable()
    {
        // Run first conversation
        if (conversations[mapNumberIndex] == null) // no conversation in this map
        {
            //GameManager.instance.SetPlayerFreeze(false);
            SetActiveConversation(false);
            return;
        }
        //GameManager.instance.SetPlayerFreeze(true);
        isCharacterTalking = conversations[mapNumberIndex].isCharacterFirstTalking;
        countConversation = 0;
        countTurnInConver = 1;
        RunConversation();
    }

    [ContextMenu("TestEnable")]
    private void TestEnable()
    {
        mapNumberIndex = 0;
        isCharacterTalking = conversations[mapNumberIndex].isCharacterFirstTalking;
        // Run first conversation
        countConversation = 0;
        countTurnInConver = 1;
        RunConversation();
    }

    [ContextMenu("TestPointDown")]
    private void TestPointDown()
    {
        mapNumberIndex = 1;
        bool v = conversations[mapNumberIndex].conversions[countConversation].characterText ==   "" ? true : false;
        Debug.Log(v);
    }

    public void PointerUp()
    {
        NextTurnInConver();
    }

    private void NextTurnInConver()
    {
        if (countTurnInConver >= 2)
        {
            countConversation++;
            countTurnInConver = 1;
        }
        else
        {
            countTurnInConver++;
        }
        RunConversation();
    }

    private void RunConversation()
    {
        if (countConversation >= conversations[mapNumberIndex].conversions.Count) // end the conversation
        {
            SetActiveConversation(false);
            GameManager.instance.SetPlayerFreeze(false);
            return;
        }

        if (isCharacterTalking) // Character talk first
        {
            isCharacterTalking = false;
            if (conversations[mapNumberIndex].conversions[countConversation].characterText == "")
            {
                NextTurnInConver();
                return;
            }
            SetCharacterTalking(countConversation);
        }
        else
        {
            isCharacterTalking = true;
            if (conversations[mapNumberIndex].conversions[countConversation].bossText == "")
            {
                NextTurnInConver();
                return;
            }
            SetBossesTalking(countConversation);
        }
    }

    private void SetCharacterTalking(int covrNumber)
    {
        txtCharacter.text = conversations[mapNumberIndex].conversions[covrNumber].characterText;
        SetImgCharacter();
        SetActiveChar(true);
        SetActiveBosses(false);
    }

    private void SetBossesTalking(int covrNumber)
    {
        txtBoss.text = conversations[mapNumberIndex].conversions[covrNumber].bossText;
        SetImgBosses();
        SetActiveChar(false);
        SetActiveBosses(true);
    }

    private void SetActiveChar(bool isActive)
    {
        goCharacterTalkPanel.SetActive(isActive);
        txtCharacter.gameObject.SetActive(isActive);
        imgCharacterAvatar.gameObject.SetActive(isActive);
    }

    private void SetActiveBosses(bool isActive)
    {
        txtBoss.gameObject.SetActive(isActive);
        imgBossesAvatar[0].gameObject.SetActive(isActive);
        goBossesTalkPanel[0].gameObject.SetActive(isActive);
    }

    private void SetImgCharacter()
    {
        imgCharacterAvatar.sprite = curSptCharacter;
    }

    private void SetImgBosses()
    {
        imgBossesAvatar[0].sprite = sptBosses[0];
        if (imgBossesAvatar.Length > 1)
        {
            imgBossesAvatar[1].sprite = sptBosses[1];
        }
    }

    public void SetSptCharacter(string name)
    {
        foreach (var spt in sptCharacters)
        {
            if (spt.name == name)
            {
                curSptCharacter = spt;
                break;
            }
        }
    }

    public void SetBossMap(bool map)
    {
        mapNumberIndex = map == true ? 1 : 0;
    }

    public void SetActiveConversation(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}

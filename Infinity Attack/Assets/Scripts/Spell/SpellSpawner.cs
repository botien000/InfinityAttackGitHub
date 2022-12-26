using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum SpellType
{
    Healing,
    Chaos,
    Speedup,
    Fire,
    UtilmateRemake
}
public class SpellSpawner : MonoBehaviour
{
    [SerializeField] Button[] btnSpells;
    [SerializeField] Image[] imgBtnSpells;
    [SerializeField] Image[] imgCooldown;
    [SerializeField] GameObject healingFXPrefab, chaosPrefab, speedUpPrefab, firePrefab;

    CharacterObject player;
    SpellType type;
    SpellSingleton.SpellValuable[] spellValuables;
    private void Start()
    {
        spellValuables = new SpellSingleton.SpellValuable[btnSpells.Length];
        for (int i = 0; i < btnSpells.Length; i++)
        {
            spellValuables[i] = SpellSingleton.Instance.GetSpell(i);
            if (spellValuables[i].spellPath != null)
            {
                int index = i;
                btnSpells[i].onClick.AddListener(() => SpawnSpellBtn(index));
                imgBtnSpells[i].sprite = Resources.Load<Sprite>(spellValuables[i].spellPath);
                imgCooldown[i].gameObject.SetActive(false);
            }
            else
            {
                btnSpells[i].gameObject.SetActive(false);
            }
        }
        //StartCoroutine(IEUpdateAmount());
    }
    void SetSpellType(SpellType type, bool active, int btnIndex)
    {
        switch (type)
        {
            case SpellType.Healing:
                if (active)
                {
                    HandleSpell("HealingFx(Clone", healingFXPrefab, player.transform, 2, btnIndex, type);
                    player.Healing(25); // 25%
                }
                break;
            case SpellType.Chaos:
                if (active)
                {
                    HandleSpell("Chaos(Clone)", chaosPrefab, player.transform, 2, btnIndex, type);
                    player.IncreateDamage(2,0);
                }
                else
                {
                    player.IncreateDamage(2,1);
                }
                break;
            case SpellType.Speedup:
                if (active)
                {
                    HandleSpell("SpeedUpFX(Clone)", null, player.transform, 2, btnIndex, type);
                    player.SpeedUp(0.8f);  
                }
                else
                {
                    player.SpeedUp(-0.8f); 
                }
                break;
            case SpellType.Fire:
                if (active)
                {
                    HandleSpell("Fire", firePrefab, player.transform, 0, btnIndex, type);                   
                    Debug.Log("Active Fire");
                    SystemData.instance.FlagDataSpell();
                }
                break;
            case SpellType.UtilmateRemake:
                if (active)
                {
                    player.ResetingUtilmate();
                    HandleSpell("", null, player.transform, 0, btnIndex, type);
                    Debug.Log("Active UtilmateRemake");
                }
                break;
        }
    }
    void HandleSpell(string name, GameObject prefab, Transform position, float timeRemaining, int btnIndex, SpellType type)
    {
        if (prefab == null)
        {
            StartCoroutine(IEHandleCoolDown(btnIndex));
            StartCoroutine(IEHandleTimeRemaining(timeRemaining, type, btnIndex, null));
        }
        else
        {
            Transform[] gos = player.GetComponentsInChildren<Transform>(true);
            Transform go = Array.Find<Transform>(gos, (gameobj) => gameobj.name == name);
            if (go == null)
            {
                if (type == SpellType.Fire)
                {
                    if(position.eulerAngles.y > 0)
                    {
                        Fire fire = Instantiate(prefab, position.position + new Vector3(-1.5f, -1.5f, 0), Quaternion.identity).GetComponent<Fire>();
                        fire.Init(-1);
                    }
                    else
                    {
                        Fire fire = Instantiate(prefab, position.position + new Vector3(1.5f, -1.5f, 0), Quaternion.identity).GetComponent<Fire>();
                        fire.Init(1);
                    }
                }
                else
                {
                    go = Instantiate(prefab, position).transform;
                }
            }
            else
            {
                go.gameObject.SetActive(true);
            }
            StartCoroutine(IEHandleCoolDown(btnIndex));
            if(go == null)
            {
                StartCoroutine(IEHandleTimeRemaining(timeRemaining, type, btnIndex, null));
            }
            else
            {
                StartCoroutine(IEHandleTimeRemaining(timeRemaining, type, btnIndex, go.gameObject));
            }
        }
    }
    IEnumerator IEHandleCoolDown(int btnIndex)
    {
        btnSpells[btnIndex].interactable = false;
        imgCooldown[btnIndex].gameObject.SetActive(true);
        float coolDown = spellValuables[btnIndex].cooldown;
        float curCoolDown = coolDown;
        while (true)
        {
            curCoolDown -= Time.deltaTime;
            imgCooldown[btnIndex].fillAmount = curCoolDown / coolDown;
            if (curCoolDown <= 0)
            {
                btnSpells[btnIndex].interactable = true;
                imgCooldown[btnIndex].gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
    }
    IEnumerator IEHandleTimeRemaining(float timeRemainning, SpellType type, int btnIndex, GameObject go)
    {
        while (true)
        {
            if (type == SpellType.Fire || type == SpellType.UtilmateRemake)
                break;
            timeRemainning -= Time.deltaTime;
            if (timeRemainning <= 0)
            {
                if (go != null)
                    go.SetActive(false);
                SetSpellType(type, false, btnIndex);
                break;
            }
            yield return null;
        }
    }

    void SpawnSpellBtn(int index)
    {
        SetSpellType((SpellType)spellValuables[index].spellType, true, index);
    }

    internal void Init(CharacterObject characterObject)
    {
        player = characterObject;
    }

  
}
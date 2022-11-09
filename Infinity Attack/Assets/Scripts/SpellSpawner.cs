using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] Button spellBtn_1, spellBtn_2, spellBtn_3;

    SpellType type;

    void SetSpellType(SpellType type)
    {
        switch (type)
        {
            case SpellType.Healing:
                break;
            case SpellType.Chaos:
                break;
            case SpellType.Speedup:
                break;
            case SpellType.Fire:
                break;
            case SpellType.UtilmateRemake:
                break;
        }
    }
    IEnumerator IEHandleHealing()
    {
        //spawn player parent
        //countdown
        yield return null;
    }
    void SpawnSpellBtn()
    {

    }
}
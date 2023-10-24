using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class SpellSingleton
{
    private SpellValuable[] spellArray = new SpellValuable[3]
    {
        new SpellValuable(),
        new SpellValuable(),
        new SpellValuable()
    };
    private static SpellSingleton instance;
    public static SpellSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SpellSingleton();
            }
            return instance;
        }
    }

    public SpellValuable GetSpell(int index)
    {
        if (index >= spellArray.Length)
            return null;
        return spellArray[index];
    }

    public void SetAllSpellNull()
    {
        for (int i = 0; i < spellArray.Length; i++)
        {
            spellArray[i] = new SpellValuable();
        }
    }

    public void SetSpell(int index, string name, string spellPath, int amount, int spellType, int cooldown, string id)
    {
        this.spellArray[index].id = id;
        this.spellArray[index].name = name;
        this.spellArray[index].spellPath = spellPath;
        this.spellArray[index].amount = amount;
        this.spellArray[index].spellType = spellType;
        this.spellArray[index].cooldown = cooldown;
    }

    public int GetCountSpell()
    {
        return spellArray.Length;
    }
    public class SpellValuable
    {
        public string name;
        public string spellPath;
        public int amount;
        public int spellType;
        public int cooldown;
        public string id;
    }

}

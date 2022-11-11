using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSingleton : MonoBehaviour
{
    private SpellValuable[] spellArray = new SpellValuable[3];
    private static SpellSingleton instance;

    public static SpellSingleton Instance()
    {
        if(instance == null)
        {
            instance = new SpellSingleton();
        }
        return instance;
    }
    public void SetSpell(int index,string spellPath,int placePosition,int spellType)
    {
        spellArray[index].spellPath = spellPath;
        spellArray[index].placePosition = placePosition;
        spellArray[index].spellType = spellType;
    }

    public class SpellValuable
    {
        public string spellPath;
        public int placePosition;
        public int spellType;
    }
    
}

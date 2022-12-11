using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpellChoose : MonoBehaviour
{
    private SpellSingleton spellSingleton;
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    [SerializeField] private Image image3;
    private Sprite addSprite;
    private bool choosen1 = false, choosen2 = false, choosen3 = false;
    void Start()
    {
        spellSingleton = SpellSingleton.Instance;
        LoadAvatar();
        LoadChoosenSpell();
        LoadAvtBtn();
    }
    
    private void LoadAvtBtn()
    {
        for(int i = 0; i < 3; i++)
        {            
            if(i == 0)
            {
                if (!choosen1)
                {
                    image1.sprite = addSprite;
                }
                else
                {
                    image1.sprite = Resources.Load<Sprite>(spellSingleton.GetSpell(i).spellPath);
                }
            }
            else if (i == 1)
            {
                if (!choosen2)
                {
                    image2.sprite = addSprite;
                }
                else
                {
                    image2.sprite = Resources.Load<Sprite>(spellSingleton.GetSpell(i).spellPath);
                }
            }
            else if (i == 2)
            {
                if (!choosen3)
                {
                    image3.sprite = addSprite;
                }
                else
                {
                    image3.sprite = Resources.Load<Sprite>(spellSingleton.GetSpell(i).spellPath);
                }
            }
        }
    }
    private void LoadChoosenSpell()
    {
        for(int i = 0; i < 3; i++)
        {
            if(i == 0)
            {
                if (spellSingleton.GetSpell(i).name == null)
                {
                    choosen1 = false;
                }
                else
                {
                    choosen1 = true;
                }
            }
            else if( i == 1)
            {
                if (spellSingleton.GetSpell(i).name == null)
                {
                    choosen2 = false;
                }
                else
                {
                    choosen2 = true;
                }
            }
            else if (i == 2)
            {
                if (spellSingleton.GetSpell(i).name == null)
                {
                    choosen3 = false;
                }
                else
                {
                    choosen3 = true;
                }
            }
        }
    }

    private void LoadAvatar()
    {
        addSprite = Resources.Load<Sprite>("Spells/add");
    }
}

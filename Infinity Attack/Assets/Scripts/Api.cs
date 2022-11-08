using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Api
{
    public readonly string api = "http://localhost:3000/api/";
    public readonly string routerGetCharacters = "get-characters";
    public readonly string routerGetUserByName = "getUserByName";
    public readonly string routerGetUserById = "getUserById";
    public readonly string routerGetUsers = "getUsers";
    public readonly string routerGemUser = "updateGemUser";
    public readonly string routerGoldUser = "updateGoldUser";
    public readonly string routerCharacterOwn = "getcharacterown";
    public readonly string routerGetUsingCharNameById = "getUsingCharNameById";
    public readonly string routerAddNewCharacter = "addNewCharacter";
    public readonly string routerGetSpells = "getSpells";
    public readonly string routerSpellOwn = "getSpellOwn";

    private static Api instance;
    public readonly string routerUpdateAmount = "updateAmountSpell";
    public readonly string routerAddSpellOwn = "addNewSpellOwn";

    public static Api Instance
    {

        get
        {
            if (instance == null)
            {
                instance = new Api();
            }
            return instance;
        }
    }
}

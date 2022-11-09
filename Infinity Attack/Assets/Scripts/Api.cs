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

    public readonly string routerPostAchievementsOwn = "post-achievements-own";
    public readonly string routerUpdateLevelAchievementOwn = "update-level-achievement-own";
    public readonly string routerUpdateAllChallengeAchievedAchievementByName = "update-all-challenge-achieved-achievement-by-name";

    public readonly string routerPostQuestsOwn = "post-quests-own";
    public readonly string routerUpdateStatusQuestOwn = "update-status-quest-own";
    public readonly string routerUpdateAllChallengeAchievedQuestByName = "update-all-challenge-achieved-quest-by-name";

    public readonly string routerPostGiftsOwn = "post-gifts-own";
    public readonly string routerUpdateStatusGiftOwn = "update-status-gift-own";
        
    public readonly string routerPostSpellsOwn = "post-spells-own";

    public readonly string routerPostCharactersOwn = "post-character-own";
    public readonly string routerUpdateCharacterOwn = "update-character-own";
    public readonly string routerUpdateStatusCharacterOwn = "update-status-character-own";


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

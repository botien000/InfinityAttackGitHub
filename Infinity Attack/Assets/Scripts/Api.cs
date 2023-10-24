using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Api
{
    // http://localhost:3000/api/
    // https://infinityattackservice.onrender.com/api/
    public readonly string api = "http://localhost:3000/api/";

    //login and register
    public readonly string routerRegister = "register";
    public readonly string routerLogin = "login";

    public readonly string routerSendCodeForgot = "sendCodeForgotPass";
    public readonly string routerForgotChangePass = "forgotPass";
    public readonly string routerChangePassword = "changePassword";
    public readonly string routerSendCodeAddEmail = "sendCodeAddEmail";
    public readonly string routerAddEmail = "addEmail";
    public readonly string routerSendCodeChangeEmail = "sendCodeChangeEmail";
    public readonly string routerChangeEmail = "changeEmail";
    public readonly string routerGenerateRememberToken = "generateRememberToken";
    public readonly string routerCheckRememberToken = "checkRememberToken";
    public readonly string routerSetOffline = "SetOffline";

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
    public readonly string routerGetSpellOwn = "getSpellOwn";
    public readonly string routerGetLevelByCharNameAndUid = "getLevelByCharNameAndUid";

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

    public readonly string routerUpdateAmount = "updateAmountSpell";
    public readonly string routerAddSpellOwn = "addNewSpellOwn";
    public readonly string routerRemoveSpellOwn = "removeSpellOwn";

    public readonly string routerFindRespone = "findRespone";
    public readonly string routerDeleteAFriend = "deleteAFriend";
    public readonly string routerGetAllFriends = "getAllFriends";
    public readonly string routerCheckExistingFriend = "checkExistingFriend";
    public readonly string routerRequestAnUser = "requestAnUser";
    public readonly string routerAddTrueFriend = "addTrueFriend";


    private static Api instance;
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

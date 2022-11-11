using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string _id { get; set; }
    public UserID userID { get; set; }
    public CharacterID characterID { get; set; }
    public LevelID levelID { get; set; }
    public int status { get; set; }

    public Sprite characterSprite;
}

public class CharacterID
{
    public string _id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
}

public class LevelID
{
    public string _id { get; set; }
    public string characterID { get; set; }
    public int level { get; set; }
    public int damage { get; set; }
    public int hp { get; set; }
    public int cost { get; set; }
}

public class UserID
{
    public object _id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public int gold { get; set; }
    public int gem { get; set; }
}
public class Top5Users
{
    public string name { get; set; }
    public int count { get; set; }
}
public class Quest
{
    public string _id { get; set; }
    public UserID userID { get; set; }
    public QuestID questID { get; set; }
    public int status { get; set; }
    public int challengeAchieved { get; set; }
}
public class Gift
{
    public string _id { get; set; }
    public UserID userID { get; set; }
    public GiftID giftID { get; set; }
    public int status { get; set; }
}
public class QuestID
{
    public string _id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int point { get; set; }
    public int challenge { get; set; }
}


public class GiftID
{
    public string _id { get; set; }
    public string name { get; set; }
    public int gold { get; set; }
    public int gem { get; set; }
}
public class AchievementID
{
    public string _id { get; set; }
    public string name { get; set; }
}

public class AchievementLevelID
{
    public string _id { get; set; }
    public string achievementID { get; set; }
    public int level { get; set; }
    public string description { get; set; }
    public int challenge { get; set; }
    public int gold { get; set; }
    public int gem { get; set; }
}

public class Achievement
{
    public string _id { get; set; }
    public UserID userID { get; set; }
    public AchievementID achievementID { get; set; }
    public AchievementLevelID achievementLevelID { get; set; }
    public int challengeAchieved { get; set; }
}
public class SpellOwnUtility
{
    public string _id;
    public string userID;
    public string spellID;
    public int amount;
}
public class SpellUtility
{
    public string _id;
    public string name;
    public int price;
    public string description;
    public int cooldown;
    public int total;
}
public class CharacterUtility
{
    public string _id;
    public string name;
    public int price;
}
public class CharacterOwn
{
    public string _id;
    public string userID;
    public string characterID;
    public string levelID;
    public int status;
}


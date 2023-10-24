using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemData : MonoBehaviour
{
    [SerializeField] private int goldPerEnemy;
    [SerializeField] private int gemKillBoss;

    public int map = 3;
    public int amountBossMap_3 = 2;
    public int curTotalGold, curTotalGem;
    public int totalEnemy;
    private int challengeAchievedSingle3Time = 0,
      challengeAchievedKill50Enemy = 0, challengeAchievedKill5Boss = 0, challengeAchievedUse3Spell = 0;

    private int killenemy = 0, killboss = 0, singleplay = 0;

    public int flagEnemy, flagBoss, flagSpell;

    public static SystemData instance;

    
    // Start is called before the first frame update
    void Start()
    {
        //chi start lan dau khi vao game, sau do se load sang cac scene khac cung character
        LoadPlayerPref();
        flagEnemy = 0;
        flagBoss = 0;
        flagSpell = 0;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void FlagDataEnemy()
    {
        flagEnemy++;
        Debug.Log("Flag enemy+ : " + flagEnemy);
        curTotalGold += goldPerEnemy; 
    }

    public void FlagDataBoss()
    {
        flagBoss++;
        Debug.Log("Flag boss+ : " + flagBoss);
        curTotalGem += gemKillBoss;
    }

    public void FlagDataSpell()
    {
        flagSpell++;
        Debug.Log("Flag spell+ : " + flagSpell);
    }

    public void updateAchieved()
    {
        //chi save challengeAchieved khi nhan qua` luc end game
        //enemy
        challengeAchievedKill50Enemy += flagEnemy;
        SaveChallengeAchievedKill50Enemy(challengeAchievedKill50Enemy);
        killenemy += flagEnemy;
        SaveChallengeAchievedKillEnemy(killenemy);

        //boss
        challengeAchievedKill5Boss += flagBoss;
        SaveChallengeAchievedKill5Boss(challengeAchievedKill5Boss);
        killboss += flagBoss;
        SaveChallengeAchievedKillBoss(killboss);

        //singleplay
        challengeAchievedSingle3Time++;
        SaveChallengeAchievedSingle3Time(challengeAchievedSingle3Time);
        singleplay++;
        SaveChallengeAchievedSinglePlay(singleplay);
    }

    public void UpdateSpellUsed()
    {
        //spell
        challengeAchievedUse3Spell += flagSpell;
        SaveChallengeAchievedUse3Spell(challengeAchievedUse3Spell);
    }
    private void LoadPlayerPref()
    {
        //quest
        LoadChallengeAchievedSingle3Time();
        LoadChallengeAchievedKill50Enemy();
        LoadChallengeAchievedKill5Boss();
        LoadChallengeAchievedUse3Spell();

        //achievement
        LoadChallengeAchievedKillEnemy();
        LoadChallengeAchievedKillBoss();
        LoadChallengeAchievedSinglePlay();
    }

    #region Load & Save
    private void LoadChallengeAchievedSingle3Time()
    {
        challengeAchievedSingle3Time = PlayerPrefs.GetInt("challengeAchievedSingle3Time");
    }

    private void SaveChallengeAchievedSingle3Time(int challengeAchievedSingle3Time)
    {
        PlayerPrefs.SetInt("challengeAchievedSingle3Time", challengeAchievedSingle3Time);
    }

    private void LoadChallengeAchievedKill50Enemy()
    {
        challengeAchievedKill50Enemy = PlayerPrefs.GetInt("challengeAchievedKill50Enemy");
    }

    private void SaveChallengeAchievedKill50Enemy(int challengeAchievedKill50Enemy)
    {
        PlayerPrefs.SetInt("challengeAchievedKill50Enemy", challengeAchievedKill50Enemy);
    }
    private void LoadChallengeAchievedKill5Boss()
    {
        challengeAchievedKill5Boss = PlayerPrefs.GetInt("challengeAchievedKill5Boss");
    }

    private void SaveChallengeAchievedKill5Boss(int challengeAchievedKill5Boss)
    {
        PlayerPrefs.SetInt("challengeAchievedKill5Boss", challengeAchievedKill5Boss);
    }

    private void LoadChallengeAchievedUse3Spell()
    {
        challengeAchievedUse3Spell = PlayerPrefs.GetInt("challengeAchievedUse3Spell");
    }

    public void SaveChallengeAchievedUse3Spell(int challengeAchievedUse3Spell)
    {
        PlayerPrefs.SetInt("challengeAchievedUse3Spell", challengeAchievedUse3Spell);
    }

    private void LoadChallengeAchievedKillEnemy()
    {
        killenemy = PlayerPrefs.GetInt("killenemy");
    }

    private void SaveChallengeAchievedKillEnemy(int killenemy)
    {
        PlayerPrefs.SetInt("killenemy", killenemy);
    }
    private void LoadChallengeAchievedKillBoss()
    {
        killboss = PlayerPrefs.GetInt("killboss");
    }

    private void SaveChallengeAchievedKillBoss(int killboss)
    {
        PlayerPrefs.SetInt("killboss", killboss);
    }

    private void LoadChallengeAchievedSinglePlay()
    {
        singleplay = PlayerPrefs.GetInt("singleplay");
    }

    private void SaveChallengeAchievedSinglePlay(int singleplay)
    {
        PlayerPrefs.SetInt("singleplay", singleplay);
    }
    #endregion

}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class Boss3 : MonoBehaviour
{
    public Transform player;
    public bool isFlipped = false;
    public float LeftBound;
    public float RightBound;
    public float TopBound;
    public float BotBound;

    public float speedRun;
    public int attackDamage;
    public int hp;
    public int curhp;
    public int hp_heart;
    public int curhp_heart;
    public float cdSkill1;
    public float nextAttackSkill1;
    public int healAmount;
    public float cdHealing;
    public float nextAttackSkill2;

    public bool canAttack;
    public bool attacking;
    public bool canRun;
    public bool running;
    public bool dead;
    public bool activeskill1;
    public bool activeskill2;
    public bool canSkill1;
    public bool Skill1_ing;
    public bool Skill1_isCD;
    public bool Skill2_ing;
    public bool canSkill2;
    public bool canBeAttacked;
    public bool canHeal;

    public Rigidbody2D rgbody;
    public Animator anim;
    public Slider slider;
    public Slider heart_Slider;

    public GameObject HeartSpawn;
    public GameObject heart;

    public static Boss3 instance;

    private void Start()
    {
        CinemachineBrain cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        Slider[] sliders = cinemachineBrain.GetComponentsInChildren<Slider>(true);
        slider = sliders[0];
        heart_Slider = sliders[1];

        rgbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canBeAttacked = true;
        activeskill1 = false;
        activeskill2 = false;
        canSkill2 = false;
        canAttack = true;
        canRun = true;
        attacking = false;
        instance = this;
        canSkill1 = false;
        Skill1_isCD = false;
        Skill1_ing = false;
        Skill2_ing = false;
        curhp = hp;
        curhp_heart = hp_heart;
    }
    private void Update()
    {
        if (dead == false)
        {
            if (!dead && !Skill1_ing && !Skill2_ing)
            {
                Debug.Log("x: " + player.position.x + " y: " + player.position.y);
                LookAtPlayer();
                RunToPlayer();
            }
            ShowSlider();
            if (curhp <= 0)
            {
                dead = true;
                // ==============================================WIN==================================================
                anim.Play("Death");
                rgbody.bodyType = RigidbodyType2D.Static;

                if (SystemData.instance.map == 3)
                {
                    SystemData.instance.amountBossMap_3--;
             
                }
                SystemData.instance.flagBoss += 1;
                GameManager.instance.SetStateGame(GameManager.StateGame.GameOver);
            }
            if (Time.time > nextAttackSkill1 && activeskill1 == true && !Skill2_ing)
            {
                Skill1_isCD = false;
                canSkill1 = true;
                Skill1();
                nextAttackSkill1 = Time.time + cdSkill1;
            }
            else
            {
                canSkill1 = false;
                Skill1_isCD = true;
            }

            if (curhp < (int)(30 * hp / 100) && activeskill2 == false)
            {
                activeskill2 = true;
                canSkill2 = true;
            }

            Skill2();
            HealingSystem();
            if (curhp < (int)(80 * hp / 100) && activeskill1 == false)
            {
                activeskill1 = true;
            }
        }
    }

    public void setTransform(Transform transform)
    {

        player = transform;
    }
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;


        if (player.position.x > LeftBound && player.position.x < RightBound && player.position.y < TopBound && player.position.y > BotBound && !attacking && !Skill1_ing && !Skill2_ing)
        {
            // xoay boss
            if (transform.position.x > player.position.x && !isFlipped)
            {
                //Neu enemy dang di tu trai qua phai va isFlipped = false
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
            else if (transform.position.x < player.position.x && isFlipped)
            {
                //Neu enemy dang di tu phai qua trai va isFlipped = true
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
        }
    }

    public void RunToPlayer()
    {
        if (player.position.x > LeftBound && player.position.x < RightBound && player.position.y < TopBound && player.position.y > BotBound && canRun && !attacking && !Skill1_ing && !Skill2_ing)
        {
            running = true;
            if (transform.position.x > player.position.x)
            {
                if (running && canRun)
                {
                    rgbody.velocity = new Vector2(-speedRun, rgbody.velocity.y);
                    anim.SetBool("Run", true);
                }
            }
            else if (transform.position.x < player.position.x)
            {
                if (running && canRun)
                {
                    rgbody.velocity = new Vector2(speedRun, rgbody.velocity.y);
                    anim.SetBool("Run", true);
                }
            }
        }
    }

    public void ShowSlider()
    {
        if (player.position.x > LeftBound && player.position.x < RightBound && player.position.y < TopBound && player.position.y > BotBound)
        {

            slider.gameObject.SetActive(true);
            slider.minValue = 0;
            slider.maxValue = hp;
            slider.value = curhp;
        }
    }
    public void TakeDame(int dame)
    {
        if (canBeAttacked)
        {
            curhp -= dame;
            slider.value = curhp;
        }
    }
    /// <summary>
    /// ////////////////////////////////////////////
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            if (CharacterObject.instance.isUltimate == true)
            {
                TakeDame(InGameCharLoading.instance.damage * 2);
            }
            else if (CharacterObject.instance.isUltimate == false)
            {
                TakeDame(InGameCharLoading.instance.damage);
            }
        }
    }

    public void Skill1()
    {
        if (canSkill1 == true && Skill1_isCD == false)
        {
            canRun = false;
            canSkill1 = false;
            Skill1_ing = true;
            Skill1_isCD = true;
            Instantiate(Resources.Load("Prefabs/BossSkill/Skill1_DarkWizard"), player.transform.position, Quaternion.identity);
            anim.SetTrigger("Skill1");
        }
    }

    public void Skill2()
    {
        if (activeskill2 == true && canSkill2)
        {
            canHeal = true;
            canBeAttacked = false;
            Skill2_ing = true;
            anim.SetBool("Skill2", true);
            anim.SetTrigger("ActiveSkill2");
            heart = (GameObject)Instantiate(Resources.Load("Prefabs/Boss/Weakness_Boss3"), HeartSpawn.transform.position, Quaternion.identity);
            heart_Slider.gameObject.SetActive(true);
            heart_Slider.minValue = 0;
            heart_Slider.maxValue = hp_heart;
            heart_Slider.value = curhp_heart;
            canSkill2 = false;
        }

        if (curhp_heart <= 0)
        {
            anim.SetBool("Skill2", false);
            canBeAttacked = true;
            canHeal = false;
            Destroy(heart);
        }
        if (curhp >= hp && Skill2_ing)
        {
            curhp = hp;
            anim.Play("Run");
            canBeAttacked = true;
            canSkill2 = false;
            Destroy(heart);
            anim.SetBool("Skill2", false);
            Skill2_ing = false;
            canHeal = false;
        }
    }
    public void TakeDameHeart(int dame)
    {
        if (curhp_heart > 0)
        {

            curhp_heart -= dame;
            heart_Slider.value = curhp_heart;
        }
    }
    public void HealingSystem()
    {
        if (Time.time > nextAttackSkill2)
        {

            if (canHeal)
            {
                Heal(healAmount);
            }
            nextAttackSkill2 = Time.time + cdHealing;
        }

    }
    public void Heal(int healAmount)
    {
        if (curhp > 0 && curhp < hp)
        {
            curhp += healAmount;
            slider.value = curhp;
        }
    }
}

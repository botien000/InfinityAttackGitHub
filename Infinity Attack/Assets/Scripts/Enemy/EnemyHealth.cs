using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int health;
    public bool takeDamage = false;
    public float time, timeLoop, timeDead;
    public bool isInvulnerable = false;
    public EnemyHealthBar healthBar;

    public static EnemyHealth instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);
    }
    private void Update()
    {
        if (takeDamage)
        {
            Hurt();
            isInvulnerable = true;
            time -= Time.deltaTime;
            if (time < 0)
            {
                GetComponent<Animator>().SetBool("Hit", false);
                isInvulnerable = false;
                takeDamage = false;
                time = timeLoop;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        takeDamage = true;
        if (isInvulnerable)
        {
            return;
        }

        health -= damage;
        healthBar.SetHealth(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }

        void Die()
        {
            GetComponent<Animator>().SetBool("Dead", true);
            Destroy(gameObject, timeDead);
        }


    }
    public void Hurt()
    {
        GetComponent<Animator>().SetBool("Hit", true);
    }

   
}

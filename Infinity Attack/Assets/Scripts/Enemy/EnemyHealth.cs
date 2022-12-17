using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public float knockback;
    public bool takeDamage = false;
    public float time, timeLoop, timeDead;
    public bool isInvulnerable = false;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    public EnemyHealthBar healthBar;
    public static EnemyHealth instance;
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);
    }
    private void Update()
    {
            if (isInvulnerable)
            {
                rb.velocity = Vector2.zero;
            }

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
        if (isInvulnerable)
        {
            return;
        }
        else
        {
            takeDamage = true;
            health -= damage;
            healthBar.SetHealth(health, maxHealth);
        }
        
        if (health <= 0)
        {
            Die();
        }

        void Die()
        {
            GetComponent<Animator>().SetBool("Dead", true);
            rb.bodyType = RigidbodyType2D.Static;
            bc.isTrigger = true;
            Destroy(gameObject, timeDead);
            SystemData.instance.FlagDataEnemy();           
        }
    }
    public void Hurt()
    {
        GetComponent<Animator>().SetBool("Hit", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" && health > 0)
        {
            if (PlayerAttack.instance.box != null)
            {
                Vector2 difference = (transform.position - collision.transform.position).normalized;
                Vector2 force = difference * knockback;
                rb.AddForce(difference * force, ForceMode2D.Impulse);
                TakeDamage(InGameCharLoading.instance.damage);
            }
        }
    }

}

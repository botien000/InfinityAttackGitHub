using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    private Transform player;
    public bool chase = false;
    private Animator animator;
    public float CDAtk;
    private float time = 0;
    Vector2 startingPoint;

    public int health;
    public int maxHealth;
    public float knockback;
    public bool takeDamage = false;
    public float timeLoop, timeDead;
    public bool isInvulnerable = false;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private bool die = false;
    public EnemyHealthBar healthBar;
    public static FlyingEnemy instance;
    private bool colGround = false;
    private bool colPlayer = false;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        startingPoint = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);
    }

    
    void Update()
    {
        time -= Time.deltaTime;
        if(player == null)
        {
            return;
        }
        else
        {
            if (!die)
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
                        animator.SetBool("Hit", false);
                        isInvulnerable = false;
                        takeDamage = false;
                        time = timeLoop;
                    }
                }
                else
                {
                    if (chase)
                    {
                        Chase();
                    }
                    else
                    {
                        ReturnToStartPoint();
                    }
                    Flip();
                }
            }
            else
            {
                rb.velocity = Vector2.zero;             
            }
        }
            
    }

    private void Flip()
    {
        if(transform.position.x < player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void Chase()
    {
        Vector2 target = player.position;
        target.y -= 0.5f;
        if (Vector2.Distance(transform.position, target) > 1.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            if(time < 0)
            {
                animator.SetTrigger("Attack");
                time = CDAtk;
            }          
        }
    }

    public void TakeDamageFlyingEnemy(int damage)
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
            die = true;
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("Dead", true);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = speed * 3;
    }

    private void Hurt()
    {
        animator.SetBool("Hit", true);
    }
    private void ReturnToStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint, speed * Time.deltaTime);
    }

    public void SetPlayerTransform(Transform transform)
    {
        this.player = transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && die && !colPlayer)
        {
            colGround = true;           
            rb.bodyType = RigidbodyType2D.Static;
            bc.isTrigger = true;
            Destroy(gameObject, timeDead);
            SystemData.instance.FlagDataEnemy();
        }

        if(collision.gameObject.tag == "Player" && die && !colGround)
        {
            colPlayer = true;
            rb.bodyType = RigidbodyType2D.Static;
            bc.isTrigger = true;
            Destroy(gameObject, timeDead);
            SystemData.instance.FlagDataEnemy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" && health > 0)
        {
            if (PlayerAttack.instance.box != null)
            {
                if (PlayerAttack.instance.box != null)
                {
                    if (!CharacterObject.instance.isUltimate)
                    {
                        Vector2 difference = (transform.position - collision.transform.position).normalized;
                        Vector2 force = difference * knockback;
                        rb.AddForce(difference * force, ForceMode2D.Impulse);
                        TakeDamageFlyingEnemy(InGameCharLoading.instance.damage);
                    }
                    else
                    {
                        Vector2 difference = (transform.position - collision.transform.position).normalized;
                        Vector2 force = difference * knockback * 2;
                        rb.AddForce(difference * force, ForceMode2D.Impulse);
                        TakeDamageFlyingEnemy(InGameCharLoading.instance.damage * 4);
                    }

                }
            }
        }
    }

    internal void TakeDamageFromFire(int damage, Vector3 position)
    {
        position.x += 2;
        position.y += 2;
        if (health > 0)
        {
            Vector2 difference = (transform.position - new Vector3(position.x, position.y, 0)).normalized;
            Vector2 force = difference * knockback * 100000;
            Debug.Log($"force: {force}");
            rb.AddForce(force, ForceMode2D.Impulse);
            TakeDamageFlyingEnemy(damage);
        }
    }
}

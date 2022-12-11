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
    public bool takeDamage = false;
    public float timeLoop, timeDead;
    public bool isInvulnerable = false;
    private Vector2 velocityDamaged;
    private Rigidbody2D rb;
    private bool die = false;

    public static FlyingEnemy instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        startingPoint = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        velocityDamaged = new Vector2(0, 0);
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
                    rb.velocity = velocityDamaged;
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
                Die();
                rb.velocity = velocityDamaged;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = speed * 3;
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
        Debug.Log("Take damage: " + damage);
        if (isInvulnerable)
        {
            return;
        }
        else
        {
            takeDamage = true;
            health -= damage;
        }

        if (health <= 0)
        {
            die = true;
        }
    }

    private void Die()
    {
        animator.SetBool("Dead", true);
    }

    private void Hurt()
    {
        animator.SetBool("Hit", true);
    }
    private void ReturnToStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint, speed * Time.deltaTime);
    }

    public void setTransform(Transform transform)
    {
        this.player = transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject, timeDead);
            SystemData.instance.FlagDataEnemy();
        }
    }
}

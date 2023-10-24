using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] float speedMove;
    [SerializeField] int damage;

    int dir;
    private void Start()
    {
        Destroy(gameObject, 20f);
    }
    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * dir * speedMove * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            animator.SetBool("Hit", true);
            collision.GetComponent<EnemyHealth>().TakeDamageFromFire(damage, transform.position);
            speedMove = 0;
        }
        else if (collision.CompareTag("Boss2"))
        {
            animator.SetBool("Hit", true);
            collision.GetComponent<Boss2>().TakeDame(damage);
            speedMove = 0;
        }
        else if (collision.CompareTag("Boss3"))
        {
            animator.SetBool("Hit", true);
            collision.GetComponent<Boss3>().TakeDame(damage);
            speedMove = 0;
        }
        else if (collision.CompareTag("Ground"))
        {
            animator.SetBool("Hit", true);
            speedMove = 0;
        }
        else if (collision.CompareTag("FlyingEnemy"))
        {
            speedMove = 0;
            animator.SetBool("Hit", true);
            collision.GetComponent<FlyingEnemy>().TakeDamageFromFire(damage, transform.position);
        }
    }

    public void Init(int direction)
    {
        dir = direction;
        transform.localScale = new Vector2(transform.localScale.x * dir, transform.localScale.y);
    }

    public void HandleEventDestroy()
    {
        Destroy(gameObject);
    }
}

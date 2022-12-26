using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] float speedMove;
    [SerializeField] float damage;


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
        }
        else if (collision.CompareTag("Boss2"))
        {
            animator.SetBool("Hit", true);

        }
        else if (collision.CompareTag("Boss3"))
        {
            animator.SetBool("Hit", true);
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

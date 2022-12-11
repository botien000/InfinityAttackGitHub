using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] float speedMove;

    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * speedMove * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            animator.SetBool("Hit", true);
        }
    }
    
    public void HandleEventDestroy()
    {
        Destroy(gameObject);
    }
}

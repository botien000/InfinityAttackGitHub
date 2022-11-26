using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public bool isFlipped = false;
    public EnemyHealthBar healthBar;
    public float LeftBound;
    public float RightBound;
    public float cooldownAtk;
    public float speed;
    public float attackRange;
    public float detectionRange;
    private void Awake()
    {
      
    }

    public void setTransform(Transform transform)
    {
        player = transform;
    }
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

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

    public void LookAtBound(bool hitBound, bool moveToRight)
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (hitBound && moveToRight && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
        else if (hitBound && !moveToRight && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
    }

    public void checkMove(bool moveToRight)
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (moveToRight && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (!moveToRight && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void SetPositionHealthBar(bool atk, bool hit)
    {
        if (!atk && !hit)
        {
            if (isFlipped)
            {
                healthBar.SetPositionHealthBarMoveToLeft();
            }
            else
            {
                healthBar.SetPositionHealthBarMoveToRight();
            }
        }
        
        if(atk && !hit)
        {

            if (isFlipped)
            {
                healthBar.SetPositionHealthBarAttackLeft();
            }
            else
            {
                healthBar.SetPositionHealthBarAttackRight();
            }
        }
        
        if (hit && !atk)
        {

            if (isFlipped)
            {
                healthBar.SetPositionHealthBarTakeDamageLeft();
            }
            else
            {
                healthBar.SetPositionHealthBarTakeDamageRight();
            }
        }

    }
}

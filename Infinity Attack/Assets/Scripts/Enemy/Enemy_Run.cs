using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Run : StateMachineBehaviour
{
    private float speed;
    private float attackRange;
    private float detectionRange;
    private float LeftBound;
    private float RightBound;
    private float distance;
    private float time = 0, timeLoop;
    Rigidbody2D rb;
    Enemy enemy;
    EnemyHealth health;
    Transform player;
    Vector2 target;
    Vector2 newPos;
    private bool moveToRight = true;
    private bool hitBound = false;
    private bool atk = false;



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();
        health = animator.GetComponent<EnemyHealth>();

        LeftBound = enemy.LeftBound;
        RightBound = enemy.RightBound;
        timeLoop = enemy.cooldownAtk;
        speed = enemy.speed;
        attackRange = enemy.attackRange;
        detectionRange = enemy.detectionRange;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = enemy.player;
        if (!player)
            return;
        distance = Vector2.Distance(player.position, rb.position);
        atk = false;
        time -= Time.deltaTime;
        if (moveToRight)
        {
            enemy.SetPositionHealthBar(atk, health.takeDamage);
            enemy.checkMove(moveToRight);
            target = new Vector2(RightBound, rb.position.y);
            newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (distance < detectionRange)
            {
                enemy.LookAtPlayer();
                target = new Vector2(player.position.x, rb.position.y);
                newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);

                if (distance <= attackRange)
                {
                    if (time < 0)
                    {
                        atk = true;
                        enemy.SetPositionHealthBar(atk, health.takeDamage);
                        animator.SetTrigger("Attack");
                        time = timeLoop;
                    }
                }
            }

            if (rb.position.x == RightBound - 0.01)
            {
                hitBound = true;
                enemy.LookAtBound(hitBound, moveToRight);
            }

            if (rb.position.x >= RightBound - 0.01)
            {
                moveToRight = false;
                hitBound = false;
            }
        }

        if (!moveToRight)
        {
            enemy.SetPositionHealthBar(atk, health.takeDamage);
            enemy.checkMove(moveToRight);
            target = new Vector2(LeftBound, rb.position.y);
            newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (distance < detectionRange)
            {
                enemy.LookAtPlayer();
                target = new Vector2(player.position.x, rb.position.y);
                newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);

                if (distance <= attackRange)
                {
                    if (time < 0)
                    {
                        atk = true;
                        enemy.SetPositionHealthBar(atk, health.takeDamage);
                        animator.SetTrigger("Attack");
                        time = timeLoop;
                    }
                }
            }

            if (rb.position.x == LeftBound + 0.01)
            {
                hitBound = true;
                enemy.LookAtBound(hitBound, moveToRight);
            }

            if (rb.position.x <= LeftBound + 0.01)
            {
                moveToRight = true;
                hitBound = false;
            }
        }

    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    internal void SetPlayer(Transform player)
    {
        this.player = player;
    }
}

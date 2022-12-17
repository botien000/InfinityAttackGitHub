using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Run : StateMachineBehaviour
{
    private float speed;
    private float attackRange;
    private float detectionRange;
    private float heightRange;
    private float LeftBound;
    private float RightBound;
    private float distance;
    private float height;
    private float heightPlayer;
    private float heightEnemy;
    private float time = 0, timeLoop;
    Rigidbody2D rb;
    Enemy enemy;
    EnemyHealth health;
    Transform player;
    Vector2 target;
    Vector2 newPos;
    private bool moveToRight = true;
    private bool hitBound = false;
    private bool hit = false;



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
        heightRange = enemy.heightRange;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = enemy.player;
        if (!player)
            return;
        hit = health.isInvulnerable;
        if (!hit)
        {
            distance = Vector2.Distance(player.position, rb.position);
            time -= Time.deltaTime;
            if (moveToRight)
            {
                enemy.checkMove(moveToRight);
                target = new Vector2(RightBound, rb.position.y);
                newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);

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

                heightPlayer = player.position.y;
                heightEnemy = rb.position.y;
                height = Math.Abs(Math.Abs(heightPlayer) - Math.Abs(heightEnemy));

                if (height < heightRange)
                {
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
                                animator.SetTrigger("Attack");
                                time = timeLoop;
                            }
                        }
                    }                
                }
                

            }

            if (!moveToRight)
            {
                enemy.checkMove(moveToRight);
                target = new Vector2(LeftBound, rb.position.y);
                newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);

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

                heightPlayer = player.position.y;
                heightEnemy = rb.position.y;
                height = Math.Abs(Math.Abs(heightPlayer) - Math.Abs(heightEnemy));

                if (height < heightRange)
                {
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
                                animator.SetTrigger("Attack");
                                time = timeLoop;
                            }
                        }
                    }               
                }             
            }
                    
        }
        else
        {         
            rb.velocity = Vector2.zero;
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

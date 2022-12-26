using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class Dark_Samurai : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rgbody;
    [SerializeField] private Transform[] transPositions;
    [SerializeField] private int velocityRun, hp, damageAttack, damageUltimate;

    private int energy;
    private int countAttack;
    private bool finishIdle;

    private Transform transPlayer;
    enum State
    {
        Idle, Run, Attack, DoubleAttack, Ultimate, Hit, Die
    }

    private State curState;
    private State preState;

    private void Start()
    {
        StartCoroutine(IEAction());
        preState = State.Die;
    }
    private void Update()
    {
        if (transPlayer == null || curState == State.Hit || curState == State.Die)
            return;

        if (finishIdle) // situlation action
        {
            float x = transPlayer.position.x - transform.position.x;
            Direction(x);
            if (curState == State.Idle && energy == 8) //samurai active ultimate
            {
                PlayAni(State.Ultimate);
            }
            else if (x <= 2 && x >= -2) // samurai attack
            {
                rgbody.velocity = Vector2.zero;
                energy++;
                if (countAttack == 3) // samurai attack 3 times then double attack
                {
                    PlayAni(State.DoubleAttack);
                    countAttack = 0;

                }
                else // samurai attack +1
                {
                    PlayAni(State.Attack);
                    countAttack++;
                }
            }
            else if (x > 2 || x < -2) // samurai run
            {
                rgbody.velocity = Vector2.right * velocityRun * x;
                PlayAni(State.Run);
            }

        }
    }
    private IEnumerator IEAction()
    {
        while (true)
        {
            yield return new WaitUntil(() => transPlayer != null);
            Debug.Log(curState);
            if (curState != preState)
            {
                preState = curState;
                switch (curState)
                {
                    case State.Idle:
                        finishIdle = false;
                        yield return new WaitForSeconds(2f);
                        finishIdle = true;
                        break;
                    case State.Run:
                        break;
                    case State.Attack:
                        yield return new WaitForSeconds(2f);
                        PlayAni(State.Idle);
                        break;
                    case State.DoubleAttack:
                        yield return new WaitForSeconds(2f);
                        PlayAni(State.Idle);
                        break;
                    case State.Hit:
                        yield return new WaitForSeconds(2f);
                        PlayAni(State.Idle);
                        break;
                    case State.Ultimate:
                        break;
                }
            }
            yield return null;
        }
    }

    private void Direction(float direction)
    {
        if(direction > 0) // right
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = Vector3.up * 180;
        }
    }
    private void Attack()
    {
        curState = State.Attack;
    }

    private void DoubleAttack()
    {

    }

    private void Ultimate()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public void Hit()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void Die()
    {

    }

    private void TelePort(Transform trans)
    {
        transform.position = trans.position;
        PlayAni(State.Idle);
    }

    private void PlayAni(State state)
    {

        if (curState == state)
            return;

        animator.SetInteger("State", (int)state);
        animator.SetTrigger("Change");
        curState = state;
        Debug.Log("Cur State: " + curState);
    }

    public void Active(Transform player)
    {
        transPlayer = player;
    }
}

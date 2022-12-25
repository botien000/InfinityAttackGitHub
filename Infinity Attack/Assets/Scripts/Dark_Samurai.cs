using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Dark_Samurai : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rgbody;
    [SerializeField] private Transform[] transPositions;
    [SerializeField] private int velocityRun, hp, damageAttack, damageUltimate;

    private int energy;
    private int countAttack;
    private bool isHit;


    private Transform transPlayer;
    enum State
    {
        Idle, Run, Attack, DoubleAttack, Ultimate, Hit, Die
    }

    private State curState;
    private State preState;

    private void Update()
    {
        if (transPlayer == null)
            return;

        float x = transPlayer.position.x - transform.position.x;
        if (curState == State.Run && (x <= 2 && x >= -2)) // samurai attack
        {
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
            PlayAni(State.Run);
        }
        else if (curState == State.Idle && energy == 8) //samurai active ultimate
        {
            PlayAni(State.Ultimate);
        }
    }
    private IEnumerator IEAction()
    {
        while (true)
        {
            if (curState != preState)
            {
                preState = curState;
                switch (curState)
                {
                    case State.Idle:
                        break;
                    case State.Run:
                        break;
                    case State.Attack:
                        yield return new WaitForSeconds(2f);
                        TelePort(transPositions[Random.Range(0, transPositions.Length)]);
                        PlayAni(State.Idle);
                        break;
                    case State.DoubleAttack:
                        yield return new WaitForSeconds(2f);
                        TelePort(transPositions[Random.Range(0, transPositions.Length)]);
                        PlayAni(State.Idle);
                        break;
                    case State.Hit:
                        Hit();
                        yield return new WaitForSeconds(2f);
                        TelePort(transPositions[Random.Range(0, transPositions.Length)]);
                        PlayAni(State.Idle);
                        break;
                    case State.Ultimate:
                        break;
                }
            }
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
        preState = curState;
    }

    public void Active(CharacterObject player)
    {
        transPlayer = player.transform;
    }
}

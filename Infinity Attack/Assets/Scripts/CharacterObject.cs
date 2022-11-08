using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterObject : MonoBehaviour
{
    enum State
    {
        Idle,
        Run,
        JumpUp, JumpDown, Attack_Air,
        Attack_1, Attack_2, Attack_3,
        Utilmate,
        Hit,
        Die
    }
    [SerializeField] private float speedRun;
    [SerializeField] private float forceJump;

    private State curState;
    private Rigidbody2D rgbody;
    private Animator animator;
    private InputController playerInput;
    private Vector2 movePlayer;

    private bool isGround;
    private bool isJump;
    private float curY;
    private float preY;
    private bool attacking = false;
    private float nextAttackTime = 0f;
    private float curChar_Attack1_duration = 1.8f;
    private float FireKnight_Attack1_duration = 1.8f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rgbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isJump)
        {
            Debug.Log("Jumping");
            curY = transform.position.y;
            if (curY < preY)
            {
                SetAnimation(State.JumpDown);
            }
            else
            {
                preY = curY;
            }
        }

        if (attacking = true && curState == State.Attack_1)
        {
            movePlayer = Vector2.zero;
            if (Time.time >= nextAttackTime)
            {
                attacking = false;
                if (!isJump)
                {
                    if (movePlayer == Vector2.zero)
                    {
                        SetAnimation(State.Idle);
                    }
                    else if (movePlayer.x > 0 || movePlayer.x < 0)
                    {
                        SetAnimation(State.Run);
                    }

                }
            }
        }
        Debug.Log("attacking: " + attacking);
        Debug.Log("curState: " + curState);
    }

    private void OnEnable()
    {

        if (playerInput == null)
        {
            playerInput = new InputController();
            playerInput.Player.Movement.started += OnMovement;
            playerInput.Player.Movement.performed += OnMovement;
            playerInput.Player.Movement.canceled += OnMovement;
            playerInput.Player.Attack.started += OnAttack;
            playerInput.Player.Attack.performed += OnAttack;
            playerInput.Player.Attack.canceled += OnAttack;
            playerInput.Player.Jump.started += OnJump;
            playerInput.Player.Jump.performed += OnJump;
            playerInput.Player.Jump.canceled += OnJump;
        }
        playerInput.Enable();
    }
    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.Disable();
        }
    }
    private void FixedUpdate()
    {
        rgbody.velocity = new Vector2(movePlayer.x * speedRun, rgbody.velocity.y);
    }
    private void SetDirection()
    {
        if (movePlayer.x < 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        else if (movePlayer.x > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.zero);
        }
    }
    private void OnJump(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            if (!isJump)
            {
                if (!attacking)
                {
                    SetAnimation(State.JumpUp);
                    rgbody.AddForce(Vector2.up * forceJump, ForceMode2D.Force);
                    preY = transform.position.y;
                }
            }
        }
    }
    private void OnAttack(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            if (!isJump && (curState == State.Idle || curState == State.Run))
            {
                attacking = true;
                Debug.Log("Time: " + Time.time);
                SetAnimation(State.Attack_1);
                nextAttackTime = Time.time + 1f / curChar_Attack1_duration;
            }
            else if (isJump)
            {
                SetAnimation(State.Attack_Air);
            }
        }
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        if (!attacking)
        {
            SetDirection();
            if (obj.started || obj.performed)
            {
                movePlayer = obj.ReadValue<Vector2>();
                Debug.Log("Move: " + movePlayer);
                if (!isJump)
                {
                    SetAnimation(State.Run);
                }
            }
            else
            {
                movePlayer = Vector2.zero;
                if (!isJump)
                {
                    SetAnimation(State.Idle);
                }
            }
        }
    }
    private void Hit()
    {

    }
    private void Utilmate()
    {

    }
    private void Die()
    {

    }
    /// <summary>
    /// Character's state
    /// </summary>
    /// <param name="state"></param>
    private void SetAnimation(State state)
    {
        if (curState == state)
            return;
        animator.SetInteger("State", (int)state);
        animator.SetTrigger("Change");
        curState = state;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && collision.otherCollider.name == "Foot")
        {
            isJump = false;
            isGround = true;
            if (movePlayer == Vector2.zero)
            {
                SetAnimation(State.Idle);
            }
            else
            {
                SetAnimation(State.Run);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Ground") && collision.otherCollider.name == "Foot")
        {
            isGround = false;
            isJump = true;
        }
    }
}

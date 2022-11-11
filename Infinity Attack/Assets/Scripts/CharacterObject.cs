using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static CharacterObject instance;

    private State curState;
    private Rigidbody2D rgbody;
    public Animator animator;
    private InputController playerInput;
    public Vector2 movePlayer;
    public Vector2 preMovePlayer;

    private bool isGround;
    public bool isJump;
    private float curY;
    private float preY;
    public bool attacking = false;
    public bool jumpAttacking = false;
    public bool nextAttack = false;
    public bool movingPressing = false;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        rgbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
        SetDirection();
        if (!attacking)
        {
            if (!isJump)
            {
                if (movingPressing)
                {
                    SetAnimation(State.Run);
                    movePlayer = preMovePlayer;
                }
                else if (!movingPressing)
                {
                    movePlayer = Vector2.zero;
                    preMovePlayer = Vector2.zero;
                    SetAnimation(State.Idle);
                }
            }
            else if (isJump)
            {
                if (!jumpAttacking)
                {
                    curY = transform.position.y;
                    if (curY < preY)
                    {
                        SetAnimation(State.JumpDown);
                        preY = curY;
                    }
                    else if (curY > preY)
                    {
                        SetAnimation(State.JumpUp);
                        preY = curY;
                    }
                }
            }
        }

        /*if (jumpAttacking)
        {
            SetAnimation(State.Attack_Air);
        }*/
        Debug.Log("Attacking: " + attacking);
        Debug.Log("MovePlayer: " + movePlayer);
        Debug.Log("preMovePlayer: " + preMovePlayer);
        Debug.Log("NextAttack: " + nextAttack);
        Debug.Log("MovingPressing: " + movingPressing);
        Debug.Log("isJump: " + isJump);
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
            if (!isJump)
            {
                movePlayer = Vector2.zero;
                attacking = true;
                nextAttack = true;
            }
            else if (isJump)
            {
                jumpAttacking = true;
            }
        }
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            if (!attacking)
            {
                movePlayer = obj.ReadValue<Vector2>();
            }
            movingPressing = true;
            preMovePlayer = obj.ReadValue<Vector2>();
        }
        if (obj.canceled)
        {
            movingPressing = false;
            movePlayer = Vector2.zero;
            preMovePlayer = Vector2.zero;
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
            jumpAttacking = false;
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
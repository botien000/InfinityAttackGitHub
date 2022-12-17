using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] private float ultimateCooldown;
    [SerializeField] private Button abilityCooldownButton;

    public static CharacterObject instance;

    private BoxCollider2D box;

    private State curState;
    public Rigidbody2D rgbody;
    public Animator animator;
    private InputController playerInput;
    [SerializeField] public Vector2 movePlayer;
    public Vector2 preMovePlayer;

    private bool isGround;
    public bool isJump;
    private float curY;
    private float preY;
    public bool attacking = false;
    public bool jumpAttacking = false;
    public bool nextAttack = false;
    public bool movingPressing = false;
    public bool isUltimate = false;
    public bool isCooldown;
    public bool isAttacked = false;
    public bool dead = false;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        rgbody = GetComponent<Rigidbody2D>();
        dead = false;
    }
    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        SpellSpawner spellSpawner = FindObjectOfType<SpellSpawner>();
        spellSpawner.Init(this);
        GameManager.instance.SetPlayerDontDestroy(this);
        FindObjects();
    }

    private void Update()
    {
        if (!dead)
        {
            if (!isAttacked || !dead || !attacking)
            {
                SetDirection();
            }
            if (!attacking || !isUltimate || !isAttacked || !dead)
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
                    if (!jumpAttacking || !isAttacked)
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
        }
        

        if (isCooldown)
        {
            abilityCooldownButton.GetComponent<Image>().fillAmount -= 1 / ultimateCooldown * Time.deltaTime;

            if (abilityCooldownButton.GetComponent<Image>().fillAmount <= 0)
            {
                abilityCooldownButton.GetComponent<Image>().fillAmount = 0;
                isCooldown = false;
            }
        }

        Debug.Log("curstate" + curState);
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
            playerInput.Player.Ultimate.started += Ultimate;
            playerInput.Player.Ultimate.performed += Ultimate;
            playerInput.Player.Ultimate.canceled += Ultimate;
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
        if (!isAttacked &&!dead)
            rgbody.velocity = new Vector2(movePlayer.x * speedRun, rgbody.velocity.y);
        if (InGameCharLoading.instance.curHp <= 0)
        {
            Die();
            movePlayer = Vector2.zero;
            dead = true;
            rgbody.bodyType = RigidbodyType2D.Static;
        }
    }
    private void SetDirection()
    {
        if (!dead)
        {
            if (!attacking)
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
        }
    }
    private void OnJump(InputAction.CallbackContext obj)
    {
        if (!dead)
        {

            if (obj.started)
            {
                if (!isJump && !isUltimate && !isAttacked )
                {
                    if (!attacking)
                    {
                        rgbody.AddForce(Vector2.up * forceJump, ForceMode2D.Force);
                        preY = transform.position.y;
                    }
                }
            }
        }
    }
    private void OnAttack(InputAction.CallbackContext obj)
    {
        if (!dead)
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
    }

    private void OnMovement(InputAction.CallbackContext obj)
    {
        if (!dead)
        {
            if (obj.started)
            {
                if (!attacking || !isUltimate || !isAttacked)
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
    }

    public void Hit()
    {
        if (dead == false && !isAttacked)
        {

            isAttacked = true;
            animator.Play("Hit");
            StartCoroutine(TakeHit());
        }
    }
    public void Ultimate(InputAction.CallbackContext obj)
    {
        if (!dead)
        {
            if (obj.started)
            {
                if (!isJump && !isCooldown && !attacking)
                {
                    isCooldown = true;
                    abilityCooldownButton.GetComponent<Image>().fillAmount = 1;
                    movePlayer = Vector2.zero;
                    isUltimate = true;
                    Debug.Log("dang loi ne");
                    animator.Play("Utilmate");
                }
            }
        }
    }
    private void Die()
    {
        if (dead == false)
        {

            animator.Play("Die");
        }
    }
    /// <summary>
    /// Character's state
    /// </summary>
    /// <param name="state"></param>
    private void SetAnimation(State state)
    {
        if (!dead)
        {
            if (curState == state)
                return;
            animator.SetInteger("State", (int)state);
            animator.SetTrigger("Change");
            curState = state;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dead)
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
            if (!isUltimate)
            {
                if (!isAttacked)
                {
                    if (collision.gameObject.tag == "Enemy")
                    {
                        if (EnemyHealth.instance.health > 0 && !EnemyHealth.instance.takeDamage)
                        {
                            Hit();
                            StartCoroutine(TakeHit());
                            Vector2 difference = (transform.position - collision.transform.position).normalized;

                            Vector2 force = difference * 3f;

                            Debug.Log("Force: " + force);
                            rgbody.AddForce(difference * force, ForceMode2D.Impulse);
                            InGameCharLoading.instance.Damage(EnemyWeapon.instance.attackDamage);
                        }
                    }

                    if (collision.gameObject.tag == "FlyingEnemy")
                    {
                        if (FlyingEnemy.instance.health > 0 && !FlyingEnemy.instance.takeDamage)
                        {
                            Hit();
                            StartCoroutine(TakeHit());
                            Vector2 difference = (transform.position - collision.transform.position).normalized;
                            Vector2 force = difference * 3f;
                            Debug.Log("Force: " + force);
                            rgbody.AddForce(difference * force, ForceMode2D.Impulse);
                            InGameCharLoading.instance.Damage(EnemyWeapon.instance.attackDamage);
                        }
                    }

                    if (collision.gameObject.tag == "FireBall")
                    {
                        Debug.Log("trigger fireball");
                        Hit();
                        Vector2 difference = (transform.position - collision.transform.position).normalized;
                        Vector2 force = difference * 3f;
                        Debug.Log("Force: " + force);
                        rgbody.AddForce(difference * force, ForceMode2D.Impulse);
                        StartCoroutine(TakeHit());
                    }
                }             
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!dead)
        {
            if (collision.collider.CompareTag("Ground") && collision.otherCollider.name == "Foot")
            {
                isGround = false;
                isJump = true;
            }
        }
    }
    private void FindObjects()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.setTransform(transform);
        }

        FlyingEnemy[] flyingEs = FindObjectsOfType<FlyingEnemy>();
        foreach (var flyingEnemy in flyingEs)
        {
            flyingEnemy.setTransform(transform);
        }

        Minimap minimap = FindObjectOfType<Minimap>();
        minimap.SetTransform(transform);
    }
    private IEnumerator TakeHit()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacked = false;
        movePlayer = Vector2.zero;
    }

    public void insertBtnCooldown(Button btn)
    {
        abilityCooldownButton = btn;
    }

    public void SpeedUp(float speed)
    {
        speedRun += speed;
    }
    public void Healing(float hp)
    {

    }
    public void IncreateDamage(int damage)
    {

    }
    public void ResetingUtilmate()
    {

    }
}
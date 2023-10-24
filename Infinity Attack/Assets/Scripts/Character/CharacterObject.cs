using System;
using System.Collections;
using System.Runtime.CompilerServices;
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
    [SerializeField] private float knockbackForceUp;
    [SerializeField] private float knockbackForce;

    public static CharacterObject instance;
    private BoxCollider2D box;

    private Image imgAniOpeningMap;
    private State curState;
    private ParticleSystem speedUpFX;
    public Rigidbody2D rgbody;
    public Animator animator;
    private InputController playerInput;
    [SerializeField] public Vector2 movePlayer;
    public Vector2 preMovePlayer;

    private bool isFreeze;
    private bool isGround;
    private float countTimeFX;
    private float timeSpawnSUFX = 0.15f;
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

    private Vector3 spawnPosition;
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        rgbody = GetComponent<Rigidbody2D>();
        dead = false;
    }
    private void Start()
    {
        spawnPosition = InGameCharLoading.instance.spawnPosition.position;
        FindObjects();
        box = GetComponent<BoxCollider2D>();
        SpellSpawner spellSpawner = FindObjectOfType<SpellSpawner>();
        spellSpawner.Init(this);
        GameManager.instance.SetPlayerDontDestroy(this);
        abilityCooldownButton.GetComponent<Image>().raycastTarget = false;
    }

    private void Update()
    {
        if (dead)
            return;
        if (!dead)
        {
            if (isUltimate)
            {

            }
            if (!isAttacked || !dead)
            {
                if (!attacking)
                {
                    SetDirection();
                }
            }
            if (!attacking || !isUltimate || !isAttacked || !dead)
            {
                if (!isJump)
                {
                    if (!attacking)
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
                }
                else if (isJump && !dead)
                {
                    if (!jumpAttacking || !isAttacked)
                    {
                        if (!dead)
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
        }


        if (isCooldown)
        {
            abilityCooldownButton.GetComponent<Image>().raycastTarget = true;
            abilityCooldownButton.GetComponent<Image>().fillAmount -= 1 / ultimateCooldown * Time.deltaTime;

            if (abilityCooldownButton.GetComponent<Image>().fillAmount <= 0)
            {
                abilityCooldownButton.GetComponent<Image>().raycastTarget = false;
                abilityCooldownButton.GetComponent<Image>().fillAmount = 0;
                isCooldown = false;
            }
        }

        if (Enemy.instance.player == null && !dead)
        {
            FindObjects();
            //transform.position = spawnPosition;
        }
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
        if (dead)
            return;
        if (!isAttacked && !dead)
        {
            rgbody.velocity = new Vector2(movePlayer.x * speedRun, rgbody.velocity.y);
            if (Mathf.Abs(rgbody.velocity.x) > 0 && !isJump)
                ExcuteSpeedingUpFx();
        }
        if (InGameCharLoading.instance.curHp <= 0)
        {
            StartCoroutine(Die());
        }
        if (isUltimate && !dead)
        {
            rgbody.bodyType = RigidbodyType2D.Static;
        }
        else if (!isUltimate && !dead)
        {
            rgbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void ExcuteSpeedingUpFx()
    {
        if (speedUpFX == null)
            return;

        countTimeFX += Time.deltaTime;
        if (countTimeFX >= timeSpawnSUFX)
        {
            speedUpFX.Play();
            countTimeFX = 0;
        }
    }

    private void SetDirection()
    {
        if (!dead)
        {
            if (!attacking && !isUltimate)
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
        if (GameManager.instance.GetActiveConversation() || isFreeze)
        {
            return;
        }
        if (!dead && !isFreeze)
        {
            if (obj.started)
            {
                if (!isJump && !isUltimate && !isAttacked)
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
        if (GameManager.instance.GetActiveConversation() || isFreeze)
        {
            return;
        }
        if (!dead && !isFreeze)
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

        if (GameManager.instance.GetActiveConversation() || isFreeze)
        {
            return;
        }
        if (!dead && !isFreeze)
        {
            if (obj.started)
            {
                if (!attacking)
                {
                    if (!attacking || !isUltimate || !isAttacked)
                    {
                        movePlayer = obj.ReadValue<Vector2>();
                    }
                }
                movingPressing = true;
                preMovePlayer = obj.ReadValue<Vector2>();
            }
            if (obj.canceled)
            {
                //if (speedUpGO != null)
                //    speedUpGO.SetActive(false);
                movingPressing = false;
                if (!attacking || !isUltimate || !isAttacked)
                {
                    if (!attacking)
                    {
                        movePlayer = Vector2.zero;
                        preMovePlayer = Vector2.zero;
                    }
                }
            }
        }
    }

    public void Hit(string tag)
    {
        if (dead == false && !isAttacked)
        {
            Knockback(tag);
            isAttacked = true;
            animator.Play("Hit");
            StartCoroutine(TakeHit());
        }
    }
    public void Knockback(string tag)
    {
        Transform attacker = getClosestDamageSource(tag);
        Vector2 knockbackDirection = new Vector2(transform.position.x - attacker.transform.position.x, 0);
        rgbody.velocity = new Vector2(knockbackDirection.x, knockbackForceUp) * knockbackForce;
    }
    public Transform getClosestDamageSource(string tag)
    {
        GameObject[] DamageSources = GameObject.FindGameObjectsWithTag(tag);
        float closestDistance = Mathf.Infinity;
        Transform currentClosestDamageSource = null;

        foreach (GameObject go in DamageSources)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                currentClosestDamageSource = go.transform;
            }
        }

        return currentClosestDamageSource;
    }
    public void Ultimate(InputAction.CallbackContext obj)
    {
        if (GameManager.instance.GetActiveConversation() || isFreeze)
        {
            return;
        }
        if (!dead && !isFreeze)
        {
            if (obj.started)
            {
                if (!isJump && !isCooldown && !attacking)
                {
                    isCooldown = true;
                    abilityCooldownButton.GetComponent<Image>().fillAmount = 1;
                    movePlayer = Vector2.zero;
                    isUltimate = true;
                    animator.Play("Utilmate");
                }
            }
        }
    }
    private IEnumerator Die()
    {
        if (dead == false)
        {
            //========================================LOSE=================================================
            setNull();
            rgbody.bodyType = RigidbodyType2D.Static;
            animator.Play("Die");
            movePlayer = Vector2.zero;
            dead = true;
            yield return new WaitForSeconds(1.5f);
            GameManager.instance.SetStateGame(GameManager.StateGame.GameOver);

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
                        if (collision.gameObject.GetComponent<EnemyHealth>().health > 0
                            && !collision.gameObject.GetComponent<EnemyHealth>().takeDamage)
                        {
                            Hit("Enemy");
                            StartCoroutine(TakeHit());
                            if (collision.transform.position.x >= transform.position.x)
                            {
                                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                            }
                            else if (collision.transform.position.x < transform.position.x)
                            {
                                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                            }
                            InGameCharLoading.instance.Damage(collision.gameObject.GetComponent<EnemyWeapon>().attackDamage / 2);
                        }
                    }

                    if (collision.gameObject.tag == "FlyingEnemy")
                    {
                        if (collision.gameObject.GetComponent<FlyingEnemy>().health > 0
                            && !collision.gameObject.GetComponent<FlyingEnemy>().takeDamage)
                        {
                            Hit("FlyingEnemy");
                            StartCoroutine(TakeHit());
                            if (collision.transform.position.x >= transform.position.x)
                            {
                                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                            }
                            else if (collision.transform.position.x < transform.position.x)
                            {
                                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                            }
                            InGameCharLoading.instance.Damage(collision.gameObject.GetComponent<EnemyWeapon>().attackDamage / 2);
                        }
                    }

                    if (collision.gameObject.tag == "FireBall")
                    {
                        Hit("FireBall");
                        if (collision.transform.position.x >= transform.position.x)
                        {
                            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                        }
                        else if (collision.transform.position.x < transform.position.x)
                        {
                            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                        }
                        StartCoroutine(TakeHit());
                    }

                    if (collision.gameObject.tag == "Boss2" && !isUltimate)
                    {
                        if (isAttacked == false)
                        {
                            InGameCharLoading.instance.Damage(Boss2.instance.attackDamage);
                        }
                        Hit("Boss2");
                        if (collision.transform.position.x > transform.position.x)
                        {
                            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                        }
                        else if (collision.transform.position.x < transform.position.x)
                        {
                            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                        }
                        StartCoroutine(TakeHit());
                    }

                    if (collision.gameObject.tag == "Boss3" && !isUltimate)
                    {
                        if (isAttacked == false)
                        {
                            InGameCharLoading.instance.Damage(Boss3.instance.attackDamage);
                        }
                        Hit("Boss3");
                        if (collision.transform.position.x > transform.position.x)
                        {
                            transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                        }
                        else if (collision.transform.position.x < transform.position.x)
                        {
                            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                        }
                        StartCoroutine(TakeHit());
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boss2_Attack" && !isUltimate)
        {
            if (isAttacked == false)
            {
                InGameCharLoading.instance.Damage(Boss2.instance.attackDamage);
            }
            Hit("Boss2_Attack");
            if (collision.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if (collision.transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            StartCoroutine(TakeHit());
        }
        ////////////////////////////////////////
        if (collision.gameObject.tag == "Boss3_Attack" && !isUltimate)
        {
            if (isAttacked == false)
            {
                InGameCharLoading.instance.Damage(Boss3.instance.attackDamage);
            }
            Hit("Boss3_Attack");
            if (collision.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if (collision.transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            StartCoroutine(TakeHit());
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
            enemy.SetPlayerTransform(transform);
        }

        FlyingEnemy[] flyingEs = FindObjectsOfType<FlyingEnemy>();
        foreach (var flyingEnemy in flyingEs)
        {
            flyingEnemy.SetPlayerTransform(transform);
        }

        Minimap minimap = FindObjectOfType<Minimap>();
        minimap.SetPlayerTransform(transform);

        Boss2[] boss2Arr = FindObjectsOfType<Boss2>();
        foreach (var boss2 in boss2Arr)
        {
            boss2.SetPlayerTransform(transform);
        }

        Boss3[] boss3Arr = FindObjectsOfType<Boss3>();
        foreach (var boss3 in boss3Arr)
        {
            boss3.SetPlayerTransform(transform);
        }
    }

    private void setNull()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.SetPlayerTransform(null);
        }

        FlyingEnemy[] flyingEs = FindObjectsOfType<FlyingEnemy>();
        foreach (var flyingEnemy in flyingEs)
        {
            flyingEnemy.SetPlayerTransform(null);
        }
    }
    private IEnumerator TakeHit()
    {
        yield return new WaitForSeconds(0.6f);
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
    public void Healing(float percent)
    {
        int hp = (int)(InGameCharLoading.instance.curHp * (percent / 100f));
        InGameCharLoading.instance.curHp += (int)hp;
        if (InGameCharLoading.instance.curHp >= InGameCharLoading.instance.hp)
        {
            InGameCharLoading.instance.curHp = InGameCharLoading.instance.hp;
        }
        InGameCharLoading.instance.HealthBar.fillAmount = (float)InGameCharLoading.instance.curHp / InGameCharLoading.instance.hp;
        InGameCharLoading.instance.HealthText.text = "" + InGameCharLoading.instance.curHp + "/" + InGameCharLoading.instance.hp;
    }

    private int originDamage;
    public void IncreateDamage(int type)
    {
        if (type == 0)
        {
            originDamage = InGameCharLoading.instance.damage;
            InGameCharLoading.instance.damage += InGameCharLoading.instance.damage / 2;
        }
        else
        {
            InGameCharLoading.instance.damage = originDamage;
        }
    }
    public void ResetingUtilmate()
    {
        abilityCooldownButton.GetComponent<Image>().fillAmount = 0;
        abilityCooldownButton.GetComponent<Image>().raycastTarget = false;
        isCooldown = false;
    }

    public Image GetImage()
    {
        return imgAniOpeningMap;
    }

    internal void SetImage(Image img)
    {
        imgAniOpeningMap = img;
    }

    internal void SetFreeze(bool isFreeze)
    {
        this.isFreeze = isFreeze;
        if (isFreeze)
        {
            movePlayer = Vector2.zero;
            preMovePlayer = Vector2.zero;
            movingPressing = false;
        }
    }

    internal void SetSpeedUpGO(ParticleSystem fx)
    {
        speedUpFX = fx;
    }
}
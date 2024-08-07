using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    public GameController gameController;
    public InventoryController inventoryController;
    public Rigidbody2D rb;
    public Camera camera;
    public Animator animator;
    public GameObject grapplingHookReachPrefab;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer parrySpriteRenderer;
    private SpriteRenderer playerSpriteRenderer;
    public ParryController parryController;
    public ItemDatabase itemDatabase;
    public int framesToShow = 5;
    public Material hitMaterial;
    private Material originalMaterial;
    //public ParticleSystem hitParticleSystem;


    [Header("Player Movement")]
    public float moveSpeed = 10f;
    private float unBoostedMoveSpeed;
    private float DefaultMoveSpeed = 10f;
    private float moveSpeedBoost = 2f;
    private float boostDuration = 1f;
    private bool isUsingHook = false;
    public float distance;
    public float fallingConst = 5f;
    public float constantForceUpward = 0.5f;
    public float constantForceDownward = 1.5f;
    private float lastYPosition;
    private float wallDirection = 0f;
    public float grappleSpeed = 20f;
    public float grappleLifetime = 1f;

    [Header("Stats")]
    public int health = 100;
    public int maxHealth = 100;
    public int damage = 10;
    public int luck = 1;

    [Header("Wall Jumping System")]
    bool isTouchingWall = false;
    bool isWallSliding = false;
    private bool canMove = true;
    public float wallSlidingSpeed;
    public float wallJumpDuration;
    public float jumpAngle = 45f;
    public float wallJumpForce;
    bool wallJumping;
    private bool wantsToJump = false;
    private bool wantsToUseGrapplingHook = false;
    public bool _isOnParryBoost = false;

    public Transform attackPoint;

    public float attackRange;
    public LayerMask enemyLayers;
    [Header("Death Door")]
    public float deathDoorDuration = 5f;
    public bool inDeathDoor = false;
    private bool _killedEnemyDuringDeathDoor = false;
    private Coroutine _deathDoorCoroutine;

    private bool canAttack = true;
    private bool canParry = true;
    private float attackCooldown = 0.5f;
    private float parryCooldown = 1f;
    private AudioSource audioSource;
    public AudioClip katanaHitSound;
    public AudioClip parryHitSound;
    public ChangeScene sceneChanger;
    public float attackReturnForce = 2f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        lastYPosition = transform.position.y;
        unBoostedMoveSpeed = moveSpeed;
        originalMaterial = playerSpriteRenderer.material;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isTouchingWall && Input.GetKeyDown(KeyCode.Space))
        {
            wantsToJump = true;
        }

        if (Input.GetKeyDown(KeyCode.J) && !isUsingHook && !isTouchingWall)
        {
            wantsToUseGrapplingHook = true;
        }

        if (Input.GetKeyDown(KeyCode.G) && canAttack)
        {
            Attack();
            audioSource.PlayOneShot(katanaHitSound);
            StartCoroutine(AttackCooldown());
        }

        if (Input.GetKeyDown(KeyCode.K) && canParry)
        {
            parryController.Parry();
            audioSource.PlayOneShot(parryHitSound);
            StartCoroutine(ParryCooldown());
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            gameController.finish();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneChanger.MoveToScene(0);
        }

        if (Input.GetKeyDown(KeyCode.P) && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("ENDING LEVEL");
            gameController.EndLevel();
        }

        if (Input.GetKeyDown(KeyCode.O) & Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("GIVING RANDOM ITEM");
            int statToUpgrade = UnityEngine.Random.Range(0, itemDatabase.allPassiveItems.Count);
            inventoryController.AddPassiveItem(itemDatabase.allPassiveItems[statToUpgrade]);
        }

        if (Input.GetKeyDown(KeyCode.I) & Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("SAVING MYSELF");
            ResetDeathDoorState();
            gameController.enemySpawner.GetComponent<SimpleSpawnerController>().DestroyAllEnemies();
        }

        if (Input.GetKeyDown(KeyCode.U) & Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("INCREASING SCORE");
            gameController.IncreaseScore(100);
        }
    }

    void FixedUpdate()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        float horizontalVelocity = moveInputX * moveSpeed;
        float verticalVelocity;

        if (moveInputY < 0) // Pressing S or down
        {
            verticalVelocity = moveInputY * moveSpeed * constantForceDownward;
        }
        else if (moveInputY > 0) // Pressing W or up
        {
            verticalVelocity = moveInputY * moveSpeed * constantForceUpward;
        }
        else
        {
            verticalVelocity = -fallingConst;
        }

        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
        if (canMove)
        {
            if (isTouchingWall)
            {
                isWallSliding = true;

            }
            else
            {
                isWallSliding = false;
            }

            if (wallJumping)
            {
                float jumpAngleRadians = jumpAngle * Mathf.Deg2Rad;
                Vector2 jumpDirection = new Vector2(Mathf.Cos(jumpAngleRadians), Mathf.Sin(jumpAngleRadians));

                jumpDirection.x = Mathf.Abs(jumpDirection.x) * wallDirection;
                rb.velocity = new Vector2(jumpDirection.x * wallJumpForce, jumpDirection.y * wallJumpForce);
            }
            else if (isWallSliding)
            {
                rb.velocity = new Vector2(0f, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

                if (wantsToJump)
                {
                    Jump();
                }
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(7, 7, 1);
            }

            float currentYPosition = transform.position.y;

            if (currentYPosition < lastYPosition)
            {
                distance += lastYPosition - currentYPosition;
            }

            lastYPosition = currentYPosition;

            float verticalExtent = Camera.main.orthographicSize;
            float minY = Camera.main.transform.position.y - (verticalExtent - 5);
            float maxY = Camera.main.transform.position.y + (verticalExtent - 2);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);

            if (wantsToUseGrapplingHook)
            {
                FireGrapplingHook();
            }

        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    void Attack()
    {
        Debug.Log("Attacking");
        ShowAttack();
        attackPoint.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(ShowSpriteForFrames());

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        if (hitEnemies.Length > 0)
        {
            Debug.Log("hit enemies: " + hitEnemies.Length);
            // Calculate the average position of all hit enemies
            Vector2 averageEnemyPosition = Vector2.zero;
            foreach (Collider2D enemy in hitEnemies)
            {
                averageEnemyPosition += (Vector2)enemy.transform.position;
            }
            averageEnemyPosition /= hitEnemies.Length;

            Vector2 returnDirection = ((Vector2)transform.position - averageEnemyPosition).normalized;
            rb.AddForce(returnDirection * attackReturnForce, ForceMode2D.Impulse);
        }
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            if (enemy.tag == "Enemy")
            {
                enemy.GetComponent<SimpleEnemyController>().Die();
                StartCoroutine(boost());
                if (inDeathDoor)
                {
                    RegisterEnemyKill();
                }

            }
            if (enemy.tag == "Boss")
            {
                Debug.Log("Boss hit");
                enemy.GetComponent<BossController>().TakeDamage(damage, this.transform.forward);
                StartCoroutine(boost());
                if (inDeathDoor)
                {
                    RegisterEnemyKill();
                }
            }
        }
        attackPoint.GetChild(2).gameObject.SetActive(false);
    }
    IEnumerator boost()
    {
        moveSpeed += moveSpeedBoost;
        yield return new WaitForSeconds(boostDuration);
        moveSpeed = unBoostedMoveSpeed;
    }

    public void TriggerParryBoost()
    {
        StartCoroutine(ParryBoost());
    }

    public IEnumerator ParryBoost()
    {
        if (!_isOnParryBoost)
        {
            _isOnParryBoost = true;
            moveSpeed += moveSpeedBoost;
            Debug.Log("Parry boost started. Move speed boosted to " + moveSpeed);

            yield return new WaitForSeconds(boostDuration);

            moveSpeed = unBoostedMoveSpeed;
            _isOnParryBoost = false;
            Debug.Log("Parry boost ended. Move speed reset to " + moveSpeed);
        }
    }

    public void StartDeathDoorCountdown()
    {
        if (inDeathDoor) return;

        if (_deathDoorCoroutine != null)
            StopCoroutine(_deathDoorCoroutine);

        _deathDoorCoroutine = StartCoroutine(DeathDoorCountdown());
    }

    public void RegisterEnemyKill()
    {
        Debug.Log("sobreviviste al Death Door");
        _killedEnemyDuringDeathDoor = true;
        StopDeathDoorCountdown();
    }

    private void StopDeathDoorCountdown()
    {
        inDeathDoor = false;
        _killedEnemyDuringDeathDoor = false;

        if (_deathDoorCoroutine != null)
        {
            StopCoroutine(_deathDoorCoroutine);
            _deathDoorCoroutine = null;
        }
    }

    public bool InDeathDoor { get { return inDeathDoor; } }

    private IEnumerator DeathDoorCountdown()
    {
        Debug.Log("entre a Death Door ");
        inDeathDoor = true;
        _killedEnemyDuringDeathDoor = false;

        float elapsedTime = 0f;

        while (elapsedTime < deathDoorDuration)
        {
            if (_killedEnemyDuringDeathDoor)
            {
                // Stop the coroutine if an enemy is killed during death door
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_killedEnemyDuringDeathDoor)
        {
            // Reset the "Death Door" state
            inDeathDoor = false;
        }
        else
        {
            Debug.Log("moriste!! fin del juego");
            gameController.FinishGame();
        }
    }

    public void ResetDeathDoorState()
    {
        StopDeathDoorCountdown();
    }

    public void ShowAttack()
    {
        StartCoroutine(ShowSpriteForFrames());
    }

    public IEnumerator ShowSpriteForFrames()
    {
        spriteRenderer.enabled = true;

        yield return new WaitForSeconds(.1f);

        spriteRenderer.enabled = false;
    }

    public void ShowParry()
    {
        StartCoroutine(ShowParrySpriteForFrames());
    }

    public IEnumerator ShowParrySpriteForFrames()
    {
        parrySpriteRenderer.enabled = true;

        yield return new WaitForSeconds(.1f);

        parrySpriteRenderer.enabled = false;
    }

    public void IncreaseStats(StatBonus statBonus)
    {
        this.moveSpeed += statBonus.moveSpeedBonus;
        this.unBoostedMoveSpeed += statBonus.moveSpeedBonus;
        this.maxHealth += statBonus.maxHealthBonus;
        this.damage += statBonus.damageBonus;
        this.luck += statBonus.luckBonus;
    }


    public void DecreaseStats(StatBonus statBonus)
    {
        this.moveSpeed -= statBonus.moveSpeedBonus;
        this.unBoostedMoveSpeed -= statBonus.moveSpeedBonus;
        this.maxHealth -= statBonus.maxHealthBonus;
        this.damage -= statBonus.damageBonus;
        this.luck -= statBonus.luckBonus;
    }


    void Jump()
    {
        wallJumping = true;
        isTouchingWall = false;
        wantsToJump = false;

        Invoke("StopWallJump", wallJumpDuration);
    }

    void StopWallJump()
    {
        wallJumping = false;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = new Vector3(7, 7, 1);
        animator.SetBool("onWall", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !wallJumping)
        {
            isTouchingWall = true;
            animator.SetBool("onWall", true);

            wallDirection = collision.GetContact(0).normal.x;

            if (transform.localPosition.x < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, 7, 1);
                transform.localPosition = new Vector3(-9.48f, transform.localPosition.y, transform.localPosition.z);
            }
            else if (transform.localPosition.x > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, -7, 1);
                transform.localPosition = new Vector3(9.48f, transform.localPosition.y, transform.localPosition.z);
            }
        }
    }


    public void ResetPlayerPosition()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool CanMove()
    {
        return canMove;
    }

    public void MoveToEnemy(Vector3 enemyPosition, GameObject grapple)
    {
        StartCoroutine(MoveTowardsPosition(enemyPosition, grapple));
    }

    IEnumerator MoveTowardsPosition(Vector3 targetPosition, GameObject grapple)
    {
        isUsingHook = true;
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float journeyTime = journeyLength / grappleSpeed;

        // Initial scale of the grapple
        float initialGrappleLength = grapple.transform.localScale.x;

        while (Time.time - startTime < journeyTime)
        {
            float fracJourney = (Time.time - startTime) / journeyTime;

            // Move the player towards the target position
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, fracJourney);
            transform.position = currentPosition;

            // Calculate the new length of the grapple
            float newLength = Mathf.Lerp(initialGrappleLength, 0.6f, fracJourney);

            // Calculate the current direction from the player to the target position
            Vector3 currentDirection = (targetPosition - currentPosition).normalized;

            // Adjust the grapple's scale to simulate shrinking from one end
            grapple.transform.localScale = new Vector3(newLength, grapple.transform.localScale.y, grapple.transform.localScale.z);

            // Adjust grapple position to start from the current player position
            grapple.transform.position = currentPosition + (currentDirection * newLength / 2);

            yield return null;
        }

        // Ensure the player reaches the exact target position and grapple has shrunk completely at the end
        transform.position = targetPosition;
        grapple.transform.localScale = new Vector3(0.6f, grapple.transform.localScale.y, grapple.transform.localScale.z);
        Vector3 finalDirection = (targetPosition - startPosition).normalized;
        grapple.transform.position = startPosition + (finalDirection * 0.6f / 2);

        isUsingHook = false;
        Destroy(grapple);
    }

    void FireGrapplingHook()
    {
        wantsToUseGrapplingHook = false;

        float offsetDistance = 5.0f;
        Vector3 spawnPosition = transform.position + Vector3.down * offsetDistance;
        GameObject grapple = Instantiate(grapplingHookReachPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = grapple.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, grappleSpeed);
    }

    public bool isOnParryBoost
    {
        get { return _isOnParryBoost; }
    }
    public void PickupActiveItem(ActiveItemPickup itemPickup)
    {
        if (inventoryController != null)
        {
            inventoryController.AddActiveItem(itemPickup.itemData);
            Destroy(itemPickup.gameObject);
        }
    }

    public void ChangeMaterialOnHit()
    {
        transform.position += new Vector3(0, 180f * Time.deltaTime, 0);
        StartCoroutine(TurnWhiteOnHit());
    }

    IEnumerator TurnWhiteOnHit()
    {
        playerSpriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(0.1f);
        playerSpriteRenderer.material = originalMaterial;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator ParryCooldown()
    {
        canParry = false;
        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
    }
}


using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private bool combatEnabled;
    [SerializeField] private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField] private Transform attack1HitBoxPos;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private LayerMask whatIsDamagable;
    [SerializeField] private float stunDamageAmount = 0f;
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float projectileCooldown = 2f;
    [SerializeField] private float blockDamageMultiplier = 0.2f;

    public float ProjectileCooldown => projectileCooldown;
    public float LastProjectileTime => lastProjectileTime;

    private bool gotInput, isAttacking, isFirstAttack, isBlocking;

    private float lastInputTime = Mathf.NegativeInfinity;
    private float lastProjectileTime = Mathf.NegativeInfinity;
    private float projectileCooldownTimer = 0f;

    private AttackDetails attackDetails;

    private PlayerController PC;
    private PlayerStats PS;

    [SerializeField] private Boss_AIBrain boss_AIBrain;


    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerController>();
        PS = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckShurikenCooldown();
        CheckBlockInput();
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
                boss_AIBrain.RegisterMeleeAttack();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (combatEnabled && !isAttacking && Time.time >= lastProjectileTime + projectileCooldown)
            {
                FireProjectile();
                lastProjectileTime = Time.time;
                boss_AIBrain.RegisterRangeAttack();
            }
        }
    }

    private void CheckAttacks()
    {
        if(gotInput)
        {
            if(!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if(Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }

    }

    private void CheckBlockInput()
    {
        isBlocking = Input.GetKey(KeyCode.LeftShift) && combatEnabled && !isAttacking;
        anim.SetBool("Block", isBlocking);
    }

    private void CheckShurikenCooldown()
    {
        if (projectileCooldownTimer > 0f)
        {
            projectileCooldownTimer -= Time.deltaTime;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamagable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;

        foreach(Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            Quaternion.identity
        );

        AttackDetails projectileDetails = new AttackDetails
        {
            damageAmount = projectileDamage,
            position = transform.position,
            stunDamageAmount = stunDamageAmount
        };

        int facingDirection = PC.facingDirection;

        projectileCooldownTimer = projectileCooldown;
        lastProjectileTime = Time.time;

        projectile
            .GetComponent<Projectile>()
            .Setup(projectileDetails, facingDirection);

        Debug.Log("Cooldown Timer Set To: " + projectileCooldownTimer);
    }

    public float GetShurikenCooldown()
    {
        if (projectileCooldownTimer <= 0f)
            return 1f;

        return 1f - (projectileCooldownTimer / projectileCooldown);
    }


    private void Damage(AttackDetails attackDetails)
    {
        if (isBlocking)
        {
            anim.SetTrigger("BlockHit");

            PS.DecreaseHealth(attackDetails.damageAmount * blockDamageMultiplier);
            Debug.Log("Blocked!");
            return;
        }

        int direction;

        PS.DecreaseHealth(attackDetails.damageAmount);

        if (attackDetails.position.x < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        PC.Knockback(direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}

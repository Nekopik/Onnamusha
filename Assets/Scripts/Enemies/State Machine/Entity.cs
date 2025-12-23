using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Entity entityData;
    public Rigidbody2D rb {  get; private set; }
    public Animator animator { get; private set; }
    public GameObject aliveGameObject { get; private set; }
    public AnimationToStateMachine animationToStateMachine { get; private set; }

    public int lastDamageDirection {  get; private set; }
    public int facingDirection { get; private set; }

    private Vector2 velocityWorkspace;

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;

    protected bool isStuned;
    protected bool isDead;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;

    public virtual void Start()
    {
        facingDirection = 1;
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;

        aliveGameObject = transform.Find("Alive").gameObject;
        rb = aliveGameObject.GetComponent<Rigidbody2D>();
        animator = aliveGameObject.GetComponent<Animator>();
        animationToStateMachine = aliveGameObject.GetComponent <AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();
    }

    private void Awake()
    {
        aliveGameObject = transform.Find("Alive")?.gameObject;
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
        if(Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.linearVelocityY);
        rb.linearVelocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGameObject.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGameObject.transform.right, entityData.minAggroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGameObject.transform.right, entityData.maxAggroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGameObject.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual void ResetStunResistance()
    {
        isStuned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;
        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;
        DamageHop(entityData.damageHopSpeed);

        if(attackDetails.position.x > aliveGameObject.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else
        {
            lastDamageDirection = 1;
        }

        if (currentHealth <= 0)
        {
            isDead = true;
        }
        else if (currentStunResistance <= 0)
        {
            isStuned = true;
        }
    }

    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.linearVelocityX, velocity);
        rb.linearVelocity = velocityWorkspace;
    }

    public virtual void SetVelocityStunKnockback(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.linearVelocity = velocityWorkspace;
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGameObject.transform.Rotate(0f, 180f, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        if (entityData == null || wallCheck == null || ledgeCheck == null || playerCheck == null)
            return;

        Vector3 right = (aliveGameObject != null)
            ? aliveGameObject.transform.right
            : transform.right;

        Gizmos.DrawLine(wallCheck.position, wallCheck.position + right * entityData.wallCheckDistance);
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + Vector3.down * entityData.ledgeCheckDistance);

        Gizmos.DrawWireSphere(playerCheck.position + right * entityData.closeRangeActionDistance, 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + right * entityData.minAggroDistance, 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + right * entityData.maxAggroDistance, 0.2f);
    }

}

using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Entity entityData;
    public Rigidbody2D rb {  get; private set; }
    public Animator animator { get; private set; }
    public GameObject aliveGameObject { get; private set; }
    public AnimationToStateMachine animationToStateMachine { get; private set; }
    

    public int facingDirection { get; private set; }

    private Vector2 velocityWorkspace;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;

    public virtual void Start()
    {
        facingDirection = 1;

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

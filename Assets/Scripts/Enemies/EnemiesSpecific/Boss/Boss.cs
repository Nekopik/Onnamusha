using NUnit.Framework.Interfaces;
using UnityEngine;

public class Boss : Entity
{

    public enum BossMode
    {
        Aggressive,
        Passive
    }
    public Boss_MoveState moveState { get; private set; }
    public Boss_DeadState deadState { get; private set; }
    public Boss_PlayerDetectedState playerDetectedState { get; private set; }
    public Boss_MeleeAttackState meleeAttackState { get; private set; }
    public Boss_IdleState idleState { get; private set; }
    public Boss_RangeAttackState rangeAttackState { get; private set; }

    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_RangeAttackState rangeAttackStateData;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangeAttackPosition;
    [SerializeField] public Transform basePosition;
    [SerializeField] public Transform player;

    [SerializeField] public float meleeCooldown = 5f;
    [SerializeField] public float rangeCooldown = 10f;

    [SerializeField] private float aggressiveMeleeCooldown = 3f;
    [SerializeField] private float aggressiveRangeCooldown = 15f;

    [SerializeField] private float passiveMeleeCooldown = 10f;
    [SerializeField] private float passiveRangeCooldown = 3f;

    [SerializeField] public Boss_AI_Tracker boss_ai_tracker;
    [SerializeField] private BossFightStartTrigger bossFightStartTrigger;

    [SerializeField] private float flipCooldown = 0.5f;
    [SerializeField] public BossMode currentMode = BossMode.Passive;
    [Range(0f, 1f)] public float aiMeleePreference = 0.5f;
    private float lastAttackDecisionTime;
    [SerializeField] private float attackDecisionCooldown = 0.3f;

    private float lastMeleeAttackTime = -Mathf.Infinity;
    private float lastRangeAttackTime = -Mathf.Infinity;
    private float lastFlipTime = -Mathf.Infinity;

    public bool isFightActive = false;
    public float bossStartTime;
    public float bossEndTime;



    public override void Start()
    {
        base.Start();
        Flip();

        moveState = new Boss_MoveState(this, stateMachine, "Move", moveStateData, this);
        playerDetectedState = new Boss_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedData, this);
        deadState = new Boss_DeadState(this, stateMachine, "Dead", deadStateData, this);
        idleState = new Boss_IdleState(this, stateMachine, "Idle", idleStateData, this);
        meleeAttackState = new Boss_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        rangeAttackState = new Boss_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackPosition, rangeAttackStateData, this);

        stateMachine.Initialize(idleState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        if (stateMachine.currentState == deadState)
            return;

        base.Damage(attackDetails);

        if (isDead)
        {
            SetBossFightInactive();
            //boss_ai_tracker.LogBossFight();
            stateMachine.ChangeState(deadState);
        }
        /*
        else if (isStuned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        */
    }

    public bool CanMeleeAttack()
    {
        float cooldown = currentMode == BossMode.Aggressive
            ? aggressiveMeleeCooldown
            : passiveMeleeCooldown;

        return Time.time >= lastMeleeAttackTime + cooldown;
    }

    public bool CanRangeAttack()
    {
        float cooldown = currentMode == BossMode.Aggressive
            ? aggressiveRangeCooldown
            : passiveRangeCooldown;

        return Time.time >= lastRangeAttackTime + cooldown;
    }

    public bool CanFlip()
    {
        return Time.time >= lastFlipTime + flipCooldown;
    }

    public void SetMeleeAttackOnCooldown()
    {
        lastMeleeAttackTime = Time.time;
    }

    public void SetRangeAttackOnCooldown()
    {
        lastRangeAttackTime = Time.time;
    }

    public void SetFlipOnCooldown()
    {
        lastFlipTime = Time.time;
    }

    //TODO: add charge attack state and special attack state
    public bool ShouldChargePlayer(float distanceToPlayer, float chargeDistance)
    {
        return currentMode == BossMode.Aggressive && distanceToPlayer > chargeDistance;
    }

    public bool CanUseSpecialAttack()
    {
        return currentMode == BossMode.Passive;
    }

    public void SetBossFightActive()
    {
        isFightActive = true;
        bossStartTime = Time.time;
        Debug.Log("Boss fight activated");
    }

    public void SetBossFightInactive()
    {
        isFightActive = false;
        bossEndTime = Time.time;
        bossFightStartTrigger.SetWallInactive();
        Debug.Log("Boss fight deactivated");
    }

    public bool CanMakeAttackDecision()
    {
        return Time.time >= lastAttackDecisionTime + attackDecisionCooldown;
    }

    public void MarkAttackDecision()
    {
        lastAttackDecisionTime = Time.time;
    }


    //AI Stuff
    public void SetMode(BossMode mode)
    {
        if (currentMode == mode)
            return;

        currentMode = mode;

        Debug.Log("Boss mode switched to: " + mode);
    }


    public void SetAIPreference(float value)
    {
        aiMeleePreference = value;

        if (value > 0.6f)
            SetMode(BossMode.Aggressive);
        else if (value < 0.4f)
            SetMode(BossMode.Passive);
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
        {
            FindPlayer();
        }
    }

    private void FindPlayer()
    {
        // Method 1: Find by Tag (Recommended for performance)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Boss found the Player again via Tag.");
        }
    }
        /*
    public override void OnDrawGizmos()
    {
    base.OnDrawGizmos();

    if (meleeAttackPosition == null || meleeAttackStateData == null)
      return;

    Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
    */

        // DEBUG
    }

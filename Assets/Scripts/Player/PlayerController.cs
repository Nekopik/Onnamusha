using System;
using System.Diagnostics;
using UnityEngine;
using Input = UnityEngine.Input;

public class PlayerController : MonoBehaviour
{


    private Rigidbody2D rb;
    private Animator anim;

    private float movementInputDirection;
    private float dashTimeLeft;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;
    //private float lastImageXpos;
    //private float lastDash = -100f;
    private float turnTimer = 0.15f;
    private float turnTimerSet = 0.1f;
    private bool isFacingRight = true;
    private bool isRunning;
    private bool isGrounded;
    private bool canJump;
    private bool canMove;
    private bool canFlip;
    private bool isWallSliding;
    private bool isTouchingWall;
    private bool isDead;
    private bool isDashing;
    private bool knockback;
    private int leftJumps;
    private int facingDirection = 1;

    public Transform groundCheck;
    public Transform wallCheck;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    [SerializeField]
    private Vector2 knockbackSpeed;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float wallHopForce;
    public float wallJumpForce;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;
    public int howManyJumps = 2;

    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        leftJumps = howManyJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        //CheckDash();
        CheckKnockback();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Math.Abs(rb.linearVelocity.x) >= 0.01f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        //if (Input.GetButtonDown("Dash"))
        //{
        //    if (Time.time >= (lastDash + dashCooldown))
        //    {
        //        AttemptToDash();
        //    }

        //}

        if(Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if(!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if(!canMove)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && rb.linearVelocity.y < 0.01f) || isWallSliding)
        {
            leftJumps = howManyJumps;
        }
        if (leftJumps <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.linearVelocity = new Vector2(10 * facingDirection, 0); //test
                dashTimeLeft = Time.deltaTime;

                //if (Math.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                //{
                //    PlayerAfterimagePool.instance.GetFromPool();
                //    lastImageXpos = transform.position.x;
                //}
            }

            if (dashTimeLeft <= 0 || isTouchingWall || !isGrounded)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isDead", isDead);
    }

    private void ApplyMovement()
    {
        if (!isGrounded && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * airDragMultiplier, rb.linearVelocity.y);
        }
        else if (canMove && !knockback)
        {
            rb.linearVelocity = new Vector2(movementSpeed * movementInputDirection, rb.linearVelocity.y);
        }
        

        if (isWallSliding)
        {
            if (rb.linearVelocity.y < -wallSlideSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
            }
        }

       

    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.linearVelocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void Jump()
    {
        if (canJump && !isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            leftJumps--;
        }
        else if (isWallSliding && movementInputDirection == 0 && canJump)
        {
            isWallSliding = false;
            leftJumps--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if ((isWallSliding || isTouchingWall) && movementInputDirection != 0 && canJump)
        {
            isWallSliding = false;
            leftJumps--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        canMove = true;
        canFlip = true;
    }

    //private void AttemptToDash()
    //{
    //    isDashing = true;
    //    dashTimeLeft = dashTime;
    //    lastDash = Time.time;

    //    PlayerAfterimagePool.instance.GetFromPool();
    //    lastImageXpos = transform.position.x;
    //}

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}

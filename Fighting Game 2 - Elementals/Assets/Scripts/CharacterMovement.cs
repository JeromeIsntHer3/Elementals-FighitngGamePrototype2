using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement : MonoBehaviour
{
    [Header("General Values")]
    [SerializeField] float playerSpeed;
    [SerializeField] float accelerationSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDelay;
    [SerializeField] float dashForce;
    [SerializeField] int jumpsAllowed;
    [SerializeField] float slideMultiplier;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck1;
    [SerializeField] Transform groundCheck2;
    [SerializeField] float groundCheckRadius;

    Rigidbody2D rb;
    CharacterInput cInput;
    CharacterAnimator cAnimator;
    Vector2 movement;
    int jumps;
    bool canMove = true;
    bool jumping;
    bool recovering;
    bool sliding;
    float recoveryTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cInput = GetComponent<CharacterInput>();
        cAnimator = GetComponent<CharacterAnimator>();
        jumps = jumpsAllowed;
    }

    void OnEnable()
    {
        cInput.OnMovement += OnMovement;
        cInput.OnJump += OnJump;
        cInput.OnAttack1 += OnAttack1;
        cInput.OnAttack2 += OnAttack2;
        cInput.OnAttack3 += OnAttack3;
        cInput.OnUltimate += OnUltimate;
        cInput.OnRoll += OnRoll;
        cInput.OnOption += OnOptionPerformed;
        cInput.OnOptionCanceled += OnOptionCanceled;
    }

    void OnDisable()
    {
        cInput.OnMovement -= OnMovement;
        cInput.OnJump -= OnJump;
        cInput.OnAttack1 -= OnAttack1;
        cInput.OnAttack2 -= OnAttack2;
        cInput.OnAttack3 -= OnAttack3;
        cInput.OnUltimate -= OnUltimate;
        cInput.OnRoll -= OnRoll;
        cInput.OnOption -= OnOptionPerformed;
        cInput.OnOptionCanceled -= OnOptionCanceled;
    }

    void OnMovement(object sender, Vector2 args)
    {
        //movement = args;
    }

    void OnJump(object sender, EventArgs args)
    {
        if (recovering || jumps <= 0) return;
        if (IsGrounded())
        {
            canMove = false;
            jumping = true;
            StartCoroutine(JumpDelayCR());
            jumps -= 1;
        }
        else
        {
            if(!jumping)
            {
                jumps -= 1;
                jumping = true;
            }
            Jump();
            jumps -= 1;
        }
    }

    void OnAttack1(object sender, EventArgs args)
    {
        if (!Recovered()) return;
        SetRecoveryTime(.667f);
    }

    void OnAttack2(object sender, EventArgs args)
    {
        if (!Recovered()) return;
        SetRecoveryTime(1f);
    }

    void OnAttack3(object sender, EventArgs args)
    {
        if (!Recovered()) return;
        SetRecoveryTime(.8f);
    }

    void OnUltimate(object sender, EventArgs args)
    {
        if (!Recovered()) return;
        SetRecoveryTime(1.133f);
    }

    void OnRoll(object sender, EventArgs args)
    {
        if(!Recovered()) return;
        if (jumping) return;
        SetRecoveryTime(.533f);
        Roll();
    }

    void OnOptionPerformed(object sender, EventArgs args)
    {
        if (Mathf.Abs(HorizontalVelocity()) < 4) return;
        if (!Recovered()) return;
        if (jumping || sliding) return;
        sliding = true;
        Slide();
    }

    void OnOptionCanceled(object sender, EventArgs args)
    {
        if (!sliding) return;
        SetRecoveryTime(.267f);
        rb.velocity = new Vector2(rb.velocity.x / 2, rb.velocity.y);
        sliding = false;
    }

    void Update()
    {
        Debug.Log(Recovered());
        movement = cInput.CurrentMovementInput();
    }

    void FixedUpdate()
    {
        if(!canMove) return;
        if (sliding)
        {
            if (Mathf.Abs(HorizontalVelocity()) < 2) cInput.OnOptionCanceled?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (!Recovered()) return;
            
        float targetSpeed = playerSpeed * movement.x;
        float speedDiff = targetSpeed - rb.velocity.x;
        float movementRate = speedDiff * accelerationSpeed;
        rb.AddForce(Vector2.right * movementRate, ForceMode2D.Force);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //if (movement.x == 0) return;
        //Vector2 dir = movement.x < 0 ? Vector2.left : Vector2.right;
        rb.AddForce(movement * jumpForce/2, ForceMode2D.Impulse);
    }

    void Roll()
    {
        Vector2 dir = cAnimator.IsFacingLeft() ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
    }

    void Slide()
    {
        Vector2 dir = cAnimator.IsFacingLeft() ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        rb.AddForce(Mathf.Abs(rb.velocity.x) * slideMultiplier * dir, ForceMode2D.Impulse);
    }

    void SetRecoveryTime(float t)
    {
        recoveryTime = Time.time + t;
    }

    bool Recovered()
    {
        return Time.time > recoveryTime;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapArea(groundCheck1.position, groundCheck2.position, groundLayer);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGrounded())
        {
            jumping = false;
            jumps = jumpsAllowed;
            cInput.OnLand?.Invoke(this, EventArgs.Empty);
        }
    }

    public float HorizontalVelocity()
    {
        return rb.velocity.x;
    }

    public float VerticalVelocity()
    {
        return rb.velocity.y;
    }

    void OnDrawGizmos()
    {
        Vector3 point1 = groundCheck1.position;
        Vector3 point2 = new Vector3(groundCheck2.position.x, groundCheck1.position.y);
        Vector3 point3 = groundCheck2.position;
        Vector3 point4 = new Vector3(groundCheck1.position.x, groundCheck2.position.y);

        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point2, point3);
        Gizmos.DrawLine(point3, point4);
        Gizmos.DrawLine(point4, point1);
    }

    #region Coroutines

    IEnumerator JumpDelayCR()
    {
        yield return new WaitForSeconds(jumpDelay);
        canMove = true;
        Jump();
    }

    #endregion
}
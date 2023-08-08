using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class CharacterMovement : MonoBehaviour
{
    [Header("General Values")]
    [SerializeField] float playerSpeed;
    [SerializeField] float accelerationSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDelay;
    [SerializeField] float dashForce;
    [SerializeField] int jumpsAllowed;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;

    Rigidbody2D rb;
    CharacterInput cInput;
    CharacterAnimator cAnimator;
    Vector2 movement;
    int jumps;
    bool canMove = true;
    bool jumping;
    bool recovering;

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
            Jump();
            jumps -= 1;
        }
    }

    void OnAttack1(object sender, EventArgs args)
    {
        if (recovering) return;
        recovering = true;
        StartCoroutine(RecoveringCR(.85f));
    }

    void OnAttack2(object sender, EventArgs args)
    {
        if (recovering) return;
        recovering = true;
        StartCoroutine(RecoveringCR(1.26f));
    }

    void OnAttack3(object sender, EventArgs args)
    {
        if (recovering) return;
        recovering = true;
        StartCoroutine(RecoveringCR(1.1f));
    }

    void OnUltimate(object sender, EventArgs args)
    {
        if (recovering) return;
        recovering = true;
        StartCoroutine(RecoveringCR(1.417f));
    }

    void OnRoll(object sender, EventArgs args)
    {
        if (jumping || recovering) return;
        recovering = true;
        StartCoroutine(RecoveringCR(0.68f));
        Roll();
    }

    IEnumerator RecoveringCR(float t)
    {
        yield return new WaitForSeconds(t);
        recovering = false;
    }

    void Update()
    {
        movement = cInput.CurrentMovementInput();
    }

    void FixedUpdate()
    {
        if (!canMove || recovering) return;

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

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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

    #region Coroutines

    IEnumerator JumpDelayCR()
    {
        yield return new WaitForSeconds(jumpDelay);
        canMove = true;
        Jump();
    }

    #endregion
}
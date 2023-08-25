using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BaseCharacterMovement : MonoBehaviour
{
    [SerializeField] protected CharacterMovementSO data;

    [Header("Ground Check")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck1;
    [SerializeField] protected Transform groundCheck2;

    protected readonly Dictionary<AnimationType, float> animationTimes = new();

    protected BaseCharacter character;
    protected Rigidbody2D rb;
    protected CharacterInput cInput;
    protected Vector2 movement;
    protected int jumps;
    [SerializeField] protected bool canMove = true;
    protected bool jumping;
    protected bool option;
    protected bool isFacingLeft;

    protected delegate void Option();
    protected delegate bool OptionCondition();
    protected Option OptionPerformedDelegate;
    protected Option OptionCanceledDelegate;
    protected Option OptionUpdate;
    protected OptionCondition OptionPerformCond;
    protected OptionCondition OptionCancelCond;

    public virtual void Awake()
    {
        character = GetComponent<BaseCharacter>();
        rb = GetComponent<Rigidbody2D>();
        cInput = GetComponent<CharacterInput>();
        jumps = data.JumpsAllowed;
    }

    public virtual void OnEnable()
    {
        character.OnJump += OnJump;
        character.OnRoll += OnRoll;
        character.OnChangeFaceDirection += OnChangeFaceDirection;
        character.OnHit += OnHit;
        character.OnBlockHit += OnBlockHit;

        if (character.AnimationData.optionIsHeld)
        {
            character.OnOption += OnOptionPerformed;
            character.OnOptionCanceled += OnOptionCanceled;
        }
        else if (character.AnimationData.optionIsTriggered)
        {
            character.OnOption += OnOptionPerformed;
        }
    }

    public virtual void OnDisable()
    {
        character.OnJump -= OnJump;
        character.OnRoll -= OnRoll;
        character.OnChangeFaceDirection -= OnChangeFaceDirection;
        character.OnHit -= OnHit;
        character.OnBlockHit -= OnBlockHit;

        if (character.AnimationData.optionIsHeld)
        {
            character.OnOption -= OnOptionPerformed;
            character.OnOptionCanceled -= OnOptionCanceled;
        }
        else if (character.AnimationData.optionIsTriggered)
        {
            character.OnOption -= OnOptionPerformed;
        }
    }

    void OnJump(object sender, EventArgs args)
    {
        if (jumps <= 0) return;
        if (IsGrounded() && !jumping)
        {
            Invoke(nameof(JumpDelay), character.GetAnimationDuration(AnimationType.JumpStart));
            canMove = false;
            jumping = true;
            jumps -= 1;
        }
        else
        {
            if (!jumping)
            {
                jumps -= 1;
                jumping = true;
            }
            Jump();
            jumps -= 1;
        }
    }

    void JumpDelay()
    {
        canMove = true;
        Jump();
    }

    void OnRoll(object sender, EventArgs args)
    {
        if (jumping) return;
        Roll();
    }

    void OnOptionPerformed(object sender, EventArgs args)
    {
        if (OptionPerformCond == null || !OptionPerformCond()) return;
        if (jumping || option) return;
        OptionPerformedDelegate?.Invoke();
        if (character.AnimationData.optionIsTriggered) return;
        option = true;
    }

    void OnOptionCanceled(object sender, EventArgs args)
    {
        if (!option) return;
        OptionCanceledDelegate?.Invoke();
        option = false;
    }

    void OnChangeFaceDirection(object sender, bool e)
    {
        isFacingLeft = e;
    }

    void OnHit(object sender, DamageData e)
    {
        Knockback(e.KnockbackDirection, e.HorizontalKnockback, e.VerticalKnockback);
    }

    void OnBlockHit(object sender, DamageData e)
    {
        Knockback(e.KnockbackDirection, e.HorizontalKnockback, e.VerticalKnockback);
    }

    void Update()
    {
        movement = cInput.CurrentMovementInput();
        if (option)
        {
            OptionUpdate?.Invoke();
            if (OptionCancelCond == null || !OptionCancelCond()) return;
            character.OnOptionCanceled?.Invoke(this, EventArgs.Empty);
        }
    }

    void FixedUpdate()
    {
        if (!character.Recovered() || !canMove) return;
            
        float targetSpeed = (jumping ? data.PlayerSpeed * 4/5f : data.PlayerSpeed) * movement.x;
        float speedDiff = targetSpeed - rb.velocity.x;
        float movementRate = speedDiff * data.AccelerationSpeed;
        rb.AddForce(Vector2.right * movementRate, ForceMode2D.Force);
    }

    void Jump()
    {
        movement.x = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * data.JumpForce, ForceMode2D.Impulse);
        rb.AddForce(movement * data.JumpForce/2, ForceMode2D.Impulse);
        character.SetGroundedState(false);
    }

    void Roll()
    {
        Vector2 dir = isFacingLeft ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(dir * data.DashForce, ForceMode2D.Impulse);
    }

    void Knockback(Vector2 direction, float hForce, float vForce)
    {
        rb.AddForce(direction.normalized * new Vector2(hForce, vForce), ForceMode2D.Impulse);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapArea(groundCheck1.position, groundCheck2.position, groundLayer);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGrounded() && jumping)
        {
            character.SetGroundedState(true);
            jumping = false;
            jumps = data.JumpsAllowed;
            character.OnLand?.Invoke(this, EventArgs.Empty);
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck1 == null || groundCheck2 == null) return;

        Vector3 point1 = groundCheck1.position;
        Vector3 point2 = new (groundCheck2.position.x, groundCheck1.position.y);
        Vector3 point3 = groundCheck2.position;
        Vector3 point4 = new (groundCheck1.position.x, groundCheck2.position.y);

        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point2, point3);
        Gizmos.DrawLine(point3, point4);
        Gizmos.DrawLine(point4, point1);
    }
}
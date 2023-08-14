using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class BaseCharacterMovement : BaseCharacter
{
    [SerializeField] protected CharacterMovementSO data;

    [Header("Ground Check")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck1;
    [SerializeField] protected Transform groundCheck2;

    protected readonly Dictionary<AnimationType, float> animationTimes = new();

    protected Rigidbody2D rb;
    protected CharacterInput cInput;
    protected Vector2 movement;
    protected int jumps;
    protected bool canMove = true;
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

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        cInput = GetComponent<CharacterInput>();
        jumps = data.JumpsAllowed;
    }

    public virtual void OnEnable()
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
        cInput.OnChangeFaceDirection += OnChangeFaceDirection;
        cInput.OnHit += OnHit;
        cInput.OnDefend += OnDefend;
    }

    public virtual void OnDisable()
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
        cInput.OnChangeFaceDirection -= OnChangeFaceDirection;
        cInput.OnHit -= OnHit;
        cInput.OnDefend -= OnDefend;
    }

    void OnMovement(object sender, Vector2 args)
    {
        //movement = args;
    }

    void OnJump(object sender, EventArgs args)
    {
        if (!Recovered()) return;
        if (jumps <= 0) return;
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
        if (!Recovered() || jumping) return;
        SetRecoveryDuration(GetDuration(AnimationType.Attack1));
    }

    void OnAttack2(object sender, EventArgs args)
    {
        if (!Recovered() || jumping) return;
        SetRecoveryDuration(GetDuration(AnimationType.Attack2));
    }

    void OnAttack3(object sender, EventArgs args)
    {
        if (!Recovered() || jumping) return;
        SetRecoveryDuration(GetDuration(AnimationType.Attack3));
    }

    void OnUltimate(object sender, EventArgs args)
    {
        if (!Recovered() || jumping) return;
        SetRecoveryDuration(GetDuration(AnimationType.Ultimate));
    }

    void OnRoll(object sender, EventArgs args)
    {
        if(!Recovered()) return;
        if (jumping) return;
        SetRecoveryDuration(GetDuration(AnimationType.Roll));
        Roll();
    }

    void OnOptionPerformed(object sender, EventArgs args)
    {
        if (OptionPerformCond()) return;
        if (!Recovered()) return;
        if (jumping || option) return;
        OptionPerformedDelegate?.Invoke();
        option = true;
    }

    void OnOptionCanceled(object sender, EventArgs args)
    {
        if (!option) return;
        SetRecoveryDuration(GetDuration(AnimationType.CharacterOptionEnd));
        OptionCanceledDelegate?.Invoke();
        option = false;
    }

    void OnChangeFaceDirection(object sender, bool e)
    {
        isFacingLeft = e;
    }

    void OnHit(object sender, DamageData e)
    {
        SetRecoveryDuration(GetDuration(AnimationType.Damaged));
        Knockback(e.Direction, e.HorizontalKnockback, e.VerticalKnockback);
    }

    void OnDefend(object sender, DamageData e)
    {
        Knockback(e.Direction, e.HorizontalKnockback, e.VerticalKnockback);
    }

    void Update()
    {
        movement = cInput.CurrentMovementInput();
        if (option) OptionUpdate?.Invoke();
    }

    void FixedUpdate()
    {
        if(!canMove) return;
        if (option)
        {
            if (OptionCancelCond()) cInput.OnOptionCanceled?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (!Recovered()) return;
            
        float targetSpeed = data.PlayerSpeed * movement.x;
        float speedDiff = targetSpeed - rb.velocity.x;
        float movementRate = speedDiff * data.AccelerationSpeed;
        rb.AddForce(Vector2.right * movementRate, ForceMode2D.Force);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * data.JumpForce, ForceMode2D.Impulse);
        rb.AddForce(movement * data.JumpForce/2, ForceMode2D.Impulse);
    }

    void Roll()
    {
        Vector2 dir = isFacingLeft ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(dir * data.DashForce, ForceMode2D.Impulse);
    }

    void Knockback(Vector2 direction, float hForce, float vForce)
    {
        Debug.Log(direction);
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
            jumping = false;
            jumps = data.JumpsAllowed;
            cInput.OnLand?.Invoke(this, EventArgs.Empty);
        }
    }

    void OnDrawGizmos()
    {
        Vector3 point1 = groundCheck1.position;
        Vector3 point2 = new (groundCheck2.position.x, groundCheck1.position.y);
        Vector3 point3 = groundCheck2.position;
        Vector3 point4 = new (groundCheck1.position.x, groundCheck2.position.y);

        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point2, point3);
        Gizmos.DrawLine(point3, point4);
        Gizmos.DrawLine(point4, point1);
    }

    #region Coroutines

    IEnumerator JumpDelayCR()
    {
        yield return new WaitForSeconds(data.JumpDelay);
        canMove = true;
        Jump();
    }

    #endregion
}
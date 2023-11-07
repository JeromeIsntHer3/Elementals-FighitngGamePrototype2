using System;
using UnityEngine;

public class Character : MonoBehaviour, ICharacter, IKnockable
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform rotateObject;
    [SerializeField] protected Transform groundCheck1;
    [SerializeField] protected Transform groundCheck2;

    [field: SerializeField] public CharacterMovementSO MovementData { get; set; }
    [field: SerializeField] public Rigidbody2D ObjectRigidbody {  get; set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    [field: SerializeField] public bool HasOptionAnimation { get; set; }
    [field: SerializeField] public float DelayBetweenJumps { get; set; }

    int jumps = 1;

    PlayerInputHandler input;
    Vector2 movement;
    bool blockPressed, optionPressed, jumpPressed, attackPressed, rollPressed, facingLeft;
    int currentAttackIndex = -1;

    #region Getter And Setters

    public Vector2 Movement { get { return movement; } }
    public bool IsBlockPressed { get { return blockPressed; } }
    public bool IsOptionPressed { get { return optionPressed; } }
    public bool IsRollPressed { get { return rollPressed; } }
    public bool IsJumpPressed { get { return jumpPressed; } }
    public bool IsAttackPressed {  get { return attackPressed; } }
    public bool IsFacingLeft {  get { return facingLeft; } }
    public int CurrentAttack { get {  return currentAttackIndex; } set { currentAttackIndex = value; } }

    #endregion

    public EventHandler<int> OnTriggerAttack;


    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        jumps = MovementData.JumpsAllowed;
    }

    void OnEnable()
    {
        input.OnMovementPressed += OnMovement;
        input.OnBlock += OnBlock;
        input.OnOption += OnOption;
        input.OnJump += OnJump;
        input.OnAttackPressed += OnAttack;
        input.OnRoll += OnRoll;
    }

    void OnDisable()
    {
        input.OnMovementPressed -= OnMovement;
        input.OnBlock -= OnBlock;
        input.OnOption -= OnOption;
        input.OnJump -= OnJump;
        input.OnAttackPressed -= OnAttack;
        input.OnRoll -= OnRoll;
    }

    void OnMovement(object sender, Vector2 direction)
    {
        movement = direction;

        if(direction.x < 0)
        {
            facingLeft = true;
            rotateObject.localScale = new Vector3(-1, 1, 1);
        }else if(direction.x > 0)
        {
            facingLeft = false;
            rotateObject.localScale = new Vector3(1, 1, 1);
        }
    }

    void OnBlock(object sender, bool state)
    {
        blockPressed = state;
    }

    void OnOption(object sender, bool state)
    {
        optionPressed = state;
    }

    void OnJump(object sender, bool state)
    {
        jumpPressed = state;
    }

    void OnAttack(object sender, int index)
    {
        currentAttackIndex = index;
        attackPressed = true;
    }

    void OnRoll(object sender, bool state)
    {
        rollPressed = state;
    }

    void Update()
    {
        if(rollPressed) rollPressed = false;
        if (attackPressed) attackPressed = false;
    }

    public bool IsTouchingGround()
    {
        return Physics2D.OverlapArea(groundCheck1.position, groundCheck2.position, groundLayer);
    }

    public bool CanJump()
    {
        return jumps > 0;
    }

    public void ResetJumps()
    {
        jumps = MovementData.JumpsAllowed;
    }

    public void JumpUsed()
    {
        jumps--;
    }

    public void TriggerAttack(int index)
    {
        OnTriggerAttack?.Invoke(this, index);
    }

    public void Damage(float dmgAmount)
    {
        
    }

    public void Knockback(Vector2 direction, float force)
    {
        ObjectRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
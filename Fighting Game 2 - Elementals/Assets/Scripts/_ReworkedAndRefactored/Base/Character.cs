using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour, ICharacter
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck1;
    [SerializeField] protected Transform groundCheck2;

    [field: SerializeField] public CharacterMovementSO MovementData { get; set; }
    [field: SerializeField] public Rigidbody2D Rb {  get; set; }
    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    public int Jumps = 1;

    CharacterStateMachine stateMachine;
    PlayerInputHandler input;
    Vector2 movement;
    bool blockPressed, optionPressed, jumpPressed;
    int currentAttackIndex = -1;


    #region Getter And Setters

    public Vector2 Movement { get { return movement; } }
    public bool IsBlockPressed { get { return blockPressed; } }
    public bool IsOptionPressed { get { return optionPressed; } }
    public bool IsJumpPressed { get { return jumpPressed; } }
    public int CurrentAttack { get {  return currentAttackIndex; } }

    #endregion


    void Awake()
    {
        stateMachine = GetComponent<CharacterStateMachine>();
        input = GetComponent<PlayerInputHandler>();
    }

    void OnEnable()
    {
        input.OnMovementPressed += OnMovement;
        input.OnBlock += OnBlock;
        input.OnOption += OnOption;
        input.OnJump += OnJump;
        input.OnAttackPressed += OnAttack;
    }

    void OnDisable()
    {
        input.OnMovementPressed -= OnMovement;
        input.OnBlock -= OnBlock;
        input.OnOption -= OnOption;
        input.OnJump -= OnJump;
        input.OnAttackPressed -= OnAttack;
    }

    void OnMovement(object sender, Vector2 direction)
    {
        movement = direction;
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
    }

    void Update()
    {
        
    }

    public bool IsTouchingGround()
    {
        return Physics2D.OverlapArea(groundCheck1.position, groundCheck2.position, groundLayer);
    }

    public void Damage(float dmgAmount)
    {
        
    }
}
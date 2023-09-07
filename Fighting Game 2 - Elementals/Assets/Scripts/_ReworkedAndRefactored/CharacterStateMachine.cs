using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public TextMeshProUGUI currentStateText;

    Character character;
    CharacterAnimator animator;
    CharacterState currentState;
    CharacterStateFactory stateFactory;

    public Character P_Character { get { return character; } }
    public CharacterAnimator P_Animator { get { return animator; } }
    public CharacterState P_CurrentState {  get {  return currentState; } set { currentState = value; } }


    void Awake()
    {
        character = GetComponent<Character>();
        animator = GetComponent<CharacterAnimator>();
    }

    void Start()
    {
        InitializeStateMachine();
    }

    void Update()
    {
        currentState?.FrameUpdate();
        if(currentState != null) animator.SetAnimation(currentState.UpdateAnimation());
    }

    void FixedUpdate()
    {
        currentState?.PhysicsUpdate();    
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState?.OnCollisionEnter2D(collision);
    }

    public void InitializeStateMachine()
    {
        stateFactory = new(this);
        currentState = stateFactory.Grounded();
        currentState.EnterState();
    }
}
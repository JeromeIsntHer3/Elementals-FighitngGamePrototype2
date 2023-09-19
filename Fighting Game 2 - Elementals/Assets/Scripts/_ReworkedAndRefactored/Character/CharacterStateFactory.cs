public class CharacterStateFactory
{
    CharacterStateMachine _context;

    public CharacterStateFactory(CharacterStateMachine currentContext)
    {
        _context = currentContext;
    }

    public CharacterAttackingState Attacking() { return new CharacterAttackingState(_context, this); }
    public CharacterGroundedState Grounded() { return new CharacterGroundedState(_context, this); }
    public CharacterIdleState Idle() {  return new CharacterIdleState(_context, this); }
    public  CharacterRunningState Running() {  return new CharacterRunningState(_context, this); }
    public CharacterJumpingState Jumping() { return new CharacterJumpingState(_context, this); }
    public CharacterStunnedState Stunned() { return new CharacterStunnedState(_context, this); }
    public CharacterBlockState Blocking() { return new CharacterBlockState(_context, this); }
    public CharacterActionState Action() {  return new CharacterActionState(_context, this); }
}
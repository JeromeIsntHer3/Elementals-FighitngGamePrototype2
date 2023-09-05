
public class AnimationCondition
{
    bool condition;
    AnimationType animationType;

    public bool Condition { get { return condition; } }
    public AnimationType AnimType { get { return animationType; } }

    public void SetAnimationType(AnimationType t)
    {
        animationType = t;
    }
}
public class CMAnimator : BaseCharacterAnimator
{
    void Start()
    {
        OptionPerformCondition = COndition;
    }

    bool COndition()
    {
        return true;
    }
}
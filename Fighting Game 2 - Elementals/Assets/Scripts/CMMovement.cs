public class CMMovement : BaseCharacterMovement
{
    void Start()
    {
        OptionPerformCond = SomeBool;
    }

    bool SomeBool()
    {
        return false;
    }
}
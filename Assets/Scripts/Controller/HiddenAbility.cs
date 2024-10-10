public class HiddenAbility : PlayerAbility
{
    public HiddenAbility(float coolTime)
        :base(coolTime)
    {}
    public override void ActiveAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void CoolingDownAbility(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {
        throw new System.NotImplementedException();
    }
}
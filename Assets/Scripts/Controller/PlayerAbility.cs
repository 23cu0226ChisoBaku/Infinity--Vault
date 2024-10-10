using System;

public abstract class PlayerAbility:IDisposable
{
    public PlayerAbility(float coolTime)
    {
        _coolTime = coolTime;
    }
    ~PlayerAbility()
    {
        Dispose(false);
    }
    private float _coolTime;
    private bool _isAbilityExecuting;
    private bool _isDisposed;
    public event Action OnActiveAbility;
    public float CoolTime
    {
        get
        {
            return _coolTime;
        }
    }
    public abstract void ActiveAbility();
    public abstract void CoolingDownAbility(float deltaTime);
    public abstract void Dispose();
    protected abstract void Dispose(bool disposing);
}
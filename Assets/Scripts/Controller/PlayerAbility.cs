using System;

public abstract class PlayerAbility
{
    private float _coolTime;
    private bool _isAbilityExecuting;
    public event Action OnActiveAbility;
    public abstract void ActiveAbility();
    public abstract void StartCoolingDownAbility();
}
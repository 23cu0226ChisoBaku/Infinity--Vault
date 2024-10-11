using System;

public interface IAbility
{
    public string AbilityName {get;}
    public float CoolTime {get;}
    public float LeftCoolTime {get;}
    public event Action OnActiveAbility;
    public event Action OnFinishAbility;
    public event Action<float> OnCooldown;
    public event Action OnFinishCooldown;

    public void ActiveAbility();
    public void FinishAbility();
    public void CooldownAbility(float deltaTime);
    
}


public class PlayerAbility : IAbility
{
    public PlayerAbility(string name, float coolTime)
    {
        _abilityName = name;
        _coolTime = coolTime;
        _coolTimeCnt = 0f;
        _isAbilityExecuting = false;
        _isCoolingdown = false;
    }
    private string _abilityName;
    private float _coolTime;
    private float _coolTimeCnt;
    private bool _isAbilityExecuting;
    private bool _isCoolingdown;
    public string AbilityName => _abilityName;
    public event Action OnActiveAbility;
    public event Action OnFinishAbility;
    public event Action<float> OnCooldown;
    public event Action OnFinishCooldown;
    public float CoolTime
    {
        get
        {
            return _coolTime;
        }
    }

    public float LeftCoolTime
    {
        get
        {
            return _coolTimeCnt;
        }
    }
    public void ActiveAbility()
    {
        if (_isAbilityExecuting || _isCoolingdown)
        {
            return;
        }
        else
        {
            OnActiveAbility?.Invoke();
            _isAbilityExecuting = true;
        }
    }
    
    public void FinishAbility()
    {
        if (!_isAbilityExecuting)
        {
            return;
        }
        else
        {
            _isAbilityExecuting = false;
            _isCoolingdown = true;
            _coolTimeCnt = _coolTime;
            OnFinishAbility?.Invoke();
        }
    }

    public void CooldownAbility(float deltaTime)
    {
        if (!_isCoolingdown)
        {
            return;
        }

        _coolTimeCnt -= deltaTime;

        if (_coolTimeCnt <= 0f)
        {
            _coolTimeCnt = 0f;
            _isCoolingdown = false;
            OnFinishCooldown?.Invoke();
        }
        else
        {
            if (_coolTime <= 0f)
            {
                OnCooldown?.Invoke(0f);
            }
            else
            {
                float cooldownRate = _coolTimeCnt / _coolTime;
                OnCooldown?.Invoke(cooldownRate);
            }
        }
    }
}
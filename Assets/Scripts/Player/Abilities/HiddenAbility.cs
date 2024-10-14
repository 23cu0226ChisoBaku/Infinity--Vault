using System;

public sealed class HiddenAbility : IAbility
{
    public string AbilityName => "Hidden";
    public float CoolTime => _coolTime;
    public float LeftCoolTime => _coolTimeCnt;

    public event Action OnActiveAbility;
    public event Action OnFinishAbility;
    public event Action<float> OnCooldown;
    public event Action OnFinishCooldown;
    private bool _isActivatingAbility;
    private bool _isCoolingDown;
    private float _coolTime;
    private float _coolTimeCnt;

    public HiddenAbility(float coolTime)
    {
        _coolTime = coolTime;
        _coolTimeCnt = 0f;

        _isActivatingAbility = false;
        _isCoolingDown = false;

    }

    void IAbility.ActiveAbility()
    {
        if (!_isActivatingAbility && !_isCoolingDown)
        {
            _isActivatingAbility = true;
            OnActiveAbility?.Invoke();
        }
    }
    void IAbility.FinishAbility()
    {
        if (_isActivatingAbility)
        {
            _isActivatingAbility = false;
            _isCoolingDown = true;

            OnFinishAbility?.Invoke();

            _coolTimeCnt = _coolTime;
        }
    }

    void IAbility.CooldownAbility(float deltaTime)
    {
        if (_isCoolingDown)
        {
            OnCooldown?.Invoke(_coolTimeCnt);
            _coolTimeCnt -= deltaTime;

            if(_coolTimeCnt <= 0f)
            {
                _coolTimeCnt = 0f;
                _isCoolingDown = false;
                OnFinishCooldown?.Invoke();
            }
        }
    }

    public bool IsCoolingDown()
    {
        return _isCoolingDown;
    }
}
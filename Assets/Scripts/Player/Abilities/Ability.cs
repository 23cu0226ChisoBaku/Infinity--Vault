using System;
using MEvent;

/// <summary>
/// アビリティ
/// </summary>
public interface IAbility
{
  /// <summary>
  /// 名前
  /// </summary>
  public string AbilityName {get;}
  /// <summary>
  /// クールタイム
  /// </summary>
  public float CoolTime {get;}
  /// <summary>
  /// 残るクールタイム
  /// </summary>
  public float LeftCoolTime {get;}
  /// <summary>
  /// 発動するとき呼び出されるイベント
  /// </summary>
  public event Action OnActiveAbility;
  /// <summary>
  /// 発動終了するとき呼び出されるイベント
  /// </summary>
  public event Action OnFinishAbility;
  /// <summary>
  /// クールダウンしているとき呼び出されるイベント
  /// </summary>
  public event Action<float> OnCooldown;
  /// <summary>
  /// クールダウンが終わったとき呼び出されるイベント
  /// </summary>
  public event Action OnFinishCooldown;
  /// <summary>
  /// アビリティ実行
  /// </summary>
  public void ActiveAbility();
  /// <summary>
  /// アビリティ終了
  /// </summary>
  public void FinishAbility();
  /// <summary>
  /// アビリティをクールダウン
  /// </summary>
  /// <param name="deltaTime">デルタタイム</param>
  public void CooldownAbility(float deltaTime);
  /// <summary>
  /// クールダウンしているか
  /// </summary>
  /// <returns>true:している、false:していない</returns>
  public bool IsCoolingDown();
    
}
/// <summary>
/// プレイヤーアビリティクラス
/// TODO 抽象クラスにしたい
/// </summary>
public class Ability : IAbility
{
// プライベートフィールド
#region Private Field
  private string _abilityName;
  private float _coolTime;
  private float _coolTimeCnt;
  private bool _isAbilityExecuting;
  private bool _isCoolingdown;
  private DisposableEvent _onActiveAbility;
  private DisposableEvent _onFinishAbility;
  private DisposableEvent _onCooldown;
  private DisposableEvent _onFinishCooldown;
#endregion Private Field
// End of プライベートフィールド
  public Ability(string name, float coolTime)
  {
    _abilityName = name;
    _coolTime = coolTime;
    _coolTimeCnt = 0f;
    _isAbilityExecuting = false;
    _isCoolingdown = false;

    // アビリティイベント初期化
    {
      _onActiveAbility = new DisposableEvent();
      _onFinishAbility = new DisposableEvent();
      _onCooldown = new DisposableEvent();
      _onFinishCooldown = new DisposableEvent();
    }
  }
// インターフェース
#region Interface
  // IAbility実装部分
  #region IAbility interface
    string IAbility.AbilityName => _abilityName;
    bool IAbility.IsCoolingDown() => _isCoolingdown;

    event Action IAbility.OnActiveAbility
    {
      add
      {
        _onActiveAbility.Subscribe(value);
      }
      remove
      {
        _onActiveAbility.Unsubscribe(value);
      }
    }
    event Action IAbility.OnFinishAbility
    {
      add
      {
        _onFinishAbility.Subscribe(value);
      }
      remove
      {
        _onFinishAbility.Unsubscribe(value);
      }
    }
    public event Action<float> OnCooldown;

    event Action IAbility.OnFinishCooldown
    {
      add
      {
        _onFinishCooldown.Subscribe(value);
      }
      remove
      {
        _onFinishCooldown.Unsubscribe(value);
      }
    }
    float IAbility.CoolTime => _coolTime; 
    float IAbility.LeftCoolTime => _coolTimeCnt;

    void IAbility.ActiveAbility()
    {
      if (_isAbilityExecuting || _isCoolingdown)
      {
        return;
      }
      else
      {
        _onActiveAbility?.Invoke();
        _isAbilityExecuting = true;
      }
    }
    
    void IAbility.FinishAbility()
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
        _onFinishAbility?.Invoke();
      }
    }

    void IAbility.CooldownAbility(float deltaTime)
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
        _onFinishCooldown?.Invoke();
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
  #endregion IAbility
  // End of IAbility実装部分
#endregion Interface
// End of インターフェース
}
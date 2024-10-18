using System;
using MMVCFramework.Model;

/// <summary>
/// プレイヤーモデル
/// </summary>
public sealed class PlayerModel : Model<PlayerData> , IPlayerModel, IPlayerUIMessage
{
  public event Action<float,float> OnChangeMoveSpeedValue;
  public event Action<float,float> OnChangeClimbSpeedValue;
  public event Action<int,int> OnChangeWealthValue;
  public PlayerModel()
  {
    _model = new PlayerData();
  }
  public PlayerModel(float moveSpeed, float climbSpeed)
      : this()
  {
    _model.MoveSpeed = moveSpeed;
    _model.ClimbSpeed = climbSpeed;
    _model.Wealth = 0;
  }

  float IPlayerModel.MoveSpeed
  {
    get
    {
      return _model.MoveSpeed;
    }
    set
    {
      float old = _model.MoveSpeed;
      _model.MoveSpeed = value;
      OnChangeMoveSpeedValue?.Invoke(old,value);
    }
  }

  float IPlayerModel.ClimbSpeed
  {
    get
    {
      return _model.ClimbSpeed;
    }
    set
    {
      float old = _model.ClimbSpeed;
      _model.ClimbSpeed = value;
      OnChangeClimbSpeedValue?.Invoke(old,value);
    }
  }

  int IPlayerModel.Wealth
  {
    get
    {
      return _model.Wealth;
    }
    set
    {
      int old = _model.Wealth;
      _model.Wealth = value;
      OnChangeWealthValue?.Invoke(old,value);
    }
  }

  public override string ModelName => "PlayerModel";

  int IPlayerUIMessage.GetWealth()
  {
    return _model.Wealth;
  }

}
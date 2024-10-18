using System;

/// <summary>
/// プレイヤーモデルインターフェース
/// </summary>
public interface IPlayerModel
{
  public float MoveSpeed {get;set;}
  public float ClimbSpeed {get;set;}
  public int Wealth {get;set;}
  public event Action<float,float> OnChangeMoveSpeedValue;
  public event Action<float,float> OnChangeClimbSpeedValue;
  public event Action<int,int> OnChangeWealthValue;
}
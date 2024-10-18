using UnityEngine;

/// <summary>
/// 登ることができるオブジェクト
/// </summary>
internal interface IClimbable
{
  /// <summary>
  /// 一番上のどれぐらい(%)登れるかを返す
  /// </summary>
  public float ClimbTopRate {get;}
  /// <summary>
  /// 一番下のどれぐらい(%)登れるかを返す
  /// </summary>
  public float ClimbBottomRate {get;}
  /// <summary>
  /// トップ座標
  /// </summary>
  public Vector2 ClimbTopPos {get;}
  /// <summary>
  /// ボトム座標
  /// </summary>
  public Vector2 ClimbBottomPos {get;}
  /// <summary>
  /// 登る長さ
  /// </summary>
  public float ClimbLength {get;}
}
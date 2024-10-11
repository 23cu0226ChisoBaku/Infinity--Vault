using UnityEngine;

/// <summary>
/// はしごなど登られる/降りられるもの
/// </summary>
internal interface IClimbable
{
    /// <summary>
    /// 今登っている/降りているオブジェクトの場所を取得
    /// </summary>
    /// <param name="canClimb">今上っているオブジェクト</param>
    /// <returns>(正規化した場所:0~1)</returns>
    public float GetClimbRate(ICanClimb canClimb);
    /// <summary>
    /// 登る/降りる
    /// </summary>
    /// <param name="canClimb">登る/降りるオブジェクト</param>
    /// <param name="moveDir">進める方向(進める長さ含み)</param>
    public void Climb(ICanClimb canClimb, Vector2 moveDir);
}
public struct RotateDialPuzzleModel
{
  public enum ERotateDial
  {
    Clockwise = 1,          // 時計回り
    CounterClockwise = -1,  // 反時計回り
  }

  // ダイヤルを回す方向
  public ERotateDial RotateDial;
  // 回す回数（周）
  public int Round;
}
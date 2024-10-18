public enum ERotateDial
{
  Clockwise = 1,          // 時計回り
  CounterClockwise = -1,  // 反時計回り
}
public struct RotateDialPuzzleModel
{
  // 回転方向
  public ERotateDial RotateDial;
  // 回転する周数
  public int Round;
}
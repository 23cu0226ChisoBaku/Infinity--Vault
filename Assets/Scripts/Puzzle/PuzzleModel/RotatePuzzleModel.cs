public struct RotateDialPuzzleModel
{
  public enum ERotateDial
  {
    Clockwise = 1,          // ���v���
    CounterClockwise = -1,  // �����v���
  }

  // �_�C�������񂷕���
  public ERotateDial RotateDial;
  // �񂷉񐔁i���j
  public int Round;
}
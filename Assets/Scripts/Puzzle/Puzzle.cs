using MEvent;
/// <summary>
/// パズルの親クラス
/// </summary>
public abstract class Puzzle
{
  protected DisposableEvent _onPuzzleClear = new DisposableEvent();
  ~Puzzle()
  {
    UnityEngine.Debug.LogWarning("HogeHoge");
    _onPuzzleClear.Dispose();
  }
}

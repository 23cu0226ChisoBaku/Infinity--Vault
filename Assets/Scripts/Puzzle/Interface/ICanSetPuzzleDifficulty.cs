internal interface ICanSetPuzzleDifficulty
{
  /// <summary>
  /// 難易度設定を行い、パズル(IPuzzle)を返す
  /// </summary>
  /// <param name="difficulty"></param>
  /// <returns></returns>
  IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty);
}
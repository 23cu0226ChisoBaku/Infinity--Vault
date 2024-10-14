using System;
using UnityEngine;

/// <summary>
/// �p�Y���C���^�[�t�F�[�X
/// </summary>
internal interface IPuzzle
{
  /// <summary>
  /// �p�Y����\��
  /// </summary>
  public void ShowPuzzle();
  /// <summary>
  /// �p�Y�����B��
  /// </summary>
  public void HidePuzzle();
  /// <summary>
  /// �p�Y�������Z�b�g
  /// </summary>
  public void ResetPuzzle();
  /// <summary>
  /// �p�Y���������ꂽ��Ăяo�����R�[���o�b�N
  /// </summary>
  public event Action OnPuzzleClear;   
}

internal interface IDialPuzzle : IPuzzle
{
  /// <summary>
  /// �_�C���������X�V����
  /// </summary>
  /// <param name="startPos">����J�[�\���̎n�_���W(���[���h���W)</param>
  /// <param name="endPos">����J�[�\���̏I�_���W(���[���h���W)</param>
  public void UpdateDial(Vector2 startPos, Vector2 endPos);
}

internal interface IButtonPuzzleUpdater : IPuzzle
{
  public void UpdateClick();
}
using UnityEngine;

/// <summary>
/// �͂����ȂǓo����/�~��������
/// </summary>
internal interface IClimbable
{
    /// <summary>
    /// ���o���Ă���/�~��Ă���I�u�W�F�N�g�̏ꏊ���擾
    /// </summary>
    /// <param name="canClimb">������Ă���I�u�W�F�N�g</param>
    /// <returns>(���K�������ꏊ:0~1)</returns>
    public float GetClimbRate(ICanClimb canClimb);
    /// <summary>
    /// �o��/�~���
    /// </summary>
    /// <param name="canClimb">�o��/�~���I�u�W�F�N�g</param>
    /// <param name="moveDir">�i�߂����(�i�߂钷���܂�)</param>
    public void Climb(ICanClimb canClimb, Vector2 moveDir);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class rotatedial : Puzzle
{
    //�萔
    private const float MAX_ROTATE_SPEED = 270f;
    private const float MAX_INPUT_ANGLE = 120f;

    //�ϐ�
    //�����t���_�C���������񂷃p�Y���f�[�^
    private RotateDialPuzzleInfo _dialnumberpuzzleInfo;
    //�񂵂��p�x�̓x��
    private float _totalRotateAngle;
    //����������߂ɉ񂷕K�v������x��
    private float _clearTargetAngle;
    //�}�E�X�Ńh���b�O���Ă��邩
    private 

    public override IPuzzle AcceptDifficulty(EPuzzleDifficulty difficulty)
    {
        throw new System.NotImplementedException();
    }

    public override void HidePuzzle()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetPuzzle()
    {
        throw new System.NotImplementedException();
    }

    public override void ShowPuzzle()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdatePuzzle()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}

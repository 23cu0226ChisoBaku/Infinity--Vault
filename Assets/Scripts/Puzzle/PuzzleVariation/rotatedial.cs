using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class rotatedial : Puzzle
{
    //定数
    private const float MAX_ROTATE_SPEED = 270f;
    private const float MAX_INPUT_ANGLE = 120f;

    //変数
    //数字付きダイヤル錠を回すパズルデータ
    private RotateDialPuzzleInfo _dialnumberpuzzleInfo;
    //回した角度の度数
    private float _totalRotateAngle;
    //謎を解くために回す必要がある度数
    private float _clearTargetAngle;
    //マウスでドラッグしているか
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

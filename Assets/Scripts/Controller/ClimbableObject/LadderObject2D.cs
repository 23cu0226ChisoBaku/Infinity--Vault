using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

internal interface ICanClimb
{
    public Transform GetTransform();
    /// <summary>
    /// �o��/�~���I�u�W�F�N�g�̃R���C�_�[�̍���(2D:Y)
    /// </summary>
    public float Collider2DHeight {get;}
    public float Collider2DOffsetY {get;}

    /// <summary>
    /// �I�u�W�F�N�g���o��/�~��n�߂�Ƃ��Ăяo�����R�[���o�b�N
    /// </summary>
    public void OnStartClimb();
    /// <summary>
    /// �I�u�W�F�N�g���o��/�~��I���Ƃ��Ăяo�����R�[���o�b�N
    /// </summary>
    public void OnEndClimb();  
}

// TODO
// ���͂͂��������Ȃ��̂ŁA����ȊO�̓o���I�u�W�F�N�g���z�肳��Ă��Ȃ��݌v�ɂȂ��Ă���
internal class LadderObject2D : MonoBehaviour,IClimbable
{
    // 0.00001f
    private static readonly float LADDER_BOTTOM_ADJUSTMENT_RATE = 1E-05f;
    // 0.99999f
    private static readonly float LADDER_TOP_ADJUSTMENT_RATE = 1f - 1E-05f;

    private float _climbLength;
    private float _ladderTopPosY;
    private float _ladderBottomPosY;

    // ���͂������g���Ă���I�u�W�F�N�g�̃��j�[�N��񂪓����Ă���R���e�i
    private HashSet<int> _climbingObjects = new HashSet<int>();
    
    private void Awake()
    {
        var collider = GetComponent<Collider2D>();
#if UNITY_EDITOR
        Assert.IsNotNull(collider,$"Ladder {gameObject.name} does not contains a collider2D");
#endif

        Vector2 centerPos = (Vector2)transform.position + collider.offset;
        _climbLength = collider.bounds.size.y;
        _ladderTopPosY = centerPos.y + _climbLength * 0.5f;
        _ladderBottomPosY = centerPos.y - _climbLength * 0.5f;
    }

    public void Climb(ICanClimb canClimb, Vector2 moveDir)
    {
        if (!canClimb.IsAlive())
        {
            return;
        }

        var climbTransform = canClimb.GetTransform();

        if (climbTransform == null)
        {
            return;
        }
        
        int instanceID = climbTransform.GetInstanceID();

        // �܂��g���Ă��Ȃ��I�u�W�F�N�g�̏���o�^
        if (!_climbingObjects.Contains(instanceID))
        {
            _climbingObjects.Add(instanceID);
            // X���W�𒲐�����
            var adjustPos = climbTransform.position;
            adjustPos.x = transform.position.x;
            climbTransform.position = adjustPos;

            canClimb.OnStartClimb();
        }

        // TODO
        // �I�u�W�F�N�g���ړ�������
        climbTransform.Translate(moveDir);

        // �ړ��ł���ꏊ���͂����͈͓̔��ɒ���
        AdjustCanClimbPos(canClimb);

    }

    public float GetClimbRate(ICanClimb canClimb)
    {
        if (!canClimb.IsAlive())
        {
            return 0f;
        }

        var climbTransform = canClimb.GetTransform();

        if (climbTransform == null)
        {
            return 0f;
        }

        // ���p�I�u�W�F�N�g�̃R���C�_�[���S���W����͂����̈�ԉ��ւ̋���
        float distanceToLadderBottom = (climbTransform.position.y + canClimb.Collider2DOffsetY - canClimb.Collider2DHeight * 0.5f) - _ladderBottomPosY;

        return distanceToLadderBottom / _climbLength;

    }

    private void AdjustCanClimbPos(ICanClimb canClimb)
    {   
        float canClimbPosRate = GetClimbRate(canClimb);

        bool isClimbOver = false;
        // ��𒴂�����
        if (canClimbPosRate > LADDER_TOP_ADJUSTMENT_RATE)
        {
            isClimbOver = true;

            var adjustPos = canClimb.GetTransform().position;
            adjustPos.y = _ladderTopPosY + canClimb.Collider2DHeight * 0.5f - canClimb.Collider2DOffsetY;
            canClimb.GetTransform().position = adjustPos;
        }
        // �����[�𒴂����牺���[�܂Œ���
        else if (canClimbPosRate < LADDER_BOTTOM_ADJUSTMENT_RATE)
        {
            isClimbOver = true;

            var adjustPos = canClimb.GetTransform().position;
            adjustPos.y = _ladderBottomPosY + canClimb.Collider2DHeight * 0.5f - canClimb.Collider2DOffsetY;
            canClimb.GetTransform().position = adjustPos;
        }

        // �g���I�������(��ԏ�������͈�ԉ��ɓ��B������)
        if (isClimbOver)
        {
            canClimb.OnEndClimb();

            // ���܂Ŏg���Ă����I�u�W�F�N�g�̏����폜
            _climbingObjects.Remove(canClimb.GetTransform().GetInstanceID());

        }
        
    }

}
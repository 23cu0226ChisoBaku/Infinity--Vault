using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

internal interface ICanClimb
{
    public Transform GetTransform();
    /// <summary>
    /// 登る/降りるオブジェクトのコライダーの高さ(2D:Y)
    /// </summary>
    public float Collider2DHeight {get;}
    public float Collider2DOffsetY {get;}

    /// <summary>
    /// オブジェクトが登り/降り始めるとき呼び出されるコールバック
    /// </summary>
    public void OnStartClimb();
    /// <summary>
    /// オブジェクトが登り/降り終わるとき呼び出されるコールバック
    /// </summary>
    public void OnEndClimb();  
}

// TODO
// 今ははしごしかないので、それ以外の登れるオブジェクトが想定されていない設計になっている
internal class LadderObject2D : MonoBehaviour,IClimbable
{
    // 0.00001f
    private static readonly float LADDER_BOTTOM_ADJUSTMENT_RATE = 1E-05f;
    // 0.99999f
    private static readonly float LADDER_TOP_ADJUSTMENT_RATE = 1f - 1E-05f;

    private float _climbLength;
    private float _ladderTopPosY;
    private float _ladderBottomPosY;

    // 今はしごを使っているオブジェクトのユニーク情報が入っているコンテナ
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

        // まだ使っていないオブジェクトの情報を登録
        if (!_climbingObjects.Contains(instanceID))
        {
            _climbingObjects.Add(instanceID);
            // X座標を調整する
            var adjustPos = climbTransform.position;
            adjustPos.x = transform.position.x;
            climbTransform.position = adjustPos;

            canClimb.OnStartClimb();
        }

        // TODO
        // オブジェクトを移動させる
        climbTransform.Translate(moveDir);

        // 移動できる場所をはしごの範囲内に調整
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

        // 利用オブジェクトのコライダー中心座標からはしごの一番下への距離
        float distanceToLadderBottom = (climbTransform.position.y + canClimb.Collider2DOffsetY - canClimb.Collider2DHeight * 0.5f) - _ladderBottomPosY;

        return distanceToLadderBottom / _climbLength;

    }

    private void AdjustCanClimbPos(ICanClimb canClimb)
    {   
        float canClimbPosRate = GetClimbRate(canClimb);

        bool isClimbOver = false;
        // 上を超えたら
        if (canClimbPosRate > LADDER_TOP_ADJUSTMENT_RATE)
        {
            isClimbOver = true;

            var adjustPos = canClimb.GetTransform().position;
            adjustPos.y = _ladderTopPosY + canClimb.Collider2DHeight * 0.5f - canClimb.Collider2DOffsetY;
            canClimb.GetTransform().position = adjustPos;
        }
        // 下っ端を超えたら下っ端まで調整
        else if (canClimbPosRate < LADDER_BOTTOM_ADJUSTMENT_RATE)
        {
            isClimbOver = true;

            var adjustPos = canClimb.GetTransform().position;
            adjustPos.y = _ladderBottomPosY + canClimb.Collider2DHeight * 0.5f - canClimb.Collider2DOffsetY;
            canClimb.GetTransform().position = adjustPos;
        }

        // 使い終わったら(一番上もしくは一番下に到達したら)
        if (isClimbOver)
        {
            canClimb.OnEndClimb();

            // 今まで使っていたオブジェクトの情報を削除
            _climbingObjects.Remove(canClimb.GetTransform().GetInstanceID());

        }
        
    }

}
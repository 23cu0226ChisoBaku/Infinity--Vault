using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;


// TODO
// 今ははしごしかないので、それ以外の登れるオブジェクトが想定されていない設計になっている
internal class LadderObject2D : MonoBehaviour,IClimbable
{
    private static readonly float LADDER_BOTTOM_ADJUSTMENT_RATE = 0f;
    private static readonly float LADDER_TOP_ADJUSTMENT_RATE = 1f;
    private float _climbLength;
    private Vector2 _ladderTopPos;
    private Vector2 _ladderBottomPos;
    float IClimbable.ClimbTopRate => LADDER_TOP_ADJUSTMENT_RATE;
    float IClimbable.ClimbBottomRate => LADDER_BOTTOM_ADJUSTMENT_RATE;

    Vector2 IClimbable.ClimbTopPos => _ladderTopPos;
    Vector2 IClimbable.ClimbBottomPos => _ladderBottomPos;
    float IClimbable.ClimbLength => _climbLength;

    private void Awake()
    {
        var collider = GetComponent<Collider2D>();
#if UNITY_EDITOR
        Assert.IsNotNull(collider,$"Ladder {gameObject.name} does not contains a collider2D");
#endif
        _climbLength = collider.bounds.size.y;

        Vector2 centerPos = (Vector2)transform.position + collider.offset;
        _ladderTopPos = centerPos + _climbLength * 0.5f * Vector2.up;
        _ladderBottomPos = centerPos + _climbLength * 0.5f * Vector2.down;
    }
}
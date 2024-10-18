using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// はしご(一番上と一番下まで行ける)
/// </summary>
internal class LadderObject2D : MonoBehaviour,IClimbable
{ 
  /// <summary>
  /// はしごの下へ行けるところ(長さと比例している)
  /// </summary>
  private static readonly float LADDER_BOTTOM_RATE = 0f;
  /// <summary>
  /// はしごの上へ行けるところ(長さと比例している)
  /// </summary>
  private static readonly float LADDER_TOP_RATE = 1f;
  private float _ladderLength;
  private Vector2 _ladderTopPos;
  private Vector2 _ladderBottomPos;
  float IClimbable.ClimbTopRate => LADDER_TOP_RATE;
  float IClimbable.ClimbBottomRate => LADDER_BOTTOM_RATE;
  Vector2 IClimbable.ClimbTopPos => _ladderTopPos;
  Vector2 IClimbable.ClimbBottomPos => _ladderBottomPos;
  float IClimbable.ClimbLength => _ladderLength;

  private void Awake()
  {
      var collider = GetComponent<Collider2D>();
  #if UNITY_EDITOR
      Assert.IsNotNull(collider,$"Ladder {gameObject.name} does not contains a collider2D");
  #endif
      _ladderLength = collider.bounds.size.y;
      Vector2 centerPos = (Vector2)transform.position + collider.offset;
      _ladderTopPos = centerPos + _ladderLength * 0.5f * Vector2.up;
      _ladderBottomPos = centerPos + _ladderLength * 0.5f * Vector2.down;
  }
}
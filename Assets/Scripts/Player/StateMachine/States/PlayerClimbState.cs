using MStateMachine;
using UnityEngine;

namespace IV
{
    namespace PlayerState
    {
        internal sealed class PlayerClimbState : PlayerState
        {
            private readonly static float NO_GRAVITY = 0f;
            private static float _defaultGravity;
            private enum EPlayerClimbDir
            {
                None = 0,
                Up = 1,
                Down = -1,  
            }
            private int _playerClimbDir;
            private IClimbable _climbableObj;
            private Vector2 _climbDirection;    // 登れるオブジェクトの一番下から一番上までの方向ベクトル;

            public PlayerClimbState(PlayerContext context)
                :base(context,PlayerStateMachine.EPlayerState.Climb)
            {
                _defaultGravity = _context.PlayerRigidbody.gravityScale;
            }

            public override void EnterState()
            {
                _context.PlayerRigidbody.gravityScale = NO_GRAVITY;
                _context.PlayerRigidbody.excludeLayers |= LayerMask.GetMask("Ground");
                _climbableObj = _context.PlayerController.GetClimbable();

                if (_climbableObj != null)
                {
                    _climbDirection = (_climbableObj.ClimbTopPos - _climbableObj.ClimbBottomPos).normalized;
                }
                else
                {
                    _climbDirection = Vector2.up;
                }

                var adjustPos = _context.PlayerGameObject.transform.position;
                var currentRate = CalculateClimbRate();
                if (currentRate <= 0.1f)
                {
                    adjustPos.x = _climbableObj.ClimbBottomPos.x;
                }
                else
                {
                    adjustPos.x = _climbableObj.ClimbTopPos.x;
                }
                _context.PlayerGameObject.transform.position = adjustPos;

            }

            public override void ExitState()
            {
                _context.PlayerRigidbody.gravityScale = _defaultGravity;
                _context.PlayerRigidbody.excludeLayers &= ~LayerMask.GetMask("Ground");
                _climbableObj = null;
                _climbDirection = Vector2.zero;
            }

            public override void UpdateState(float deltaTime)
            {
                if (_context.PlayerController.GetClimbable() == null)
                {
                    return;
                }
                else
                {
                    _playerClimbDir = 0;

                    if (Input.GetKey(KeyCode.W))
                    {
                        _playerClimbDir += (int)EPlayerClimbDir.Up;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        _playerClimbDir += (int)EPlayerClimbDir.Down;
                    }

                    float playerClimbSpeed = _context.Model.ClimbSpeed;
                    _context.PlayerGameObject.transform.Translate(_playerClimbDir * playerClimbSpeed * deltaTime * _climbDirection);

                    if (_playerClimbDir != 0)
                    {
                        AdjustPos();
                    }
                }
            }

            private void AdjustPos()
            {
                float canClimbPosRate = CalculateClimbRate();

                bool isClimbOver = false;
                // 上を超えたら
                if (canClimbPosRate >= _climbableObj.ClimbTopRate)
                {
                    isClimbOver = true;
                    
                    var adjustPos = _context.PlayerGameObject.transform.position;

                    adjustPos.x =  _climbableObj.ClimbBottomPos.x
                                 + _climbableObj.ClimbLength * _climbableObj.ClimbTopRate * _climbDirection.x
                                 - _context.PlayerCollider.offset.x;

                    adjustPos.y =  _climbableObj.ClimbBottomPos.y
                                 + _climbableObj.ClimbLength * _climbableObj.ClimbTopRate * _climbDirection.y
                                 + _context.PlayerCollider.bounds.size.y * 0.5f 
                                 - _context.PlayerCollider.offset.y;

                    _context.PlayerGameObject.transform.position = adjustPos;
                }
                // 下っ端を超えたら下っ端まで調整
                else if (canClimbPosRate < _climbableObj.ClimbBottomRate)
                {
                    isClimbOver = true;
                    
                    var adjustPos = _context.PlayerGameObject.transform.position;
 
                    adjustPos.x =  _climbableObj.ClimbBottomPos.x
                                 + _climbableObj.ClimbLength * _climbableObj.ClimbBottomRate * _climbDirection.x
                                 - _context.PlayerCollider.offset.x;
                                 
                    adjustPos.y =  _climbableObj.ClimbBottomPos.y
                                 + _climbableObj.ClimbLength * _climbableObj.ClimbBottomRate * _climbDirection.y
                                 + _context.PlayerCollider.bounds.size.y * 0.5f 
                                 - _context.PlayerCollider.offset.y;

                    _context.PlayerGameObject.transform.position = adjustPos;

                }

                // 使い終わったら(一番上もしくは一番下に到達したら)
                if (isClimbOver)
                {
                    _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Walk);
                }
            }

            private float CalculateClimbRate()
            {
                if (_climbableObj.ClimbLength <= 0f)
                {
                    return 0f;
                }
                else
                {
                    float rate = (_context.PlayerGameObject.transform.position.y + _context.PlayerCollider.offset.y - _context.PlayerCollider.bounds.size.y * 0.5f - _climbableObj.ClimbBottomPos.y)
                                /(_climbDirection * _climbableObj.ClimbLength).y;

                    return rate;
                }
            }
        }
    }
}
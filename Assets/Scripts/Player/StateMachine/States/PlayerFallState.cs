using UnityEngine;
using MStateMachine;

namespace IV
{
  namespace PlayerState
  {
    internal sealed class PlayerFallState : PlayerState
    {

      private readonly static float GRAVITY;
      private readonly static float MAX_FALL_SPEED;
      static PlayerFallState()
      {
        GRAVITY = 100f;
        MAX_FALL_SPEED = 10f;
      }
      public PlayerFallState(PlayerContext context)
          :base(context,PlayerStateMachine.EPlayerState.Fall)
      {

      }
      public override void EnterState()
      {
        _context.PlayerRigidbody.velocity = Vector2.zero;
      }

      public override void ExitState()
      {
        _context.PlayerRigidbody.velocity = Vector2.zero;
      }
      public override void UpdateState(float deltaTime)
      {
        if (_context.PlayerController.IsGrounded())
        {
          _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
        }
      }
      public override void FixedUpdateState(float fixedDeltaTime)
      { 
        // 落下速度を計算
        if (Mathf.Abs(_context.PlayerRigidbody.velocity.y) <= MAX_FALL_SPEED)
        {
          Vector2 newFallVelocity = _context.PlayerRigidbody.velocity;

          newFallVelocity.y += -GRAVITY * fixedDeltaTime;
          // 最大落下速度制限
          newFallVelocity.y = newFallVelocity.y <= -MAX_FALL_SPEED ? -MAX_FALL_SPEED : newFallVelocity.y;

          _context.PlayerRigidbody.velocity = newFallVelocity;
        }
      }
    }
  }
}
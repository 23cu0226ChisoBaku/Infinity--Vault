using MStateMachine;
using Unity.Mathematics;
using UnityEngine;

namespace IV
{
  namespace PlayerState
  {
    internal sealed class PlayerIdleState : PlayerState
    {
      private readonly static float DEFAULT_GRAVITY;
      static PlayerIdleState()
      {
        DEFAULT_GRAVITY = 1f;
      }
      public PlayerIdleState(PlayerContext context)
          :base(context,PlayerStateMachine.EPlayerState.Idle)
      {

      }

      public override void EnterState()
      {
        _context.PlayerRigidbody.gravityScale = DEFAULT_GRAVITY;
        _context.PlayerRigidbody.velocity = Vector2.zero;
      }

      public override void ExitState()
      {
          
      }

      public override void UpdateState(float deltaTime)
      {
        // 左右移動ボタンを押したら歩く状態
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
          _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Walk);
        }
        // 登るオブジェクトが存在すれば
        else if (_context.PlayerController.GetClimbable().IsAlive())
        {
          // 上下移動ボタンを押したら登る状態
          if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
          {
            _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Climb);
          }
        }
        // アビリティ発動ボタンを押したら
        else if (Input.GetKeyDown(KeyCode.Space))
        {
          _context.PlayerController.GetAbility()?.ActiveAbility();
        }
        // 接地していなかったら落下状態
        else if (!_context.PlayerController.IsGrounded())
        {
          _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Fall);
        }
      }
    }
  }
}
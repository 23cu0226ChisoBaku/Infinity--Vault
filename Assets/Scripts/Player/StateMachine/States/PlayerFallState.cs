using UnityEngine;
using MStateMachine;

namespace IV
{
    namespace PlayerState
    {
        internal sealed class PlayerFallState : PlayerState
        {
            private readonly float GRAVITY;
            private readonly float MAX_FALL_SPEED;
            public PlayerFallState(PlayerContext context)
                :base(context,PlayerStateMachine.EPlayerState.Fall)
            {
                GRAVITY = 100f;
                MAX_FALL_SPEED = 10f;
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
                if (Mathf.Abs(_context.PlayerRigidbody.velocity.y) <= 10f)
                {
                    Vector2 newFallVelocity = _context.PlayerRigidbody.velocity;

                    newFallVelocity.y += -GRAVITY * fixedDeltaTime;

                    // Å‘å—Ž‰º‘¬“x§ŒÀ
                    newFallVelocity.y = newFallVelocity.y <= -MAX_FALL_SPEED ? -MAX_FALL_SPEED : newFallVelocity.y;

                    _context.PlayerRigidbody.velocity = newFallVelocity;
                }
            }
        }
    }
}
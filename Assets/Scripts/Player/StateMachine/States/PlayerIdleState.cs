using MStateMachine;
using UnityEngine;

namespace IV
{
    namespace PlayerState
    {
        internal sealed class PlayerIdleState : PlayerState
        {
            public PlayerIdleState(PlayerContext context)
                :base(context,PlayerStateMachine.EPlayerState.Idle)
            {

            }

            public override void EnterState()
            {
                _context.PlayerRigidbody.gravityScale = 0f;
                _context.PlayerRigidbody.velocity = Vector2.zero;
            }

            public override void ExitState()
            {
                
            }

            public override void UpdateState(float deltaTime)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Walk);
                }
                else if (_context.PlayerController.GetClimbable() != null)
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                    {
                        _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Climb);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    _context.PlayerController.GetAbility()?.ActiveAbility();
                }
                else if (!_context.PlayerController.IsGrounded())
                {
                    _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Fall);
                }
                
            }
        }
    }
}
using UnityEngine;

namespace IV
{
    namespace PlayerState
    {
        internal sealed class PlayerWalkState : PlayerState
        {
            private enum EPlayerMoveDir
            {
                None = 0,
                Left = -1,
                Right = 1,
            }

            private int _playerMoveDir;

            public PlayerWalkState(PlayerContext context)
                :base(context, PlayerStateMachine.EPlayerState.Walk)
            {

            }

            public override void EnterState()
            {
                _playerMoveDir = 0;
            }

            public override void ExitState()
            {
                _context.PlayerRigidbody.velocity = Vector2.zero;
            }


            public override void UpdateState(float deltaTime)
            {
                _playerMoveDir = 0;

                if (Input.GetKey(KeyCode.A))
                {
                    _playerMoveDir += (int)EPlayerMoveDir.Left;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    _playerMoveDir += (int)EPlayerMoveDir.Right;
                }

                if (_playerMoveDir == 0)
                {
                    _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
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
            public override void FixedUpdateState(float fixedDeltaTime)
            {
                float playerMoveSpeed = _context.Model.MoveSpeed;
                _context.PlayerRigidbody.velocity = new Vector2(_playerMoveDir * playerMoveSpeed, 0);
            }
        }
    }
}
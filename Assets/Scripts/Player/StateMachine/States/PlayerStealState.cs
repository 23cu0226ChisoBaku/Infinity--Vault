using MStateMachine;
using UnityEngine;

namespace IV
{
    namespace PlayerState
    {
        internal sealed class PlayerStealState : PlayerState
        {
            public PlayerStealState(PlayerContext context)
                :base(context,PlayerStateMachine.EPlayerState.Steal)
            {

            }

            public override void EnterState()
            {
                Time.timeScale = 0.2f;
            }

            public override void ExitState()
            {
                Time.timeScale = 1f;
            }


            public override void UpdateState(float deltaTime)
            {
              if (_context.PlayerController.GetInteractable() == null)
              {
                _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
              }
              Debug.Log("Stealing");
              if (Input.GetKeyDown(KeyCode.E))
              {
                _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
                PuzzleManager.Instance.ExitPuzzle();
              }

            }
            public override void FixedUpdateState(float fixedDeltaTime)
            {
                
            }
        }
    }
}
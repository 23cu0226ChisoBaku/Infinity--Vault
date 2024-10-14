using MStateMachine;

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

            }

            public override void ExitState()
            {
                
            }


            public override void UpdateState(float deltaTime)
            {
                
            }
            public override void FixedUpdateState(float fixedDeltaTime)
            {
                
            }
        }
    }
}
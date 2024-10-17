using UnityEngine;

namespace IV
{
    namespace PlayerState
    {
        internal sealed class PlayerAbilityState : PlayerState
        {
            private Material playerMat;
            public PlayerAbilityState(PlayerContext context)
                :base(context,PlayerStateMachine.EPlayerState.Ability)
            {
                playerMat = null;
            }

            public override void EnterState()
            {
                if (playerMat == null)
                {
                    Renderer renderer = _context.PlayerGameObject.GetComponentInChildren<Renderer>();
                    playerMat = renderer.material;

                    renderer.material = playerMat;
                }
                
                {
                    var color = playerMat.color;
                    color.a = 0.2f;
                    playerMat.color = color;
                }
            }

            public override void ExitState()
            {
                {
                    var color = playerMat.color;
                    color.a = 1f;
                    playerMat.color = color;
                }
            }

            public override void UpdateState(float deltaTime)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _context.PlayerController.GetAbility()?.FinishAbility();
                }
            }

            ~PlayerAbilityState()
            {
                if (playerMat != null)
                {
                    Object.Destroy(playerMat);
                }
            }
        }
    }
}
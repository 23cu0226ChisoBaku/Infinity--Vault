using MStateMachine;
using UnityEngine;

namespace IV
{
    namespace PlayerState
    {
        internal sealed class PlayerAbilityState : PlayerState
        {
            public PlayerAbilityState(PlayerContext context)
                :base(context,PlayerStateMachine.EPlayerState.Ability)
            {

            }

            public override void EnterState()
            {
                var renderers = _context.PlayerGameObject.GetComponentsInChildren<Renderer>();
                foreach(var renderer in renderers)
                {
                    var color = renderer.sharedMaterial.color;
                    color.a = 0.2f;
                    renderer.sharedMaterial.color = color;
                }
            }

            public override void ExitState()
            {
                var renderers = _context.PlayerGameObject.GetComponentsInChildren<Renderer>();
                foreach(var renderer in renderers)
                {
                    var color = renderer.sharedMaterial.color;
                    color.a = 1f;
                    renderer.sharedMaterial.color = color;
                }
            }

            public override void UpdateState(float deltaTime)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _context.PlayerController.GetAbility()?.FinishAbility();
                }
            }
        }
    }
}
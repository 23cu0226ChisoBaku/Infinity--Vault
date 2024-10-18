using UnityEngine;
using MStateMachine;
using Unity.VisualScripting;

namespace IV
{
  namespace PlayerState
  {
    internal sealed class PlayerStateMachine : StateMachine<PlayerStateMachine.EPlayerState>, IUnityPhysicsBaseStateMachine<PlayerStateMachine.EPlayerState>
    {
      public enum EPlayerState
      {
        Idle = 0,
        Walk,
        Climb,
        Fall,
        Ability,
        Steal,
      }

      public PlayerStateMachine(IPlayerModel playerModel, GameObject playerGameObject)
      {
        if (playerModel == null || playerGameObject == null)
        {
#if UNITY_EDITOR
          UnityEngine.Assertions.Assert.IsNotNull(playerModel,$"{playerModel.GetType().Name} is null");
          UnityEngine.Assertions.Assert.IsNotNull(playerGameObject,$"{playerGameObject.name} is null or invalid");
#endif
          return;
        }
        else
        {
          PlayerContext playerContext = new PlayerContext(playerModel,playerGameObject,this);
          
          AddState(EPlayerState.Idle, new PlayerIdleState(playerContext));
          AddState(EPlayerState.Walk, new PlayerWalkState(playerContext));
          AddState(EPlayerState.Climb, new PlayerClimbState(playerContext));
          AddState(EPlayerState.Ability, new PlayerAbilityState(playerContext));
          AddState(EPlayerState.Steal, new PlayerStealState(playerContext));
          AddState(EPlayerState.Fall, new PlayerFallState(playerContext));
        }
      }   
      
      void IUnityPhysicsBaseStateMachine<EPlayerState>.FixedUpdate(float fixedDeltaTime)
      {
        _currentState?.FixedUpdateState(fixedDeltaTime);
      }      
    }
  }
}
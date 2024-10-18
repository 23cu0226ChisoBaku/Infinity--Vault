using System.Runtime.Remoting.Contexts;
using IV.PlayerState;
using MStateMachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace IV
{
  namespace PlayerState
  {      
    internal sealed class PlayerContext
    {
      private IPlayerModel _model;
      private ISwitchState<PlayerStateMachine.EPlayerState> _stateMachineSwitch;
      private GameObject _playerGameObj;
      private Rigidbody2D _playerRigidbody;
      private Collider2D _playerCollider;
      private PlayerController _playerController;
      public PlayerContext(IPlayerModel model, GameObject playerGameObj,ISwitchState<PlayerStateMachine.EPlayerState> switchState)
      {
        Assert.IsNotNull(model, "Player Model is NULL!!!");
        Assert.IsNotNull(playerGameObj, "Player GameObject is NULL!!!");
        Assert.IsNotNull(switchState, "Player State Machine is NULL!!!");

        _model = model;
        _playerGameObj = playerGameObj;
        _stateMachineSwitch = switchState;

        _playerRigidbody = _playerGameObj.GetOrAddComponent<Rigidbody2D>();

        _playerCollider = _playerGameObj.GetComponent<Collider2D>();

        Assert.IsNotNull(_playerCollider, "Player does not contains a Collider2D,Please attach one");

        _playerController = _playerGameObj.GetComponent<PlayerController>();

      }

      public IPlayerModel Model => _model;
      public GameObject PlayerGameObject => _playerGameObj;
      public Rigidbody2D PlayerRigidbody => _playerRigidbody;
      public Collider2D PlayerCollider => _playerCollider;
      public PlayerController PlayerController => _playerController;
      public ISwitchState<PlayerStateMachine.EPlayerState> StateMachineSwitch => _stateMachineSwitch;
    }
  }
}
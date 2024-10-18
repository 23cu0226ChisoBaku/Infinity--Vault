using MStateMachine;

namespace IV
{
  namespace PlayerState
  {
    internal abstract class PlayerState : State<PlayerStateMachine.EPlayerState>
    {
      protected internal PlayerContext _context;
      public PlayerState(PlayerContext context, PlayerStateMachine.EPlayerState playerState)
          :base(playerState)
      {
          _context = context;
      }
    }
  }
}
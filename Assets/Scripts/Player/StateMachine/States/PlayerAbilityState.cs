using UnityEngine;

namespace IV
{
  namespace PlayerState
  {
    /// <summary>
    /// アビリティステート
    /// TODO リファクタリングする予定
    /// </summary>
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
        // TODO　
        // 今のアビリティ設計が終わっていないため、直接にここでアビリティのエフェクトを出す
        if (playerMat == null)
        {
          Renderer renderer = _context.PlayerGameObject.GetComponentInChildren<Renderer>();
          playerMat = renderer.material;

          renderer.material = playerMat;
        }
        
        // 隠れ身の術
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
        // TODO
        // 実行したアビリティをもう一回アビリティ実行ボタンを押すと終わらせる
        // 設計が完成してないためこういう風にやってしまった
        if (Input.GetKeyDown(KeyCode.Space))
        {
          _context.PlayerController.GetAbility()?.FinishAbility();
        }
      }

      ~PlayerAbilityState()
      {
        // メモリリークを防ぐ
        if (playerMat != null)
        {
          Object.Destroy(playerMat);
        }
      }
    }
  }
}
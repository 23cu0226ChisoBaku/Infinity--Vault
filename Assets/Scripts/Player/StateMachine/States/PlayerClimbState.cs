using System.Drawing.Text;
using MStateMachine;
using UnityEngine;

namespace IV
{
  namespace PlayerState
  {
    /// <summary>
    /// プレイヤー登るステート
    /// </summary>
    internal sealed class PlayerClimbState : PlayerState
    {
// プライベートフィールド
#region Private Field
      /// <summary>
      /// 登るときの重力
      /// </summary>
      private readonly static float NO_GRAVITY = 0f;
      /// <summary>
      /// 登る前の重力
      /// </summary>
      private static float _previousGravity;
      /// <summary>
      /// プレイヤー登る方向(EPlayerClimbDir列挙の値)
      /// </summary>
      private int _playerClimbDir;
      // 登る方向
      private Vector2 _climbDirection;  
      /// <summary>
      /// 登る方向を表す列挙
      /// </summary>
      private enum EPlayerClimbDir
      {
          None = 0,
          Up = 1,
          Down = -1,  
      }
#endregion Private Field
// End of プライベートフィールド
      public PlayerClimbState(PlayerContext context)
          :base(context,PlayerStateMachine.EPlayerState.Climb)
      {
          
      }

      public override void EnterState()
      {
        // プレイヤーのパラメーター設定
        _previousGravity = _context.PlayerRigidbody.gravityScale;
        _context.PlayerRigidbody.gravityScale = NO_GRAVITY;
        _context.PlayerRigidbody.excludeLayers |= LayerMask.GetMask("Ground");

        // 登る方向を決める
        var climbableObj = _context.PlayerController.GetClimbable();
        if (climbableObj.IsAlive())
        {
            _climbDirection = (climbableObj.ClimbTopPos - climbableObj.ClimbBottomPos).normalized;
        }
        // ヌルだったら待機状態に戻る
        else
        {
          _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
          return;
        }

        // プレイヤーの座標を調整する
        var currentRate = CalculateClimbRate(climbableObj);
        // 下っ端だったら下の座標に合わせる
        // TODO マジックナンバー0.1をどうにかしたい
        // TODO リファクタリングする予定
        var adjustPos = _context.PlayerGameObject.transform.position;
        if (currentRate <= 0.1f)
        {
          adjustPos.x = climbableObj.ClimbBottomPos.x;
        }
        // 上の座標に合わせる
        else
        {
          adjustPos.x = climbableObj.ClimbTopPos.x;
        }
        _context.PlayerGameObject.transform.position = adjustPos;
      }

      public override void ExitState()
      {
        _context.PlayerRigidbody.gravityScale = _previousGravity;
        _context.PlayerRigidbody.excludeLayers &= ~LayerMask.GetMask("Ground");
        _climbDirection = Vector2.zero;
      }

      public override void UpdateState(float deltaTime)
      {
        // 登れるものがなければ待機状態に戻る
        if (_context.PlayerController.GetClimbable() == null)
        {
          _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
          return;
        }
        else
        {
          // プレイヤーの登る入力チェック
          _playerClimbDir = 0;
          // TODO リファクタリングする予定
          if (Input.GetKey(KeyCode.W))
          {
            _playerClimbDir += (int)EPlayerClimbDir.Up;
          }
          if (Input.GetKey(KeyCode.S))
          {
            _playerClimbDir += (int)EPlayerClimbDir.Down;
          }

          // 移動する
          _context.PlayerGameObject.transform.Translate(_playerClimbDir * _context.Model.ClimbSpeed * deltaTime * _climbDirection);

          // 移動が発生したら座標の調整を試みる
          if (_playerClimbDir != 0)
          {
            AdjustPos();
          }
        }
      }

      /// <summary>
      /// 座標調整内部実装
      /// </summary>
      private void AdjustPos()
      {
        var climbableObj = _context.PlayerController.GetClimbable();
        // ヌルチェック
        if (!climbableObj.IsAlive())
        {
          _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
          return;
        }

        bool isClimbOver = false;
        // 今登るオブジェクトのどの辺にいるかを計算する（0~1）
        float canClimbPosRate = CalculateClimbRate(climbableObj);

        // 上へできる範囲まで上にいたら調整
        if (canClimbPosRate >= climbableObj.ClimbTopRate)
        {
          isClimbOver = true;
          
          var adjustPos = _context.PlayerGameObject.transform.position;

          adjustPos.x =  climbableObj.ClimbBottomPos.x
                        + climbableObj.ClimbLength * climbableObj.ClimbTopRate * _climbDirection.x
                        - _context.PlayerCollider.offset.x;

          adjustPos.y =  climbableObj.ClimbBottomPos.y
                        + climbableObj.ClimbLength * climbableObj.ClimbTopRate * _climbDirection.y
                        + _context.PlayerCollider.bounds.size.y * 0.5f 
                        - _context.PlayerCollider.offset.y;

          _context.PlayerGameObject.transform.position = adjustPos;
        }
        // 下へできる範囲まで下にいたら調整
        else if (canClimbPosRate < climbableObj.ClimbBottomRate)
        {
          isClimbOver = true;
          
          var adjustPos = _context.PlayerGameObject.transform.position;

          adjustPos.x =  climbableObj.ClimbBottomPos.x
                        + climbableObj.ClimbLength * climbableObj.ClimbBottomRate * _climbDirection.x
                        - _context.PlayerCollider.offset.x;
                        
          adjustPos.y =  climbableObj.ClimbBottomPos.y
                        + climbableObj.ClimbLength * climbableObj.ClimbBottomRate * _climbDirection.y
                        + _context.PlayerCollider.bounds.size.y * 0.5f 
                        - _context.PlayerCollider.offset.y;

          _context.PlayerGameObject.transform.position = adjustPos;
        }

        // 登ることが終了したら待機状態に戻る
        if (isClimbOver)
        {
          _context.StateMachineSwitch.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
        }
      }
      
      /// <summary>
      /// 登るオブジェクトのどの辺にいるかの内部実装
      /// </summary>
      /// <returns>正規化された場所(0~1)</returns>
      private float CalculateClimbRate(IClimbable climbableObj)
      {
        if (!climbableObj.IsAlive() || climbableObj.ClimbLength <= 0f)
        {
          return 0f;
        }
        else
        {
          float rate = (_context.PlayerGameObject.transform.position.y + _context.PlayerCollider.offset.y - _context.PlayerCollider.bounds.size.y * 0.5f - climbableObj.ClimbBottomPos.y)
                      /(_climbDirection * climbableObj.ClimbLength).y;

          return rate;
        }
      }
    }
  }
}
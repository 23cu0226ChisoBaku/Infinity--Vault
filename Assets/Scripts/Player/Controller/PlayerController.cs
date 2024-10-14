using System;
using UnityEngine;
using MMVCFramework.Model;
using System.Runtime.Remoting.Messaging;
using MStateMachine;
using IV.PlayerState;
using IV;

public interface IPlayerModel
{
    public float MoveSpeed {get;set;}
    public float ClimbSpeed {get;set;}
    public int Wealth {get;set;}
    public event Action<float,float> OnChangeMoveSpeedValue;
    public event Action<float,float> OnChangeClimbSpeedValue;
    public event Action<int,int> OnChangeWealthValue;
}

[RequireComponent(typeof(PlayerModelContainer))]
public class PlayerController : MonoBehaviour, IItemGetable
{
    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _capsuleCollider;
    
    private PhysicsMaterial2D _noFrictionMat;
    private PlayerModelContainer _playerModelContainer;

    private bool _isClimbable;
    private IClimbable _climbable;

    // TODO
    private IInteractable _interactable = null;

    #region Player Ability (Experimental)

    private IAbility _ability;

    #endregion Player Ability
    // End of Player Ability

    private IUnityPhysicsBaseStateMachine<PlayerStateMachine.EPlayerState> _playerStateMachine;

#region Raycast Ground Check
    private RaycastHit2D[] _groundHit2Ds;
#endregion Raycast Ground Check
// end of Raycast Ground Check

#region Unity main loop message
    private void Awake()
    {
        // TODO
        // テストのため、プレイヤーをアイテムの生成にする
        VaultManager.Instance.InitItem();
        
        _isClimbable = false;

        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (_rigidbody2D != null)
        {
            _noFrictionMat = new PhysicsMaterial2D();
            _noFrictionMat.friction = 0f;
            _rigidbody2D.sharedMaterial = _noFrictionMat;
        }

        _playerModelContainer = GetComponent<PlayerModelContainer>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();

        // ステートマシン初期化
        {
            _playerStateMachine = new PlayerStateMachine(PlayerModelContainer.PLAYER_MODEL,gameObject);

            _playerStateMachine.InitStateMachine(PlayerStateMachine.EPlayerState.Idle);
        }
        _groundHit2Ds = new RaycastHit2D[2];

        // アビリティ作成
        {
            _ability = new HiddenAbility(5f);
            _ability.OnActiveAbility += () => {
                                                _playerStateMachine.SwitchNextState(PlayerStateMachine.EPlayerState.Ability);
                                                _capsuleCollider.enabled = false;
                                            };
            _ability.OnFinishAbility += () => {
                                                _playerStateMachine.SwitchNextState(PlayerStateMachine.EPlayerState.Idle);
                                                _capsuleCollider.enabled = true;
                                            };
            _ability.OnCooldown += deltaTime => {
                                                    Debug.Log(_ability.LeftCoolTime);
                                                };
            _ability.OnFinishCooldown += () =>  {
                                                    Debug.Log($"{_ability.AbilityName} Finish Cooldown");
                                                };
        }
    }
    private void Update()
    {
        // アビリティクールダウン
        {
            if ((_ability != null) && _ability.IsCoolingDown())
            {
                _ability.CooldownAbility(Time.deltaTime);
            }
        }

        if (_interactable.IsAlive())
        {
            if (Input.GetKeyDown(_interactable.GetTargetInfo().InteractKey))
            {
                _interactable.DoInteract();
            }
        }

        Debug.Log(_playerStateMachine.CurrentState);

        _playerStateMachine.Update(Time.deltaTime);

    }

    private void Test()
    {
        Debug.Log("HogeHoge");
    }

    private void FixedUpdate() 
    {
        // var fallSpeed = _rigidbody2D.velocity.y < -10f ? -10f : _rigidbody2D.velocity.y;
        // if (_state == PlayerState.Climb)
        // {
        //     fallSpeed = 0f;
        // }
        // _rigidbody2D.velocity = new Vector2((int)_moveDir * _playerModelContainer.MoveSpeed, fallSpeed);

        _playerStateMachine.FixedUpdate(Time.fixedDeltaTime);

    }
#endregion Unity main loop message
// end of Unity main loop message

#region Interface

    public void GetItem(GemContainer gem)
    {
        PlayerModelContainer playerModel = GetComponent<PlayerModelContainer>();
        playerModel.AddWealth(gem.Worth);
    }

    public void GetItem(KeyItemContainer keyItem)
    {
        throw new System.NotImplementedException();
    }

    public void GetItem(ConsumeItemContainer consumeItem)
    {
        throw new System.NotImplementedException();
    }

    internal IClimbable GetClimbable() => _climbable;

    public void SetAbility(IAbility ability)
    {
        _ability = ability;
    }

    public IAbility GetAbility() => _ability;

    public bool IsGrounded()
    {
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.NoFilter();

        filter2D.useLayerMask = true;
        filter2D.SetLayerMask(LayerMask.GetMask("Ground"));

        int rayHitCnt = 0;

        // プレイヤーコライダー真ん中の左端から下へレイを飛ばす
        rayHitCnt += Physics2D.Raycast(
                                    (Vector2)transform.position + _capsuleCollider.offset + _capsuleCollider.size.x * 0.5f * Vector2.left, 
                                    Vector2.down, 
                                    filter2D, 
                                    _groundHit2Ds, 
                                    _capsuleCollider.size.y * 0.5f + 0.01f
                                   );

        // プレイヤーコライダー真ん中の右端から下へレイを飛ばす
        rayHitCnt += Physics2D.Raycast(
                                    (Vector2)transform.position + _capsuleCollider.offset + _capsuleCollider.size.x * 0.5f * Vector2.right, 
                                    Vector2.down, 
                                    filter2D, 
                                    _groundHit2Ds, 
                                    _capsuleCollider.size.y * 0.5f + 0.01f
                                   );
        
        if (rayHitCnt > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

#endregion Interface
// end of Interface

#region Unity Message
    private void OnTriggerEnter2D(Collider2D other) 
    {

        if (_interactable == null)
        {
            if (other.gameObject.TryGetComponent(out _interactable))
            {
                _interactable.ActiveInteract();
            }
        }

        if (_climbable == null)
        {
            if (other.gameObject.TryGetComponent(out _climbable))
            {
                _isClimbable = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out _interactable))
        {
            _interactable.EndInteract();
            _interactable = null;
        }

        if (other.gameObject.TryGetComponent(out _climbable))
        {
            _climbable = null;
            _isClimbable = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.TryGetComponent(out IPickable pickable))
        {
            pickable.OnPick(this);
        }
    }

    private void OnDestroy() 
    {
        if (_noFrictionMat != null)
        {
            Destroy(_noFrictionMat);
            _noFrictionMat = null;
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.cyan;
        if(_capsuleCollider != null)
        {
            Gizmos.DrawLine(
                                (Vector2)transform.position + _capsuleCollider.offset,
                                (Vector2)transform.position + _capsuleCollider.offset + Vector2.down * _capsuleCollider.size.y * 0.5f + Vector2.down * 0.01f
                           );  
        }
    }

#endif
    // DrawGizmos

    #endregion Unity Message
    // End of Unity Message


}
using MDesingPattern.MMediator;
using UnityEngine;

public interface IParam<T>
{
    T GetParam();
    void SetParam(T param);
}
public interface IParametrizable
{
    T GetParameter<T>();
    void SetParameter<T>(T param);
}
public class Param<T> : IParam<T> where T : struct
{
    protected T param;
    public T GetParam() => param;
    public void SetParam(T param) => this.param = param;

}

public class CharaModel : Param<CharaParam>
{
    public CharaModel()
    {
        param = new CharaParam();
    }
    public CharaModel(float moveSpeed, float climbSpeed)
        : this()
    {
        param.MoveSpeed = moveSpeed;
        param.ClimbSpeed = climbSpeed;
        param.Wealth = 0;
    }
}

[RequireComponent(typeof(PlayerModel))]
public class PlayerController : MonoBehaviour,IParametrizable,IItemGetable,ICanClimb
{
    private enum PlayerState
    {
        None = 0,
        Climb,
    }
    private enum MoveDir
    {
        None = 0,
        Left = -1,
        Right = 1,
    }
    private enum VerticalDir
    {
        None = 0,
        Up = 1,
        Down = -1,
    }
    private Rigidbody2D _rigidbody2D;

    private CapsuleCollider2D _capsuleCollider;
    
    private PhysicsMaterial2D _noFrictionMat;
    private IParam<CharaParam> _param = new CharaModel();

    private bool _isClimbable;
    private bool _isClimbing;
    private IClimbable _climbable;
    private VerticalDir vertical = VerticalDir.None;
    private float _ladderAdjustPosX;

    private MoveDir _moveDir = MoveDir.None;

    private PlayerState _state = PlayerState.None;

    private Transform _interactTarget;

    // TODO
    private IInteractable _interactable = null;

    #region Player Ability (Experimental)

    private AbilityController _abilityController;

    #endregion Player Ability
    // End of Player Ability

    public float Collider2DHeight => _capsuleCollider.bounds.size.y;

    public float Collider2DOffsetY => _capsuleCollider.offset.y;

#region Unity main loop message
    private void Awake()
    {
        // TODO
        // テストのため、プレイヤーをアイテムの生成にする
        VaultManager.Instance.InitItem();
        
        _isClimbable = false;
        _isClimbing = false;

        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (_rigidbody2D != null)
        {
            _noFrictionMat = new PhysicsMaterial2D();
            _noFrictionMat.friction = 0f;
            _rigidbody2D.sharedMaterial = _noFrictionMat;
        }

        _capsuleCollider = GetComponent<CapsuleCollider2D>();

    }
    private void Update()
    {
        _moveDir = MoveDir.None;
        vertical = VerticalDir.None;

        // TODO weird design
        if (_isClimbable)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                _isClimbing = true;
                _state = PlayerState.Climb;
            }
        }

        switch (_state)
        {
            case PlayerState.None:
            {
                if (Input.GetKey(KeyCode.A))
                {
                    _moveDir = MoveDir.Left;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    _moveDir = MoveDir.Right;
                }
            }
            break;
            case PlayerState.Climb:
            {
                if (Input.GetKey(KeyCode.W))
                {
                    vertical = VerticalDir.Up;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    vertical = VerticalDir.Down;
                }

            }
            break;
        }

        if (_interactable.IsAlive())
        {
            if (Input.GetKeyDown(_interactable.GetTargetInfo().InteractKey))
            {
                _interactable.DoInteract();
            }
        }

        if (_climbable.IsAlive())
        {
            if (_isClimbing)
            {
                _climbable.Climb(this, (int)vertical * _param.GetParam().ClimbSpeed * Time.deltaTime * Vector2.up);
            }
        }

    }

    private void FixedUpdate() 
    {
        var fallSpeed = _rigidbody2D.velocity.y < -10f ? -10f : _rigidbody2D.velocity.y;
        if (_state == PlayerState.Climb)
        {
            fallSpeed = 0f;
        }
        _rigidbody2D.velocity = new Vector2((int)_moveDir * _param.GetParam().MoveSpeed, fallSpeed);

    }
#endregion Unity main loop message
// end of Unity main loop message

#region Interface
    public void SetParam(CharaParam charaParam)
    {
        _param.SetParam(charaParam);
    }
    
    public T GetParameter<T>()
    {
        if (typeof(CharaParam).IsAssignableFrom(typeof(T)))
        {
            return (T)_param;
        }

        // TODO
        throw new System.Exception($"Param type of {typeof(T).Name} does not exist");
    }

    public void SetParameter<T>(T param)
    {
        if (typeof(CharaParam).IsAssignableFrom(typeof(T)))
        {
            _param.SetParam((CharaParam)(object)param);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"Param type of {typeof(T).Name} does not exist");
#endif
        }
    }
    
    public void GetItem(ItemInfo item)
    {
        switch(item.Type)
        {
            case EItemType.Worth:
            {
                PlayerModel model = GetComponent<PlayerModel>();
                model.AddWealth(100);
            }
            break;
            default:
            {
                throw new System.NotImplementedException();
            }
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnStartClimb()
    {
        _isClimbable = true;
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.excludeLayers |= LayerMask.GetMask("Ground");
    }

    public void OnEndClimb()
    {
        _state = PlayerState.None;
        _rigidbody2D.gravityScale = 10f;
        _rigidbody2D.excludeLayers &= ~LayerMask.GetMask("Ground"); 
        _isClimbing = false;
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
                _interactable.BeginInteract();
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
                                (Vector2)transform.position + _capsuleCollider.offset + Vector2.down * _capsuleCollider.size.y * 0.5f + Vector2.down * _param.GetParam().ClimbSpeed * Time.deltaTime
                           );  
        }
    }
#endif
// DrawGizmos

#endregion Unity Message
// End of Unity Message


}
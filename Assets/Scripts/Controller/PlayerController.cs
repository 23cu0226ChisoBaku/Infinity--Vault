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
public class PlayerController : MonoBehaviour,IParametrizable,IItemGetable
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
    private Rigidbody2D _rigidbody2D;

    private CapsuleCollider2D _capsuleCollider;
    
    private PhysicsMaterial2D _noFrictionMat;
    private IParam<CharaParam> _param = new CharaModel();

    private bool _isClimbable;

    private bool _isClimbing;

    private MoveDir _moveDir = MoveDir.None;

    private PlayerState _state = PlayerState.None;

    private Transform _interactTarget;

    // TODO
    private IInteractable _interactable = null;

    private RaycastHit2D[] _hitInfoBuffer = new RaycastHit2D[3];

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
        if (_state != PlayerState.Climb)
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

        if (_interactable != null)
        {
            if (Input.GetKeyDown(_interactable.GetTargetInfo().InteractKey))
            {
                _interactable.DoInteract();
            }
        }

        if (_isClimbable)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if(_state != PlayerState.Climb)
                {
                    _state = PlayerState.Climb;
                    var adjustPos = transform.position;
                    adjustPos.x = _interactTarget.position.x;

                    transform.position = adjustPos;

                    _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                    
                    _rigidbody2D.velocity = Vector2.zero;
                    _rigidbody2D.gravityScale = 0f;
                    _rigidbody2D.excludeLayers = LayerMask.GetMask("Ground");
                }
                transform.Translate(0f, _param.GetParam().ClimbSpeed * Time.deltaTime, 0f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if(_state != PlayerState.Climb)
                {
                    _state = PlayerState.Climb;
                    var adjustPos = transform.position;
                    adjustPos.x = _interactTarget.position.x;

                    transform.position = adjustPos;

                    _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                    _rigidbody2D.gravityScale = 0f;
                    _rigidbody2D.velocity = Vector2.zero;

                    _rigidbody2D.excludeLayers = LayerMask.GetMask("Ground");
                }
                transform.Translate(0f, -_param.GetParam().ClimbSpeed * Time.deltaTime, 0f);
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

        // はしごの付近にいるかをレイキャストで探す
        var rayLength = _capsuleCollider.size.y * 0.5f + _param.GetParam().ClimbSpeed * Time.deltaTime;
        var rayLayerMask = LayerMask.GetMask("Interactable");
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.NoFilter();
        filter2D.useLayerMask = true;
        filter2D.SetLayerMask(rayLayerMask);
        int hitCnt = _capsuleCollider.Raycast(Vector2.down, filter2D, _hitInfoBuffer, rayLength);

        if (hitCnt < 1)
        {
            if(_state == PlayerState.Climb)
            {
                _state = PlayerState.None;
                _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                _rigidbody2D.gravityScale = 10f;
                // Nothingにする
                _rigidbody2D.excludeLayers = 0;
            }
            _interactTarget = null;
            _isClimbable = false;
        }
        else
        {
            for (int i = 0; i < hitCnt; ++i)
            {
                _interactTarget = _hitInfoBuffer[i].collider.gameObject.transform;
                _isClimbable = true;
            }
        }

    }

    public void SetParam(CharaParam charaParam)
    {
        _param.SetParam(charaParam);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        // {
        //     _isClimbable = true;
        //     _interactTarget = other.gameObject.transform;
        // }

        if (_interactable == null)
        {
            if (other.gameObject.TryGetComponent(out _interactable))
            {
                _interactable.BeginInteract();
            }
        }

        if (other.gameObject.TryGetComponent(out IPickable pickable))
        {
            pickable.OnPick(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        // {
        //     _isClimbable = false;
        //     if(_state == PlayerState.Climb)
        //     {
        //         _state = PlayerState.None;
        //         _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        //         _rigidbody2D.gravityScale = 10f;
        //         // Nothingにする
        //         _rigidbody2D.excludeLayers = 0;
        //     }
        //     _interactTarget = null;
        // }

        if (other.gameObject.TryGetComponent(out _interactable))
        {
            _interactable.EndInteract();
            _interactable = null;
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
#endif
}
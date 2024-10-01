using UnityEngine;

public interface IParam<T>
{
    T GetParam();
}

public struct CharaParam
{
    public float MoveSpeed;
    public float ClimbSpeed;
    public string test;
}

public class Param<T> : IParam<T> where T : struct
{
    protected T param;
    public T GetParam() => param;

}

public class CharaModel : Param<CharaParam>
{
    public CharaModel(float moveSpeed, float climbSpeed)
    {
        param = new CharaParam 
        {
            MoveSpeed = moveSpeed,
            ClimbSpeed = climbSpeed
        };
    }
}

public class PlayerController : MonoBehaviour
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
    
    private PhysicsMaterial2D _noFrictionMat;

    public float _moveSpeed;
    public float _climbSpeed;

    private IParam<CharaParam> _param;

    private bool _isClimbable;

    private MoveDir _moveDir = MoveDir.None;

    private PlayerState _state = PlayerState.None;

    private Transform _interactTarget;

    private void Awake()
    {
        _param = new CharaModel(_moveSpeed, _climbSpeed);

        _isClimbable = false;

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _noFrictionMat = new PhysicsMaterial2D();
        _noFrictionMat.friction = 0f;
        _rigidbody2D.sharedMaterial = _noFrictionMat;
    }
    private void Update()
    {
        _moveDir = MoveDir.None;
        if (_state != PlayerState.Climb)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _moveDir = MoveDir.Left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _moveDir = MoveDir.Right;
            }
        }

        if (_isClimbable)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if(_state != PlayerState.Climb)
                {
                    _state = PlayerState.Climb;
                    var adjustPos = transform.position;
                    adjustPos.x = _interactTarget.position.x;

                    transform.position = adjustPos;

                    _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                    _rigidbody2D.velocity = Vector2.zero;

                    _rigidbody2D.excludeLayers = LayerMask.GetMask("Ground");
                }
                transform.Translate(0f, _param.GetParam().ClimbSpeed * Time.deltaTime, 0f);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if(_state != PlayerState.Climb)
                {
                    _state = PlayerState.Climb;
                    var adjustPos = transform.position;
                    adjustPos.x = _interactTarget.position.x;

                    transform.position = adjustPos;

                    _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                    _rigidbody2D.velocity = Vector2.zero;

                    _rigidbody2D.excludeLayers = LayerMask.GetMask("Ground");
                }
                transform.Translate(0f, -_param.GetParam().ClimbSpeed * Time.deltaTime, 0f);
            }
        }
    }

    private void FixedUpdate() 
    {
        _rigidbody2D.velocity = Vector2.right * (int)_moveDir * _param.GetParam().MoveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            _isClimbable = true;
            _interactTarget = other.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            _isClimbable = false;
            if(_state == PlayerState.Climb)
            {
                _state = PlayerState.None;
                _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                // Nothing‚É‚·‚é
                _rigidbody2D.excludeLayers = 0;
            }
            _interactTarget = null;
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

}
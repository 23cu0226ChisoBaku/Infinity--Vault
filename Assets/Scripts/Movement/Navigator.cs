using UnityEngine;

public interface INavigator
{
  public void InitNavigator(Transform userTransform);
  public void UpdateNavigating(float rate);
  public void StartNavigate(Vector2 nextDirection);
  public void StopNavigate();
  public bool IsReachTargetDestination {get;}
}
internal class Navigator : INavigator
{
  private bool _canNavigate = false;
  private Vector2 _destination;
  private Transform _navigatorUser;
  private bool _isNavigating;

  public void InitNavigator(Transform userTransform)
  {
    _navigatorUser = userTransform;
  }
  public virtual void UpdateNavigating(float rate)
  {
    if (_navigatorUser == null)
    {
      return;
    }

    // TODO test code
    if (!_canNavigate)
    {
      return;
    }

    // TODO need change
    Vector2 leftDisVec = (_destination - (Vector2)_navigatorUser.position);
    Vector2 moveDir = leftDisVec.normalized * rate * Time.deltaTime;
    moveDir = moveDir.sqrMagnitude > leftDisVec.sqrMagnitude ? leftDisVec : moveDir;

    _navigatorUser.Translate(moveDir);
  }
  public void StartNavigate(Vector2 nextDirection)
  {
    if (_navigatorUser == null)
    {
      return;
    }

    _destination = nextDirection;
    _canNavigate = true;
    _isNavigating = true;
  }

  public void StopNavigate()
  {
    _isNavigating = false;
  }
  public bool IsReachTargetDestination
  {
    get
    {
      if (_navigatorUser == null)
      {
#if UNITY_EDITOR
        Debug.LogWarning("Navigator is not initialized");
#endif
        return false;
      }

      if (_isNavigating)
      {
        return (Vector2)_navigatorUser.position == _destination;
      }
      else
      {
        return false;
      }
    }
  }
}

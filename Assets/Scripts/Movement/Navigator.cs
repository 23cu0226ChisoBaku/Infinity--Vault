using UnityEngine;

internal interface INavigator
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
    private readonly static float DISTANCE_EPSILON;
    private Vector2 _destination;
    private Transform _navigatorUser;
    private bool _isNavigating;

    static Navigator()
    {
        DISTANCE_EPSILON = 0.001f;
    }

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

        Vector2 moveDir = (_destination - (Vector2)_navigatorUser.position).normalized;
        _navigatorUser.Translate(moveDir * rate * Time.deltaTime);

    }
    public void StartNavigate(Vector2 nextDirection)
    {
        if (_navigatorUser == null)
        {
            return;
        }

        _destination = nextDirection;
        _canNavigate = true;
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
                var leftDistance = ((Vector2)_navigatorUser.position - _destination).sqrMagnitude;
                return leftDistance <= DISTANCE_EPSILON;
            }
            else
            {
                return false;
            }
        }
    }

}

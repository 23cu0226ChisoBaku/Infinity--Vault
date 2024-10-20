using UnityEngine;

/// <summary>
/// ナビゲーター設定できるオブジェクト
/// </summary>
internal interface ICanSetNavigator
{
    /// <summary>
    /// ナビゲーターを設定
    /// </summary>
    /// <param name="navigator">ナビゲーター</param>
    public void SetNavigator(INavigator navigator);
}

internal interface INavigatable
{
  public Transform GetNavigateTarget();
}

namespace IV
{
    namespace Enemy
    {
        internal class EnemyController : MonoBehaviour, ICanSetNavigator,INavigatable
        {
            // TODO
            private INavigator _navigator;

            public Vector2 TEST_DESTINATION;

            private float _speed = 3f;

            private void Update()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _navigator?.StartNavigate(TEST_DESTINATION);
                }
                _navigator?.UpdateNavigating(_speed);

                if((_navigator != null) && _navigator.IsReachTargetDestination)
                {
                    _navigator.StopNavigate();
                }
            }
            public void SetNavigator(INavigator navigator)
            {
                _navigator = navigator;
                _navigator.InitNavigator(transform);
            }

            public Transform GetNavigateTarget()
            {
                return transform;
            }
        }

    }
}
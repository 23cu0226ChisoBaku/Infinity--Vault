using UnityEngine;

/// <summary>
/// ナビゲーターつけられるオブジェクト
/// </summary>
internal interface INavigatable
{
    /// <summary>
    /// ナビゲーターをつける
    /// </summary>
    /// <param name="navigator">ナビゲーター</param>
    public void SetNavigator(INavigator navigator);
}

namespace IV
{
    namespace Enemy
    {
        /// <summary>
        /// TODO まだ使っていない
        /// </summary>
        internal class EnemyController : MonoBehaviour, INavigatable
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

        }

    }
}
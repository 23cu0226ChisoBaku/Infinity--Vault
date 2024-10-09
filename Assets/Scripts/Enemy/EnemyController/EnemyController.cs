using UnityEngine;

/// <summary>
/// �i�r�Q�[�V�����ňړ�����I�u�W�F�N�g
/// </summary>
internal interface INavigatable
{
    /// <summary>
    /// �i�r�Q�[�^�[��ݒ�
    /// </summary>
    /// <param name="navigator"></param>
    public void SetNavigator(INavigator navigator);
}

namespace IV
{
    namespace Enemy
    {
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
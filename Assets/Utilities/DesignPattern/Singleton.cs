using UnityEngine;
using UnityEngine.Assertions;

namespace MSingleton
{

    /// <summary>
    /// �V���O���g���iMonoBehaviour�ˑ��j
    /// </summary>
    /// <typeparam name="T">�W�F�l���b�N�N���X</typeparam>
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static volatile T _instance;
        private static readonly object padlock = new object();
        private static bool _isApplicationQuitting = false;
        public static T Instance
        {
            get
            {
                if (_isApplicationQuitting)
                {
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    // Scene�Ɍ�����Ȃ�������V�������
                    if (_instance == null)
                    {
                        lock(padlock)
                        {
                            GameObject singleton = new GameObject(typeof(T).Name);
                            _instance = singleton.AddComponent<T>();
                            DontDestroyOnLoad(_instance);
                        }
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnApplicationQuit() 
        {
            _isApplicationQuitting = true;
        }

        protected SingletonMono() {}
    }

    /// <summary>
    /// �V���O���g���iMonoBehaviour�ˑ����Ȃ��j
    /// </summary>
    /// <typeparam name="T">�W�F�l���b�N�N���X</typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        private volatile static T _instance;
        private static readonly object padlock = new object();
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // �����A�N�Z�X�h�~
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }

                return _instance;
            }
        }

        protected Singleton() 
        {
            Assert.IsNull(_instance);
        }
    }
}


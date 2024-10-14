using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

using System.Linq.Expressions;

namespace MStateMachine
{
    // ---------------------------------------
    // �X�e�[�g�}�V��(MonoBehaviour�ˑ�)
    // EState -> �񋓌^(enum)
    // ---------------------------------------
    public class MonoStateMachine<EState> : MonoBehaviour where EState : System.Enum
    {
        // �X�e�[�g������Dictionary(Key:EState(EnumToInt) / Value:BaseState<EState>)
        private Dictionary<int, State<EState>> _states 
            = new Dictionary<int, State<EState>>();

        // �X�e�[�g�؂�ւ��閽�߂�����L���[
        private Queue<EState> _queuedStates                       
            = new Queue<EState>();

        // ���݃X�e�[�g
        protected State<EState> _currentState;

        // �X�e�[�g��؂�ւ��Ă��邱�Ƃ�\���t���O
        protected bool _isTransitioningState = false;        

        protected void AddState(EState type, State<EState> state)
        {
            // var typeKey = Convert.ToInt32(type);
            var typeKey = type.ConvertToInt();
            if(!_states.ContainsKey(typeKey))
            {
                _states.Add(typeKey,state);
            }
        } 
        
        private void Start()
        {
            // �f�t�H���g�X�e�[�g�ɓ���
            _currentState?.EnterState();
        }

        private void Update()
        {
            // �X�e�[�g�X�V
            _currentState?.UpdateState(Time.deltaTime);

            // ���̃X�e�[�g�������Ă������Ԃ�؂�ւ���
            if (_queuedStates.Count != 0)
            {
                TransitionToState(_queuedStates.Dequeue());
            }
        }

        private void FixedUpdate()
        {
            _currentState?.FixedUpdateState(Time.fixedDeltaTime);
        }

        /// <summary>
        /// �X�e�[�g��؂�ւ���
        /// </summary>
        /// <param name="nextState">���̃X�e�[�g</param>
        private void TransitionToState(EState nextState)
        {
            if(_currentState != null)
            {
                // ���̃X�e�[�g�ƌ��݂̃X�e�[�g��������������I��
                if (_currentState.StateKey.Equals(nextState))   // nextState == _currentState
                    return;
            }

            // �X�e�[�g��؂�ւ���
            {
                _isTransitioningState = true;
                SwitchNextStateImpl(nextState);
                _isTransitioningState = false;

                // �t���[�����ɃX�e�[�g�̐؂�ւ���������񂵂��󂯕t���Ȃ�
                _queuedStates.Clear();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _currentState.OnCollisionEnter(collision);
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            _currentState.OnCollisionStay(collision);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            _currentState.OnCollisionExit(collision);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            _currentState.OnTriggerEnter(collision);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            _currentState.OnTriggerStay(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            _currentState.OnTriggerExit(collision);
        }

        /// <summary>
        /// ���̃X�e�[�g�ɐ؂�ւ���
        /// </summary>
        /// <param name="nextState"></param>
        public void SwitchNextState(EState nextState)
        {
            _queuedStates.Enqueue(nextState);
        }

        public void SwitchStateImmediately(EState nextState)
        {
            SwitchNextStateImpl(nextState);
        }

        private void SwitchNextStateImpl(EState nextState)
        {
            _currentState?.ExitState();
            var nextStateKey = nextState.ConvertToInt();
            if(_states.ContainsKey(nextStateKey))
            {
                _currentState = _states[nextStateKey];
            }
            else
            {
                Debug.LogError($"Can't find state {nextState}");
                _currentState = null;
            }
            _currentState?.EnterState();
        }

        /// <summary>
        /// ���݂̃X�e�[�g���擾����i�v���p�e�B�j
        /// </summary>
        public EState CurrentStateKey
        {
            get
            {
                if(_currentState == null)
                {
                    Debug.LogError("Current state is null!!!");
                    return default;
                }
                else
                {
                    return _currentState.StateKey;
                }

            }
        }
    }

    

}// end of namespace MStateMachine

public static class EnumConvertToIntExtension
{
    private static class StaticEnumConvertCache<TEnum> where TEnum : System.Enum
    {
        public static Func<TEnum,int> ConvertFunc = GenerateConvertFunc<TEnum>();
    }

    private static Func<TEnum,int> GenerateConvertFunc<TEnum>() where TEnum : System.Enum
    {
        var inputParameter = Expression.Parameter(typeof(TEnum));

        var body = Expression.Convert(inputParameter, typeof(int)); // means: (int)input;

        var lambda = Expression.Lambda<Func<TEnum, int>>(body, inputParameter);

        var func = lambda.Compile();

        return func;
    }

    public static int ConvertToInt<TEnum>(this TEnum enumVal) where TEnum : System.Enum
    {
        return StaticEnumConvertCache<TEnum>.ConvertFunc(enumVal);
    }
}



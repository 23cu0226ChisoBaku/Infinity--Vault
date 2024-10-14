using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

using System.Linq.Expressions;

namespace MStateMachine
{
    // ---------------------------------------
    // ステートマシン(MonoBehaviour依存)
    // EState -> 列挙型(enum)
    // ---------------------------------------
    public class MonoStateMachine<EState> : MonoBehaviour where EState : System.Enum
    {
        // ステートを入れるDictionary(Key:EState(EnumToInt) / Value:BaseState<EState>)
        private Dictionary<int, State<EState>> _states 
            = new Dictionary<int, State<EState>>();

        // ステート切り替える命令を入れるキュー
        private Queue<EState> _queuedStates                       
            = new Queue<EState>();

        // 現在ステート
        protected State<EState> _currentState;

        // ステートを切り替えていることを表すフラグ
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
            // デフォルトステートに入る
            _currentState?.EnterState();
        }

        private void Update()
        {
            // ステート更新
            _currentState?.UpdateState(Time.deltaTime);

            // 次のステートが入っていたら状態を切り替える
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
        /// ステートを切り替える
        /// </summary>
        /// <param name="nextState">次のステート</param>
        private void TransitionToState(EState nextState)
        {
            if(_currentState != null)
            {
                // 次のステートと現在のステートが同じだったら終了
                if (_currentState.StateKey.Equals(nextState))   // nextState == _currentState
                    return;
            }

            // ステートを切り替える
            {
                _isTransitioningState = true;
                SwitchNextStateImpl(nextState);
                _isTransitioningState = false;

                // フレーム毎にステートの切り替え請求を一回しか受け付けない
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
        /// 次のステートに切り替える
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
        /// 現在のステートを取得する（プロパティ）
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



using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace MStateMachine
{
    public interface ISwitchState<EState> where EState : System.Enum
    {
        void SwitchNextState(EState nextState);
    }
    public interface IStateMachine<EState> : ISwitchState<EState> where EState : System.Enum
    {
        void InitStateMachine(EState initState);
        void Update(float deltaTime);
        EState CurrentState{get;}
    }

    public interface IUnityPhysicsBaseStateMachine<EState> : IStateMachine<EState> where EState : System.Enum
    {
        void FixedUpdate(float fixedDeltaTime);
    }
    // ---------------------------------------
    // �X�e�[�g�}�V��
    // EState -> �񋓌^(enum)
    // ---------------------------------------
    public abstract class StateMachine<EState> : IStateMachine<EState> where EState : System.Enum 
    {
        // �X�e�[�g������Dictionary(Key:EState(EnumToInt) / Value:BaseState<EState>)
        private Dictionary<int, State<EState>> _states;

        // �X�e�[�g�؂�ւ��閽�߂�����L���[
        private Queue<EState> _queuedNextStates;

        // ���݃X�e�[�g
        protected State<EState> _currentState;

        public StateMachine()
        {
            _states = new Dictionary<int, State<EState>>();
            _queuedNextStates = new Queue<EState>();
            _currentState = null;
        }

        /// <summary>
        /// ���݂̃X�e�[�g���擾����i�v���p�e�B�j
        /// </summary>
        EState IStateMachine<EState>.CurrentState
        {
            get
            {
                if(_currentState == null)
                {
#if UNITY_EDITOR
                UnityEngine.Assertions.Assert.IsNotNull(_currentState,$"Current state is null");
#else
                System.Diagnostics.Debug.Assert(_currentState != null,$"Current state is null");
#endif
                    return default;
                }
                else
                {
                    return _currentState.StateKey;
                }

            }
        }        
        void IStateMachine<EState>.InitStateMachine(EState initState)
        {
            var stateKey = initState.ConvertToInt();

            if (_states.TryGetValue(stateKey, out State<EState> state))
            {
                _currentState = state;
                _currentState?.EnterState();
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Assertions.Assert.IsNotNull(null,$"State {initState.ToString()} has not register");
#else
                System.Diagnostics.Debug.Assert(state != null,$"State {initState.ToString()} has not register");
#endif
            }
        }
       
        void IStateMachine<EState>.Update(float deltaTime)
        {
            // ���̃X�e�[�g�������Ă������Ԃ�؂�ւ���
            if (_queuedNextStates.Count != 0)
            {
                TransitionToState(_queuedNextStates.Dequeue());
            }
            
            // �X�e�[�g�X�V
            _currentState?.UpdateState(deltaTime);

        }

        /// <summary>
        /// ���̃X�e�[�g�ɐ؂�ւ���
        /// </summary>
        /// <param name="nextState"></param>
        void ISwitchState<EState>.SwitchNextState(EState nextState)
        {
            _queuedNextStates.Enqueue(nextState);
        }

        protected void AddState(EState type, State<EState> state)
        {
            if (state == null)
            {
#if UNITY_EDITOR
                UnityEngine.Assertions.Assert.IsNotNull(null,$"State {state.GetType().Name} is null");
#else
                System.Diagnostics.Debug.Assert(state != null,$"State {state.GetType().Name} is null");
#endif
                return;
            }
            else
            {
                var typeKey = type.ConvertToInt();
                if(_states.ContainsKey(typeKey))
                {
#if UNITY_EDITOR
                    UnityEngine.Assertions.Assert.IsNotNull(null,$"State {state.GetType().Name} is already exist");
#else
                    System.Diagnostics.Debug.Assert(state != null,$"State {state.GetType().Name} is already exist");
#endif
                    return;
                }
                else
                {
                    _states.Add(typeKey,state);
                }
            }
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
                SwitchNextStateImpl(nextState);
                // �t���[�����ɃX�e�[�g�̐؂�ւ���������񂵂��󂯕t���Ȃ�
                _queuedNextStates.Clear();
            }
        }

        private void SwitchNextStateImpl(EState nextState)
        {
            _currentState?.ExitState();
            var nextStateKey = nextState.ConvertToInt();

            if(_states.ContainsKey(nextStateKey))
            {
                _currentState = _states[nextStateKey];
            }

            _currentState?.EnterState();
        }
    }

} 
    
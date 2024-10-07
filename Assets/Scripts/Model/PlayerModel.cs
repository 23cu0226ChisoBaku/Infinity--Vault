using System;
using UnityEngine;
using MDesingPattern.MMediator;

public class PlayerModel : MonoBehaviour,IMediatable<PlayerModel,IPlayerUIInfo>,IPlayerUIInfo
{
    private static CharaParam _charaParam = new CharaParam();
    private IParametrizable _playerParamContainer;

    private IMediator<PlayerModel,IPlayerUIInfo> _uiMediator;

    [SerializeProperty("MoveSpeed")]
    public float MoveSpeed_;

    [SerializeProperty("ClimbSpeed")]
    public float ClimbSpeed_;

    private event Action<float> _moveSpeedChangeEvent;
    private event Action<float> _climbSpeedChangeEvent;

    public float MoveSpeed 
    {
        get
        {
            return _charaParam.MoveSpeed;
        }
        private set
        {
            float old = _charaParam.MoveSpeed;
            _charaParam.MoveSpeed = value;
            MoveSpeed_ = value;
            OnChangeMoveSpeedValue(old);
        }
    }
    public float ClimbSpeed
    {
        get
        {
            return _charaParam.ClimbSpeed;
        }
        private set
        {
            float old = _charaParam.ClimbSpeed;
            _charaParam.ClimbSpeed = value;
            ClimbSpeed_ = value;
            OnChangeClimbSpeedValue(old);
        }
    }

    public int Wealth
    {
        get
        {
            return _charaParam.Wealth;
        }
        private set
        {
            int old = _charaParam.Wealth;
            _charaParam.Wealth = value;
            OnChangeWealthValue(old);
        }
    }

    private void Awake()
    {   
        if (TryGetComponent(out _playerParamContainer))
        {
            _charaParam.MoveSpeed = MoveSpeed_;
            _charaParam.ClimbSpeed = ClimbSpeed_;
            _charaParam.Wealth = 0;
            _playerParamContainer.SetParameter(_charaParam);
        }
        else
        {
            throw new System.Exception("Can not get player controller");
        }

        _uiMediator = FindAnyObjectByType<PlayerUIController>();
    }
    private void OnChangeMoveSpeedValue(float old)
    {
#if UNITY_EDITOR
        Debug.Log($"Old Value: {old}");
        Debug.Log($"New Value: {_charaParam.MoveSpeed}");
#endif
        if (MLib.MLib.IsEqual(old, _charaParam.MoveSpeed))
        {
            Debug.LogWarning("Equals");
            return;
        }

        // TODO
        _playerParamContainer?.SetParameter(_charaParam);
        _moveSpeedChangeEvent?.Invoke(_charaParam.MoveSpeed);
    }

    private void OnChangeClimbSpeedValue(float old)
    {
#if UNITY_EDITOR
        Debug.Log($"Old Value: {old}");
        Debug.Log($"New Value: {_charaParam.ClimbSpeed}");
#endif
        if (MLib.MLib.IsEqual(old, _charaParam.ClimbSpeed))
        {
            return;
        }

        // TODO
        _playerParamContainer?.SetParameter(_charaParam);
        _climbSpeedChangeEvent?.Invoke(_charaParam.ClimbSpeed);
    }

    private void OnChangeWealthValue(int old)
    {
#if UNITY_EDITOR
        Debug.Log($"Old Value: {old}");
        Debug.Log($"New Value: {_charaParam.Wealth}");
#endif
        if(MLib.MLib.IsEqual(old,_charaParam.Wealth))
        {
            return;
        }

        if (_uiMediator.IsAlive())
        {
            _uiMediator.Notify(this,this);
        }

    }

    void IMediatable<PlayerModel, IPlayerUIInfo>.SetMediator(IMediator<PlayerModel, IPlayerUIInfo> mediator)
    {
        _uiMediator = mediator;
    }

    // TODO test code
    public void AddWealth(int wealth)
    {
        Wealth += wealth;
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;

public class PlayerModel : MonoBehaviour
{
    private CharaParam _charaParam = new CharaParam();
    private IParametrizable _playerParamContainer;

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
            return MoveSpeed_;
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
            return ClimbSpeed_;
        }
        private set
        {
            float old = _charaParam.ClimbSpeed;
            _charaParam.ClimbSpeed = value;
            ClimbSpeed_ = value;
            OnChangeClimbSpeedValue(old);
        }
    }

    private void Awake()
    {   
        if (TryGetComponent(out _playerParamContainer))
        {
            _charaParam.MoveSpeed = MoveSpeed_;
            _charaParam.ClimbSpeed = ClimbSpeed_;
            _playerParamContainer.SetParameter(_charaParam);
        }
        else
        {
            throw new System.Exception("Can not get player controller");
        }
    }
    private void OnChangeMoveSpeedValue(float old)
    {
        Debug.Log($"Old Value: {old}");
        Debug.Log($"New Value: {_charaParam.MoveSpeed}");

        if (MEqual.IsEqual(old, _charaParam.MoveSpeed))
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
        Debug.Log($"Old Value: {old}");
        Debug.Log($"New Value: {_charaParam.ClimbSpeed}");

        if (MEqual.IsEqual(old, _charaParam.ClimbSpeed))
        {
            return;
        }

        // TODO
        _playerParamContainer?.SetParameter(_charaParam);
        _climbSpeedChangeEvent?.Invoke(_charaParam.ClimbSpeed);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSetter : MonoBehaviour,ISerializationCallbackReceiver
{
    private CharaParam _charaParam;
    private IParametrizable _playerParamContainer;

    [SerializeField]
    private bool _isConnectedToController = false;

    [SerializeProperty("MoveSpeed")]
    public float MoveSpeed_;

    [SerializeProperty("ClimbSpeed")]
    public float ClimbSpeed_;

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
            OnChangeClimbSpeedValue(old);
        }
    }

    private void OnChangeMoveSpeedValue(float old)
    {
        Debug.Log($"Old Value: {old}");
        Debug.Log($"New Value: {_charaParam.MoveSpeed}");

        if (_isConnectedToController)
        {
            _playerParamContainer.SetParameter(_charaParam);
        }
    }

    private void OnChangeClimbSpeedValue(float old)
    {
        Debug.Log($"Old Value: {old}");
        Debug.Log($"New Value: {_charaParam.ClimbSpeed}");

        if (_isConnectedToController)
        {
            _playerParamContainer.SetParameter(_charaParam);
        }
    }

    public void OnBeforeSerialize()
    {
        if (_playerParamContainer == null)
        {
            if (TryGetComponent(out _playerParamContainer))
            {
                _isConnectedToController = true;
            }
        }
    }

    public void OnAfterDeserialize()
    {
        if (_playerParamContainer != null)
        {
            _playerParamContainer.SetParameter(_charaParam);
        }
    }
}

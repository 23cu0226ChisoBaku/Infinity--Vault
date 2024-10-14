using System;
using UnityEngine;
using MDesingPattern.MMediator;

public class PlayerModelContainer : MonoBehaviour,IMediatable<PlayerModelContainer,IPlayerUIMessage>
{
    public readonly static IPlayerModel PLAYER_MODEL = new PlayerModel();
    private IMediator<PlayerModelContainer,IPlayerUIMessage> _uiMediator;

#region Inspector Property Settings
    [SerializeProperty("MoveSpeed")]
    [SerializeField]
    private float MoveSpeed_;

    [SerializeProperty("ClimbSpeed")]
    [SerializeField]
    private float ClimbSpeed_;
#endregion Inspector Property Settings
// End of Inspector Property Settings

    public float MoveSpeed 
    {
        get
        {
            return PLAYER_MODEL.MoveSpeed;
        }
        private set
        {
            PLAYER_MODEL.MoveSpeed = value;
            MoveSpeed_ = value;
        }
    }
    public float ClimbSpeed
    {
        get
        {
            return PLAYER_MODEL.ClimbSpeed;
        }
        private set
        {
            PLAYER_MODEL.ClimbSpeed = value;
            ClimbSpeed_ = value;
        }
    }

    private void Start() 
    {
        _uiMediator = FindAnyObjectByType<PlayerUIController>();

        PLAYER_MODEL.MoveSpeed = MoveSpeed_;
        PLAYER_MODEL.ClimbSpeed = ClimbSpeed_;
        PLAYER_MODEL.Wealth = 0;

    }

    void IMediatable<PlayerModelContainer, IPlayerUIMessage>.SetMediator(IMediator<PlayerModelContainer, IPlayerUIMessage> mediator)
    {
        _uiMediator = mediator;
    }

    // TODO test code
    public void AddWealth(int wealth)
    {
        PLAYER_MODEL.Wealth += wealth;
    }

    private void OnEnable() 
    {
        PLAYER_MODEL.OnChangeMoveSpeedValue += OnChangeMoveSpeedValue;
        PLAYER_MODEL.OnChangeClimbSpeedValue += OnChangeClimbSpeedValue;
        PLAYER_MODEL.OnChangeWealthValue += OnChangeWealthValue;

    }

    private void OnDisable() 
    {
        PLAYER_MODEL.OnChangeMoveSpeedValue -= OnChangeMoveSpeedValue;
        PLAYER_MODEL.OnChangeClimbSpeedValue -= OnChangeClimbSpeedValue;
        PLAYER_MODEL.OnChangeWealthValue -= OnChangeWealthValue;
    }

#region On Change Model Message
    private void OnChangeMoveSpeedValue(float oldValue,float newValue)
    {
#if UNITY_EDITOR
        Debug.Log($"Old Value: {oldValue}");
        Debug.Log($"New Value: {newValue}");
#endif
        if (MLibrary.MLib.IsEqual(oldValue, newValue))
        {
            return;
        }
    }

    private void OnChangeClimbSpeedValue(float oldValue,float newValue)
    {
#if UNITY_EDITOR
        Debug.Log($"Old Value: {oldValue}");
        Debug.Log($"New Value: {newValue}");
#endif
        if (MLibrary.MLib.IsEqual(oldValue, newValue))
        {
            return;
        }

    }

    private void OnChangeWealthValue(int oldValue,int newValue)
    {
#if UNITY_EDITOR
        Debug.Log($"Old Value: {oldValue}");
        Debug.Log($"New Value: {newValue}");
#endif
        if(MLibrary.MLib.IsEqual(oldValue,newValue))
        {
            return;
        }

        if (_uiMediator.IsAlive())
        {
            _uiMediator.Notify(this,PLAYER_MODEL as IPlayerUIMessage);
        }

    }
#endregion On Change Model Message
// End of On Change Model Message
}


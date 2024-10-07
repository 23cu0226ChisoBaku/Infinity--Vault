using UnityEngine;
using MDesingPattern.MMediator;
using TMPro;

public class PlayerUIController :MonoBehaviour, IMediator<PlayerModel, IPlayerUIInfo>
{
    public TMP_Text _wealthPanel;
    void IMediator<PlayerModel, IPlayerUIInfo>.Notify(PlayerModel sender, IPlayerUIInfo message)
    {
        if (!message.IsAlive())
        {
            return;
        }
        if (_wealthPanel.IsActive())
        {
            _wealthPanel.text = message.Wealth.ToString();
        }
    }
}

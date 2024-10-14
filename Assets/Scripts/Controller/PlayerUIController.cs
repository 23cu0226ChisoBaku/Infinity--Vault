using UnityEngine;
using MDesingPattern.MMediator;
using TMPro;
using UnityEditor.Rendering;


public class PlayerUIController :MonoBehaviour, IMediator<PlayerModelContainer, IPlayerUIMessage>
{
    public TMP_Text _wealthPanel;

    void IMediator<PlayerModelContainer, IPlayerUIMessage>.Notify(PlayerModelContainer sender, IPlayerUIMessage message)
    {
        if (!message.IsAlive())
        {
            return;
        }
        if (_wealthPanel.IsActive())
        {
            _wealthPanel.text = message.GetWealth().ToString();
        }
    }

}

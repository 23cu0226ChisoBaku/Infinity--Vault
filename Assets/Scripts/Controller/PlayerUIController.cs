using UnityEngine;
using MDesingPattern.MMediator;
using TMPro;
using UnityEditor.Rendering;

/// <summary>
/// TODO　適当で作ったやつ
/// 後で直す予定
/// </summary>
/// 

public class PlayerUIController :MonoBehaviour, IMediator<PlayerModelContainer, IPlayerUIMessage>
{
  public TMP_Text _wealthText;

  void IMediator<PlayerModelContainer, IPlayerUIMessage>.Notify(PlayerModelContainer sender, 
                                                                   IPlayerUIMessage message)
  {
    // ヌルチェック
    if (!message.IsAlive())
    {
      return;
    }
    
    if (_wealthText != null)
    {
      _wealthText.text = message.GetWealth().ToString();
    }
  }

}

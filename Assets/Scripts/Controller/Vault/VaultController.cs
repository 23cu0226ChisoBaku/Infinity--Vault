using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

/// <summary>
/// 金庫を初期化するインターフェース
/// </summary>
internal class VaultController : InteractableObj ,IVault
{
// TODO リファクタリングする予定部分
#region Need Refactoring
  /// <summary>
  /// メッセージボックスのパス
  /// </summary>
  private readonly static string RESOURCES_MESSAGE_BOX_PATH = "Prefabs/GlobalMessageBox";
  /// <summary>
  /// 生成したメッセージボックスオブジェクト
  /// </summary>
  private GameObject _messageBox;
  /// <summary>
  /// メッセージボックスのテキスト部分
  /// </summary>
  private TMP_Text _infoMessageUI;
#endregion Need Refactoring
// End of リファクタリングする予定部分

// プライベートフィールド
#region Private Field
  private VaultInfo _vaultInfo;           // 金庫の情報
  private IPuzzleController _puzzleCtrl;  // 金庫のパズルコントローラー 
#endregion Private Field
// End of プライベートフィールド

// Unityメインループメッセージ
#region Unity Main Loop Message
  private void Awake()
  {
    _info = new InteractTargetInfo
    {
        Name = "Vault",
        InteractKey = KeyCode.E,
        Layer = gameObject.layer,
    };
    _puzzleCtrl = null;
  }
  private void OnDestroy() 
  {

    if (_messageBox.IsAlive())
    {
      Destroy(_messageBox);
      Resources.UnloadUnusedAssets();
    }
  }
#endregion Unity Main Loop Message
// End of Unityメインループメッセージ
    
#region Interface
  // IInteractable実装部分
#region IInteractable
  public override void ActiveInteract()
  {
    // TODO リファクタリングする予定
    #region Need Refactoring
    // メッセージボックスを作成
    if (_infoMessageUI == null)
    {
      // プレハブを取得
      var msgBoxPrefab = Resources.Load<GameObject>(RESOURCES_MESSAGE_BOX_PATH);
      if (msgBoxPrefab != null)
      {
        _messageBox = Instantiate(msgBoxPrefab, transform.position, Quaternion.identity);

        _infoMessageUI = _messageBox.GetComponent<TMP_Text>();
        
        // メッセージボックスのテキストを設定
        _infoMessageUI.text = $"Press {_info.InteractKey.ToString()}";

        var interactUICanvasList = FindObjectsByType<Canvas>(FindObjectsSortMode.None);

        bool isFound = false;
        foreach(var canvas in interactUICanvasList)
        {
          // 特定のキャンパスに入れる
          if (canvas.gameObject.layer == LayerMask.NameToLayer("InteractUI"))
          {   
              _messageBox.transform.SetParent(canvas.transform);
              isFound = true;
              break;
          }
        }

        if (!isFound)
        {
            throw new System.Exception("Fix it Immediatly");
        }
      }
    }
    _messageBox.SetActive(true);
    #endregion Need Refactoring
    // End of TODO リファクタリングする予定
  }
  public override void DoInteract()
  {
    // パズルをアクティブにする
    _puzzleCtrl?.Active();
  
    // メッセージボックスを隠す
    if (_messageBox.IsAlive())
    {
      _messageBox.SetActive(false);
    }
  }
  public override void EndInteract()
  {
    if (_puzzleCtrl.IsActive)
    {
      //  パズルを非アクティブにする
      _puzzleCtrl?.Deactive();
    }
    
    // メッセージボックスを隠す
    if (_messageBox.IsAlive())
    {
      _messageBox.SetActive(false);
    }
  }
  #endregion IInteractable
  // End of IInteractable実装部分

  // IVault実装部分
  #region IVault
  void IVault.InitVault(VaultInfo vaultInfo)
  {
    _vaultInfo = vaultInfo;
    InitVaultImpl();
  }
  #endregion IVault
  // End of IVault実装部分
#endregion Interface
// End of Interface

  /// <summary>
  /// 金庫の初期化の内部実装部分
  /// </summary>
  private void InitVaultImpl()
  {
    if (_puzzleCtrl == null)
    {
      // パズルを初期化
      _puzzleCtrl = new PuzzleController();
      _puzzleCtrl.InitPuzzle(_vaultInfo.Difficulty);
      _puzzleCtrl.OnClear += OpenVault;
      _puzzleCtrl.Deactive();
    }
  }
  /// <summary>
  /// 金庫を開く
  /// TODO リファクタリングする予定
  /// </summary>
  private void OpenVault()
  {
    // TODO Need Refactoring
    // アイテムを生成する
    var items = ItemGenerator.Instance.GenerateItems("Ruby", _vaultInfo.ItemCount);
    for(int i = 0; i < _vaultInfo.ItemCount; ++i)
    {
      var item = items[i];
      if (item == null)
      {
        continue;
      }

      item.GetComponent<Renderer>().sortingOrder = i;
      item.transform.position = transform.position;
      Rigidbody2D itemRigidbody = item.GetComponent<Rigidbody2D>();

      // アイテムを出す
      if (itemRigidbody != null)
      {
        itemRigidbody.excludeLayers |= LayerMask.GetMask("Pickable");
        var popAngel = Random.Range(30f,150f);
        var popPower = Random.Range(3,8);
        Vector2 popDirection = Quaternion.Euler(0,0,popAngel) * Vector2.right;
        itemRigidbody.AddForce(popDirection * popPower, ForceMode2D.Impulse);
      }
    }
    _puzzleCtrl?.TermPuzzle();
    
    Destroy(gameObject);
  }
}
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public interface IVault
{
  public void InitVault(VaultInfo vaultInfo);
}
internal class VaultController : InteractableObj ,IVault
{
  private readonly static string RESOURCES_MESSAGE_BOX_PATH = "Prefabs/GlobalMessageBox";
  private GameObject _messageBox;
  private TMP_Text _infoMessageUI;
  private VaultInfo _vaultInfo;

  private IPuzzle _puzzle;

  private IPuzzleController _puzzleCtrl;

  private void Awake()
  {
    _info = new InteractTargetInfo
    {
        Name = "Vault",
        InteractKey = KeyCode.E,
        Layer = gameObject.layer,
    };
    // TODO
    _puzzle = null;
    _puzzleCtrl = null;
  }
    
  public override void ActiveInteract()
  {
    if (_infoMessageUI == null)
    {
      var msgBoxPrefab = Resources.Load<GameObject>(RESOURCES_MESSAGE_BOX_PATH);
      if (msgBoxPrefab != null)
      {
        _messageBox = Instantiate(msgBoxPrefab, transform.position, Quaternion.identity);

        _infoMessageUI = _messageBox.GetComponent<TMP_Text>();
        
        _infoMessageUI.text = $"Press {_info.InteractKey.ToString()}";
        _messageBox.SetActive(false);

        var interactUICanvasList = FindObjectsByType<Canvas>(FindObjectsSortMode.None);

        bool isFound = false;
        foreach(var canvas in interactUICanvasList)
        {
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
  }

  public override void DoInteract()
  {
    _puzzleCtrl?.Active();
  
    // ���b�Z�[�W���B���Ă���
    _messageBox.SetActive(false);
  }

  public override void EndInteract()
  {

    _puzzle?.ResetPuzzle();
    _puzzleCtrl?.Deactive();
    
    if (_messageBox.IsAlive())
    {
        _messageBox.SetActive(false);
    }


  }

  private void OnDestroy() 
  {
    if (_messageBox != null)
    {
      Destroy(_messageBox);
    }
  }

  public void InitVault(VaultInfo vaultInfo)
  {
    _vaultInfo = vaultInfo;
    InitVaultImpl();
  }

  private void InitVaultImpl()
  {
    if (_puzzleCtrl == null)
    {
      _puzzleCtrl = new PuzzleController();
      _puzzleCtrl.InitPuzzle(_vaultInfo.Difficulty);
      _puzzleCtrl.OnClear += OpenVault;
      _puzzleCtrl.Deactive();
    }
  }

  private void OpenVault()
  {
    // �A�C�e������
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

      if (itemRigidbody != null)
      {
        itemRigidbody.excludeLayers |= LayerMask.GetMask("Pickable");
        var popAngel = Random.Range(30f,150f);
        var popPower = Random.Range(3,8);
        Vector2 popDirection = Quaternion.Euler(0,0,popAngel) * Vector2.right;
        itemRigidbody.AddForce(popDirection * popPower, ForceMode2D.Impulse);
      }
    }
    Destroy(gameObject);
  }
}
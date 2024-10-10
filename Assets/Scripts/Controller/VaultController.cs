using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VaultController : InteractableObj , IItemSettable
{
    private readonly static string RESOURCES_MESSAGE_BOX_PATH = "Prefabs/GlobalMessageBox";
    private GameObject _messageBox;
    private TMP_Text _infoMessageUI;
    // TODO
    public GameObject PuzzlePrefab;
    private GameObject _puzzle;
    private bool _isOpened;

    private ItemInfo _item;

    private void Awake()
    {
        _info = new InteractTargetInfo
        {
            Name = "Vault",
            InteractKey = KeyCode.E,
            Layer = gameObject.layer,
        };

        _isOpened = false;
    }
    
    public override void BeginInteract()
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
        if (PuzzlePrefab != null)
        {
            _puzzle = Instantiate(PuzzlePrefab);

            var puzzleCtrl = _puzzle.GetComponent<VaultPuzzleController>();

            if (puzzleCtrl != null)
            {
                puzzleCtrl.RegisterFinishEvent(OnPuzzleFinish);
            }
        }

        // メッセージを隠しておく
        _messageBox.SetActive(false);
    }

    public override void EndInteract()
    {
        if (_puzzle != null)
        {
            Destroy(_puzzle);
        }
        _messageBox.SetActive(false);

        if (_isOpened)
        {
            // アイテム生成
            var items = ItemGenerator.Instance.GenerateItems("Ruby", 50);
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

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

    private void OnPuzzleFinish()
    {
        _puzzle.GetComponent<VaultPuzzleController>().UnregisterFinishEvent(OnPuzzleFinish);
        _isOpened = true;
#if UNITY_EDITOR
        Debug.LogWarning($"Drop Item: {_item.Name}");
#endif
        EndInteract();
    }
    private void OnDestroy() 
    {
        if (_messageBox != null)
        {
            Destroy(_messageBox);
        }
    }

    public void SetItem(ItemInfo item)
    {
        _item = item;
    }
}
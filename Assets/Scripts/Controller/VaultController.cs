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

    private Item _item;

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
            Destroy(gameObject);
        }
    }

    private void OnPuzzleFinish()
    {
        _puzzle.GetComponent<VaultPuzzleController>().UnregisterFinishEvent(OnPuzzleFinish);
        _isOpened = true;
#if UNITY_EDITOR
        Debug.LogWarning($"Drop Item: {_item.name}");
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

    public void SetItem(Item item)
    {
        _item = item;
    }
}
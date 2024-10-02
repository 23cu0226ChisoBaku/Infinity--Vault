using System.Linq;
using TMPro;
using UnityEngine;

public class VaultController : InteractableObj
{
    private readonly static string RESOURCES_MESSAGE_BOX_PATH = "Prefabs/GlobalMessageBox";
    private GameObject _messageBox;
    private TMP_Text _infoMessageUI;
    private void Awake()
    {
        _info = new InteractTargetInfo
        {
            Name = "Vault",
            InteractKey = KeyCode.E
        };
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
        Destroy(gameObject);
    }

    public override void EndInteract()
    {
        _messageBox.SetActive(false);
    }

    private void OnDestroy() 
    {
        if (_messageBox != null)
        {
            Destroy(_messageBox);
        }
    }

}
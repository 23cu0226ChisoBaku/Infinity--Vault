using UnityEngine;

public struct InteractTargetInfo
{
    public string Name;
    public KeyCode InteractKey;
    public LayerMask Layer;
}
internal interface IInteractable
{
    InteractTargetInfo GetTargetInfo();
    void BeginInteract();
    void DoInteract();
    void EndInteract();
}

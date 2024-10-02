using UnityEngine;

public struct InteractTargetInfo
{
    public string Name;
    public KeyCode InteractKey;
}
internal interface IInteractable
{
    InteractTargetInfo GetTargetInfo();
    void BeginInteract();
    void DoInteract();
    void EndInteract();
}
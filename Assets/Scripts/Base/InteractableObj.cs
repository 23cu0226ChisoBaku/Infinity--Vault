using UnityEngine;

public abstract class InteractableObj : MonoBehaviour, IInteractable
{
    protected InteractTargetInfo _info;
    public abstract void BeginInteract();
    public abstract void DoInteract();
    public abstract void EndInteract();
    public InteractTargetInfo GetTargetInfo() => _info;

}
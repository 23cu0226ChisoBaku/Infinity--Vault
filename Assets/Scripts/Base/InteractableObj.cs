using UnityEngine;

public abstract class InteractableObj : MonoBehaviour, IInteractable
{
    /// <summary>
    /// �����̏��
    /// </summary>
    protected InteractTargetInfo _info;
    public abstract void ActiveInteract();
    public abstract void DoInteract();
    public abstract void EndInteract();
    public InteractTargetInfo GetTargetInfo() => _info;

}
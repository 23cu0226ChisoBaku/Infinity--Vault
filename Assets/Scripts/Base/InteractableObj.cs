using UnityEngine;

public abstract class InteractableObj : MonoBehaviour, IInteractable
{
  // 操作できるオブジェクト
  protected InteractTargetInfo _info;
  InteractTargetInfo IInteractable.GetTargetInfo() => _info;
  public abstract void ActiveInteract();
  public abstract void DoInteract();
  public abstract void EndInteract();

}
using UnityEngine;

public struct InteractTargetInfo
{
    public string Name;
    public KeyCode InteractKey;
    public LayerMask Layer;
}
internal interface IInteractable
{
    /// <summary>
    /// 操作先の情報を取得する
    /// </summary>
    /// <returns></returns>
    InteractTargetInfo GetTargetInfo();
    /// <summary>
    /// 操作ができるようになったら呼び出す
    /// </summary>
    void ActiveInteract();
    /// <summary>
    /// 操作をする
    /// </summary>
    void DoInteract();
    /// <summary>
    /// 操作を終了する
    /// </summary>
    void EndInteract();
}

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
    /// �����̏����擾����
    /// </summary>
    /// <returns></returns>
    InteractTargetInfo GetTargetInfo();
    /// <summary>
    /// ���삪�ł���悤�ɂȂ�����Ăяo��
    /// </summary>
    void ActiveInteract();
    /// <summary>
    /// ���������
    /// </summary>
    void DoInteract();
    /// <summary>
    /// ������I������
    /// </summary>
    void EndInteract();
}

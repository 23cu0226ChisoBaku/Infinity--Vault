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
    /// ‘€ìæ‚Ìî•ñ‚ğæ“¾‚·‚é
    /// </summary>
    /// <returns></returns>
    InteractTargetInfo GetTargetInfo();
    /// <summary>
    /// ‘€ì‚ª‚Å‚«‚é‚æ‚¤‚É‚È‚Á‚½‚çŒÄ‚Ño‚·
    /// </summary>
    void ActiveInteract();
    /// <summary>
    /// ‘€ì‚ğ‚·‚é
    /// </summary>
    void DoInteract();
    /// <summary>
    /// ‘€ì‚ğI—¹‚·‚é
    /// </summary>
    void EndInteract();
}

public interface IPuzzlePanel
{
  float PanelWidth{get;}
  float PanelHeight{get;}
  UnityEngine.Vector2 PanelCenterPosition {get;}
  public void ShowPanel();
  public void HidePanel();
  public void DisposePanel();

}
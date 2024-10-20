using UnityEngine;

public class PuzzlePanle : MonoBehaviour, IPuzzlePanel
{
  private SpriteRenderer _renderer;
  private float _panelWidth;
  private float _panelHeight;
  float IPuzzlePanel.PanelWidth => _panelWidth;

  float IPuzzlePanel.PanelHeight => _panelHeight;

  Vector2 IPuzzlePanel.PanelCenterPosition
  {
    get
    {
      return transform.position;
    }
    set
    {
      transform.position = value;
    }
  }

  void IPuzzlePanel.DisposePanel()
  {
    Destroy(gameObject);
  }

  void IPuzzlePanel.HidePanel()
  {
    gameObject.SetActive(false);
  }

  void IPuzzlePanel.ShowPanel()
  {
    ResetPanelPos();
    gameObject.SetActive(true);
  }

  private void Awake()
  {
    if (TryGetComponent(out _renderer))
    {
      _panelWidth = _renderer.bounds.size.x;
      _panelHeight = _renderer.bounds.size.y;
  
    }
  }

  private void ResetPanelPos()
  {
    transform.position = (Vector2)Camera.main.transform.position;
  }
}
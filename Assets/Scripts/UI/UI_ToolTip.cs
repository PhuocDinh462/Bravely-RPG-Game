using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
  [SerializeField] private float xLimit = 960;
  [SerializeField] private float yLimit = 540;

  [SerializeField] private float xOffset = 150;
  [SerializeField] private float yOffset = 150;

  public virtual void AdjustPosition()
  {
    Vector2 mousePosition = Input.mousePosition;

    float newXOffset = (mousePosition.x > xLimit ? -1 : 1) * xOffset;
    float newYOffset = (mousePosition.y > yLimit ? -1 : 1) * yOffset;

    transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
  }

  public void AdjustFontSize(TextMeshProUGUI _text)
  {
    if (_text.text.Length > 12)
      _text.fontSize *= .8f;
  }
}

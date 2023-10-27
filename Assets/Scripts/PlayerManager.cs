using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public static PlayerManager instance;
  public Player player;

  private void Awake()
  {
    if (instance)
      Destroy(instance.gameObject);
    else
      instance = this;
  }
}

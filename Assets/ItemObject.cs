using UnityEngine;

public class ItemObject : MonoBehaviour
{
  private SpriteRenderer sr;

  [SerializeField] private ItemData itemData;

  private void Start()
  {
    sr = GetComponent<SpriteRenderer>();

    sr.sprite = itemData.icon;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.GetComponent<Player>())
    {
      Inventory.instance.addItem(itemData);
      Destroy(gameObject);
    }
  }
}

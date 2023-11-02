using UnityEngine;

public class ItemObject : MonoBehaviour
{
  [SerializeField] private ItemData itemData;

  private void OnValidate()
  {
    GetComponent<SpriteRenderer>().sprite = itemData.icon;
    gameObject.name = "Item object - " + itemData.itemName;
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

using UnityEngine;

public class ItemObject : MonoBehaviour
{
  [SerializeField] private Rigidbody2D rb;
  [SerializeField] private ItemData itemData;

  private void SetupVisuals()
  {
    if (!itemData) return;

    GetComponent<SpriteRenderer>().sprite = itemData.icon;
    gameObject.name = "Item object - " + itemData.itemName;
  }

  public void SetupItem(ItemData _itemData, Vector2 _velocity)
  {
    itemData = _itemData;
    rb.velocity = _velocity;

    SetupVisuals();
  }

  public void PickupItem()
  {
    if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
    {
      rb.velocity = new Vector2(0, 7);
      return;
    }

    Inventory.instance.AddItem(itemData);
    Destroy(gameObject);
  }
}

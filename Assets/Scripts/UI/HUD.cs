using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Inventory Inventory;

    void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        Inventory.ItemRemoved += InventoryScript_ItemRemoved;
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        Debug.Log("Item added to inventory: " + e.Item.Name);

        foreach (Transform Slot in transform.Find("Inventory"))
        {
            // Access the Image component in ItemImage
            Image image = Slot.GetChild(0).GetChild(0).GetComponent<Image>();
            ItemDragHandler itemDragHandler = Slot.GetChild(0).GetChild(0).GetComponent<ItemDragHandler>();

            if (image != null && !image.enabled)
            {
                image.enabled = true;
                image.sprite = e.Item.Image;
                itemDragHandler.Item = e.Item;

                Debug.Log("Image enabled and set to: " + e.Item.Image.name);
                break;
            }
        }
    }

    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e)
    {
        Debug.Log("Item removed from inventory: " + e.Item.Name);

        foreach (Transform Slot in transform.Find("Inventory"))
        {
            // Access the Image component in ItemImage
            Image image = Slot.GetChild(0).GetChild(0).GetComponent<Image>();
            ItemDragHandler itemDragHandler = Slot.GetChild(0).GetChild(0).GetComponent<ItemDragHandler>();

            // Check if the slot contains the item to remove
            if (itemDragHandler.Item == e.Item)
            {
                image.enabled = false;
                image.sprite = null;
                itemDragHandler.Item = null;

                Debug.Log("Image disabled and item removed from slot: " + Slot.name);
                break;
            }
        }
    }
}

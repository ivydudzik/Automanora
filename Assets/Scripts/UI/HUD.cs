using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Inventory Inventory;
    private int selectedSlot = 0; 
    private Transform[] slots;   

    void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
        Inventory.ItemRemoved += InventoryScript_ItemRemoved;

        // Cache the slots in the inventory
        slots = new Transform[transform.Find("Inventory").childCount];
        int index = 0;
        foreach (Transform Slot in transform.Find("Inventory"))
        {
            slots[index++] = Slot;
        }

        HighlightSlot(selectedSlot); // Highlight the first slot by default
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

    private void HighlightSlot(int slotIndex)
    {
        foreach (Transform Slot in slots)
        {
            Slot.GetComponent<Image>().color = Color.white; 
        }

        // Highlight the selected slot
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            slots[slotIndex].GetComponent<Image>().color = Color.yellow; 
        }
    }

    // Changes the selected slot based on input
    public void ChangeSelectedSlot(int direction)
    {
        if (slots.Length == 0) return;

        selectedSlot = (selectedSlot + direction + slots.Length) % slots.Length; 
        HighlightSlot(selectedSlot);
    }

    // Returns the currently selected item
    public IInventoryItem GetSelectedItem()
    {
        ItemDragHandler itemDragHandler = slots[selectedSlot].GetChild(0).GetChild(0).GetComponent<ItemDragHandler>();
        return itemDragHandler?.Item;
    }
}

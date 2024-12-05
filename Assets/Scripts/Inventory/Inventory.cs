using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private const int SLOTS = 7;
    private List<IInventoryItem> mItems = new List<IInventoryItem>();
    public GameObject itemPrefab;
    private int selectedSlot = 0; // Tracks the currently selected slot
    public event EventHandler<InventoryEventArgs> ItemAdded;
    public event EventHandler<InventoryEventArgs> ItemRemoved;
    public event EventHandler<InventoryEventArgs> ItemUsed;


    public void AddItem(IInventoryItem item)
    {
        if(mItems.Count < SLOTS)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                mItems.Add(item);
                item.OnPickup();

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }
            }
        }
    }

    internal void UseItem(IInventoryItem item)
    {
        if (ItemUsed != null)
        {
            ItemUsed(this, new InventoryEventArgs(item));
        }
    }

    public void RemoveItem(IInventoryItem item)
    {
        if(mItems.Contains(item))
        {
            mItems.Remove(item);

            item.OnDrop();

            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            if (ItemRemoved != null)
            {
                ItemRemoved(this, new InventoryEventArgs(item));
            }
        }
    }

    public void DropSelectedItem()
    {
        if (selectedSlot < mItems.Count)
        {
            IInventoryItem item = mItems[selectedSlot];
            mItems.RemoveAt(selectedSlot); // Remove item from inventory

            item.OnDrop(); // Handle dropping logic
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true; // Re-enable collider
            }

            if (ItemRemoved != null)
            {
                ItemRemoved(this, new InventoryEventArgs(item));
            }
        }
    }

    public int getInventory()
    {
        return mItems.Count;
    }

    public void ChangeSelectedSlot(int direction)
    {
        if (mItems.Count == 0) return;

        selectedSlot = (selectedSlot + direction + SLOTS) % SLOTS; // Loop around inventory
        Debug.Log($"Selected slot: {selectedSlot}");
    }
}

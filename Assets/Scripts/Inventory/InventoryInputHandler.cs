using UnityEngine;

public class InventoryInputHandler : MonoBehaviour
{
    public Inventory inventory;
    public HUD hud;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            IInventoryItem selectedItem = hud.GetSelectedItem();
            if (selectedItem != null)
            {
                inventory.RemoveItem(selectedItem);
                selectedItem.OnDrop();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Navigate left
        {
            hud.ChangeSelectedSlot(-1);
        }

        if (Input.GetKeyDown(KeyCode.E)) // Navigate right
        {
            hud.ChangeSelectedSlot(1);
        }
    }
}

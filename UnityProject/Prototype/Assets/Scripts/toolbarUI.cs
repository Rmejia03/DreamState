using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolbarUI : MonoBehaviour
{
    [SerializeField] private List<slotUI> toolbarSlots = new List<slotUI>();

    private slotUI selectedSlot;

    private void Start()
    {
        if(toolbarSlots == null || toolbarSlots.Count == 0)
        {
            Debug.LogError("Toolbar slots are not assigned in the toolbarUI component");
            return;
        }

        SelectSlot(0);
    }

    public void SelectSlot(int index)
    {
        if(index < toolbarSlots.Count)
        {
            if(selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }

            selectedSlot = toolbarSlots[index];

            selectedSlot.SetHighlight(true);
        }
    }
    public void UpdateToolbar(List<itemStats> items)
    {
        
        for (int i = 0; i < toolbarSlots.Count; i++)
        {
           
            if (i < items.Count)
            {
                toolbarSlots[i].UpdateSlot(items[i].icon);
            }
            else
            {
                toolbarSlots[i].UpdateSlot(null);
            }
        }

        if (selectedSlot != null)
        {
            selectedSlot.SetHighlight(true);
        }

    }
}

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
    private void Update()
    {
        CheckAlphaNumericKey();
    }

    public void SelectSlot(int index)
    {
        if(index < toolbarSlots.Count)
        {
            selectedSlot = toolbarSlots[index];
        }
    }
    public void UpdateToolbar(List<itemStats> items)
    {
        //Debug.Log("UpdateToolbar Called");

        for (int i = 0; i < toolbarSlots.Count; i++)
        {
            //if (toolbarSlots[i] == null)
            //{
            //    Debug.LogError($"Toolbar slot i is not assigned in the toolbarUI component");
            //    continue;
            //}

            //Debug.Log($"Updating slot i with item");
            //toolbarSlots[i].UpdateSlot(i < items.Count ? items[i].icon : null);

            if (i < items.Count)
            {
                toolbarSlots[i].UpdateSlot(items[i].icon);
            }
            else
            {
                toolbarSlots[i].UpdateSlot(null);
            }
        }

    }

    private void CheckAlphaNumericKey()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }
    }
  
}

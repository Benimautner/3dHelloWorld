using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventory;
    public GameObject[] slots;
    private int _selectedSlot;
    void Start()
    {
        _selectedSlot = 0;
        SlotChanged();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseMovement = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(mouseMovement) > 0.0f) {
            if (mouseMovement > 0.0f) {
                _selectedSlot++;
            }
            else {
                _selectedSlot--;
            }

            if (_selectedSlot < 0) {
                _selectedSlot = slots.Length - 1;
            }
            else if (_selectedSlot > slots.Length - 1) {
                _selectedSlot = 0;
            }
            SlotChanged();
        }
    }

    private void SlotChanged()
    {
        for (int i = 0; i < slots.Length; i++) {
            var slot = slots[i];
            if (i == _selectedSlot) {
                slot.GetComponent<Image>().color = new Color32(255, 255, 255, 200);
            }
            else {
                slot.GetComponent<Image>().color = new Color32(255, 255, 255, 50);
            }
        }
    }
}

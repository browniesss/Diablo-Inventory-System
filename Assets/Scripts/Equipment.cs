using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ������ ������ ��ũ��Ʈ
public class Equipment : MonoBehaviour
{
    public Inventory_Slot[] equipt_Slots;

    void Start()
    {
        equipt_Slots = GetComponentsInChildren<Inventory_Slot>();
    }

    void Update()
    {

    }
}

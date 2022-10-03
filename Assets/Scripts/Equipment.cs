using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 장비 슬롯을 저장할 스크립트
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

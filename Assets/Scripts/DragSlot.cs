using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 드래그 하고 있는 슬롯 스크립트
public class DragSlot : Singleton<DragSlot>
{
    [Header("Draging slot Info")]
    public Inventory_Slot draging_slot; // 드래그 하는 슬롯을 담을 변수.

    [Header("own Info")]
    public Image own_Image;

    public void Drag_Image_Set(Inventory_Slot slot) // 드래그 중인 슬롯의 이미지를 변경해주는 함수.
    {
        draging_slot = slot;

        own_Image.sprite = slot.slot_Item.item_Sprite;

        own_Image.color = new Color(255, 255, 255, 1f);
    }

    public void Drag_Image_End() // 드래그 종료시 호출 해주는 함수
    {
        draging_slot = null;

        own_Image.color = new Color(255, 255, 255, 0f);
    }

    void Start()
    {
        own_Image = GetComponent<Image>();
    }

    void Update()
    {

    }
}

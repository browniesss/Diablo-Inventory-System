using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 스크립트
public class Inventory : Singleton<Inventory>
{
    public Inventory_Slot[] slots; // 인벤토리 슬롯들 

    void Start()
    {
        slots = GetComponentsInChildren<Inventory_Slot>(); // 슬롯들 받아옴.

        for (int i = 0; i < slots.Length; i++) // 슬롯 번호를 매겨줌.
            slots[i].slot_Number = i;
    }

    public void Item_Add(ItemInfo item) // 슬롯에 아이템을 추가해주는 함수.
    {
        switch (item.item_Shape) // 아이템 형태에 따라서 아이템 추가 
        {
            case ITEM_SHAPE.DOT: // 한 칸 점 모양의 아이템
                foreach (var slot in slots)
                {
                    if (slot.slot_Item == null) // 빈 슬롯에 아이템을 추가해줌.
                    {
                        slot.Item_Add_Slot(item);
                        break;
                    }
                }
                break; 
            case ITEM_SHAPE.COLUMN: // 세로로 2칸을 차지하는 막대모양 아이템.
                for (int i = 0; i < slots.Length; i++) // 슬롯들 검사
                {
                    if (slots[i].slot_Item == null && slots[i + 10]?.slot_Item == null) // 아래칸과 해당칸이 비어있는지 검사 후 아이템을 넣어줌.
                    {
                        slots[i].Item_Add_Slot(item);
                        slots[i + 10].slot_Item = item; // 아래 슬롯에 아이템을 추가해준 후
                        slots[i + 10].isConnect = true; // 위 슬롯과 연결돼있다는 것을 알려주는 bool 변수를 true로 해줌.

                        RectTransform rect = slots[i].GetComponent<RectTransform>();

                        rect.pivot = new Vector2(0.5f, 1.0f);
                        rect.localScale = new Vector3(2, 2, 1);
                        break;
                    }
                }
                break;
        }
    }

    void Update()
    {

    }
}

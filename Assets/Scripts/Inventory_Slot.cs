using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 인벤토리 슬롯 스크립트
public class Inventory_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemInfo slot_Item; // 해당 슬롯 아이템

    public bool isEquip; // 장비 슬롯인지 검사하는 bool 변수

    public ITEM_TYPE slot_Type; // 해당 슬롯의 장비 타입. 장비 창에서 사용 

    public int slot_Number; // 슬롯 번호

    public bool isConnect = false;

    [SerializeField]
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void Slot_Reset()
    {
        RectTransform rect = this.GetComponent<RectTransform>();

        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.localScale = new Vector3(1, 1, 1);
    }

    public void Slot_Resize() // 세로로 2칸짜리 아이템을 넣었을 경우 슬롯의 사이즈 조절
    {
        RectTransform rect = this.GetComponent<RectTransform>();

        rect.pivot = new Vector2(0.5f, 1.0f);
        rect.localScale = new Vector3(2, 2, 1);

        isConnect = false;
        Inventory.Instance.slots[slot_Number + 10].isConnect = true;
    }

    public void Item_Add_Slot(ItemInfo item) // 슬롯에 아이템을 추가해주는 함수
    {
        slot_Item = item;

        image.sprite = item.item_Sprite; // 이미지를 변경
        image.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작
    {
        if (slot_Item != null) // 아이템이 있을 경우에만
        {
            DragSlot.Instance.Drag_Image_Set(this); // 드래그 하는 슬롯 스크립트의 드래그 이미지 세팅 함수를 호출
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) // 드래그 중
    {
        if (slot_Item != null) // 아이템이 있다면 
        {
            DragSlot.Instance.transform.position = eventData.position; // 드래그 슬롯의 좌표를 변경.
        }
    }

    public void OnEndDrag(PointerEventData eventData) // 드래그 종료
    {
        DragSlot.Instance.Drag_Image_End(); // 드래그 하던 스킬에게 드래그가 끝났다는 함수를 호출함. 
    }

    public void OnDrop(PointerEventData eventData) // 해당 슬롯에 드랍이 일어났을 경우
    {
        if (DragSlot.Instance.draging_slot == this) // 원래 자기 슬롯에 내려 놓았을 경우
            return;

        if (DragSlot.Instance.draging_slot != null) 
        {
            if (DragSlot.Instance.draging_slot.isEquip == false && isEquip == false) // 인벤토리 끼리 드래그앤 드롭
                Slot_Drop();
            else if (DragSlot.Instance.draging_slot.isEquip == false && isEquip == true) // 해당 슬롯이 장비 슬롯이고 인벤토리에서 드래그 했다면
                Item_Equip();
            else if (DragSlot.Instance.draging_slot.isEquip == true && isEquip == false) // 해당 슬롯이 인벤토리이고 장비창에서 드래그 했다면
                Item_Equip_Off();
        }
    }

    void Item_Equip() // 장비 장착 함수
    {
        if (DragSlot.Instance.draging_slot.slot_Item.item_Type != slot_Type) // 해당 장비 슬롯의 타입과 드래그 아이템의 타입이 다르다면 return
            return;

        if (slot_Item != null) // 아이템이 있었다면
        {
            switch (slot_Item.item_Shape) // 아이템 형태에 따라 진행
            {
                case ITEM_SHAPE.DOT: 
                    ItemInfo tempItem = slot_Item;

                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);

                    DragSlot.Instance.draging_slot.ClearSlot();
                    DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem);
                    break;
                case ITEM_SHAPE.COLUMN: 
                    ItemInfo tempItem2 = slot_Item;

                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);

                    if (!DragSlot.Instance.draging_slot.isConnect) // 연결돼있던 슬롯 ( 2칸중 아래슬롯 ) 이 아니라면
                    {
                        int tempn = DragSlot.Instance.draging_slot.slot_Number;

                        // 아래 슬롯까지 처리
                        DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem2);
                        Inventory.Instance.slots[tempn + 10].slot_Item = tempItem2;
                    }
                    else  // 연결돼있던 슬롯 ( 2칸중 아래슬롯 ) 이라면
                    {
                        int tempn = DragSlot.Instance.draging_slot.slot_Number;

                        // 위 슬롯까지 처리
                        Inventory.Instance.slots[tempn - 10].Item_Add_Slot(tempItem2);
                        DragSlot.Instance.draging_slot.slot_Item = tempItem2;
                    }
                    break;
            }
        }
        else // 아이템이 없었다면
        {
            //this.Slot_Reset();
            this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item); // 장비 슬롯에 아이템을 넣어줌

            switch (DragSlot.Instance.draging_slot.slot_Item.item_Shape) // 아이템 형태에 따라 진행
            {
                case ITEM_SHAPE.DOT:
                    DragSlot.Instance.draging_slot.ClearSlot();
                    break;
                case ITEM_SHAPE.COLUMN:
                    if (!DragSlot.Instance.draging_slot.isConnect)
                    {
                        int tempn = DragSlot.Instance.draging_slot.slot_Number;

                        DragSlot.Instance.draging_slot.Slot_Reset();
                        DragSlot.Instance.draging_slot.ClearSlot();

                        Inventory.Instance.slots[tempn + 10].ClearSlot();
                    }
                    else
                    {
                        int tempn = DragSlot.Instance.draging_slot.slot_Number;

                        Inventory.Instance.slots[tempn - 10].Slot_Reset();
                        Inventory.Instance.slots[tempn - 10].ClearSlot();

                        DragSlot.Instance.draging_slot.ClearSlot();
                    }

                    break;
            }
        }
    }

    void Item_Equip_Off() // 아이템 장착 해제 함수
    {
        if (slot_Item != null) // 아이템이 있었다면 
        {
            if (slot_Item.item_Type != DragSlot.Instance.draging_slot.slot_Item.item_Type) // 드래그하는 아이템과 해당 아이템의 타입이 다르면 return 
                return;

            switch (DragSlot.Instance.draging_slot.slot_Item.item_Shape) // 아이템 형태에 따라서 
            {
                case ITEM_SHAPE.DOT: 
                    ItemInfo tempItem = slot_Item;

                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);

                    DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem);
                    break;
                case ITEM_SHAPE.COLUMN:
                    ItemInfo tempItem2 = slot_Item;
                    if (!isConnect) 
                    {
                        this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                        Inventory.Instance.slots[slot_Number + 10].slot_Item = DragSlot.Instance.draging_slot.slot_Item;

                        DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem2);
                    }
                    else
                    {
                        Inventory.Instance.slots[slot_Number - 10].Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                        this.slot_Item = DragSlot.Instance.draging_slot.slot_Item;

                        DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem2);
                    }
                    break;
            }
        }
        else // 아이템이 없었다면 
        {
            switch (DragSlot.Instance.draging_slot.slot_Item.item_Shape) 
            {
                case ITEM_SHAPE.DOT:
                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);

                    DragSlot.Instance.draging_slot.ClearSlot();
                    break;
                case ITEM_SHAPE.COLUMN:
                    if (Inventory.Instance.slots[slot_Number + 10].slot_Item == null) // 아래 슬롯까지 비어있었으면 진행
                    {
                        this.Slot_Resize();
                        this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                        Inventory.Instance.slots[slot_Number + 10].slot_Item = DragSlot.Instance.draging_slot.slot_Item;

                        DragSlot.Instance.draging_slot.ClearSlot();
                    }
                    break;
            }
        }

    }

    void Slot_Drop() // 인벤토리 끼리 드래그앤 드랍이 일어났을 경우
    {
        switch (DragSlot.Instance.draging_slot.slot_Item.item_Shape)
        {
            case ITEM_SHAPE.DOT:
                if (slot_Item != null) // 아이템이 있었다면 
                {
                    switch (slot_Item.item_Shape) // 해당 아이템 형태에 따라 진행
                    {
                        case ITEM_SHAPE.DOT: // 같은 형태라면 아이템 슬롯 체인지
                            ItemInfo tempItem = slot_Item;

                            Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                            DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem);
                            break;
                        case ITEM_SHAPE.COLUMN: // 해당 아이템이 2칸 열 모양 아이템이라면 
                            int tempNum = DragSlot.Instance.draging_slot.slot_Number;  

                            if (Inventory.Instance.slots[tempNum + 10].slot_Item == null) // 아래 슬롯까지 검사
                            {
                                ItemInfo tempItem2 = slot_Item;

                                if (!isConnect) // 해당 아이템의 아래 부분이 아닌 곳 = 2칸 중 위 부분에 드랍했는지
                                {
                                    this.ClearSlot();
                                    this.Slot_Reset();
                                    Inventory.Instance.slots[slot_Number + 10].ClearSlot(); // 아래부분까지 비워줌.

                                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                }
                                else // 해당 아이템의 아래부분인곳 = 2칸 중 아래 부분에 드랍했다면
                                {
                                    // 위 부분까지 비워줌.
                                    Inventory.Instance.slots[slot_Number - 10].ClearSlot(); 
                                    Inventory.Instance.slots[slot_Number - 10].Slot_Reset();
                                    this.ClearSlot();

                                    Inventory.Instance.slots[slot_Number].Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                }

                                DragSlot.Instance.draging_slot.Slot_Resize();
                                DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem2);
                                Inventory.Instance.slots[tempNum + 10].slot_Item = tempItem2;
                            }
                            break;
                    }
                }
                else // 아이템이 없었다면
                {
                    Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                    DragSlot.Instance.draging_slot.ClearSlot();
                }
                break;
            case ITEM_SHAPE.COLUMN: // 2칸짜리 열 형태의 아이템이라면
                if (slot_Item != null) // 아이템이 있었다면
                {
                    switch (slot_Item.item_Shape) // 해당 아이템의 형태에 따라서
                    {
                        case ITEM_SHAPE.DOT: // 점 형태의 한칸 아이템이라면
                            if (Inventory.Instance.slots[slot_Number + 10].slot_Item == null) // 해당 슬롯 아래칸이 비었으면 
                            {
                                ItemInfo tempItem = slot_Item;

                                Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                Inventory.Instance.slots[slot_Number + 10].slot_Item = slot_Item;
                                Slot_Resize();

                                if (!DragSlot.Instance.draging_slot.isConnect) // 2칸짜리 열 형태의 아이템 칸중 윗 부분이라면
                                {
                                    int tempNum = DragSlot.Instance.draging_slot.slot_Number;

                                    DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem);
                                    DragSlot.Instance.draging_slot.Slot_Reset();

                                    Inventory.Instance.slots[tempNum + 10].ClearSlot();
                                }
                                else  // 2칸짜리 열 형태의 아이템 칸중 아래 부분이라면
                                {
                                    int tempNum = DragSlot.Instance.draging_slot.slot_Number;

                                    Inventory.Instance.slots[tempNum - 10].Item_Add_Slot(tempItem);
                                    Inventory.Instance.slots[tempNum - 10].Slot_Reset();

                                    DragSlot.Instance.draging_slot.ClearSlot();
                                }

                            }
                            break;
                        case ITEM_SHAPE.COLUMN: // 열 형태 아이템이라면
                            ItemInfo tempItem2 = slot_Item;

                            if (!DragSlot.Instance.draging_slot.isConnect)  // 드래그 아이템이 2칸짜리 열 형태의 아이템 칸중 윗 부분이라면
                            {
                                if (!isConnect)  // 2칸짜리 열 형태의 아이템 칸중 윗 부분이라면
                                {
                                    this.ClearSlot();
                                    this.Slot_Resize();
                                    Inventory.Instance.slots[slot_Number + 10].ClearSlot();

                                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                    Inventory.Instance.slots[slot_Number + 10].slot_Item = DragSlot.Instance.draging_slot.slot_Item;
                                }
                                else // 2칸짜리 열 형태의 아이템 칸중 아래 부분이라면
                                {
                                    Inventory.Instance.slots[slot_Number - 10].ClearSlot();
                                    Inventory.Instance.slots[slot_Number - 10].Slot_Resize();
                                    this.ClearSlot();

                                    Inventory.Instance.slots[slot_Number - 10].Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                    this.slot_Item = DragSlot.Instance.draging_slot.slot_Item;
                                }

                                int tempNumber = DragSlot.Instance.draging_slot.slot_Number;

                                DragSlot.Instance.draging_slot.ClearSlot();
                                DragSlot.Instance.draging_slot.Slot_Resize();
                                Inventory.Instance.slots[tempNumber + 10].ClearSlot();

                                DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem2);
                                Inventory.Instance.slots[tempNumber + 10].slot_Item = tempItem2;
                            }
                            else  // 드래그 아이템이 2칸짜리 열 형태의 아이템 칸중 아래 부분이라면
                            {
                                if (!isConnect) // 2칸짜리 열 형태의 아이템 칸중 윗 부분이라면
                                {
                                    this.ClearSlot();
                                    this.Slot_Resize();
                                    Inventory.Instance.slots[slot_Number + 10].ClearSlot();

                                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                    Inventory.Instance.slots[slot_Number + 10].slot_Item = DragSlot.Instance.draging_slot.slot_Item;

                                }
                                else // 2칸짜리 열 형태의 아이템 칸중 아래 부분이라면
                                {
                                    Inventory.Instance.slots[slot_Number - 10].ClearSlot();
                                    Inventory.Instance.slots[slot_Number - 10].Slot_Resize();
                                    this.ClearSlot();

                                    Inventory.Instance.slots[slot_Number - 10].Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                    this.slot_Item = DragSlot.Instance.draging_slot.slot_Item;
                                }

                                int tempN = DragSlot.Instance.draging_slot.slot_Number;

                                Inventory.Instance.slots[tempN - 10].ClearSlot();
                                Inventory.Instance.slots[tempN - 10].Slot_Resize();
                                DragSlot.Instance.draging_slot.ClearSlot();

                                Inventory.Instance.slots[tempN - 10].Item_Add_Slot(tempItem2);
                                DragSlot.Instance.draging_slot.slot_Item = tempItem2;
                            }
                            break;
                    }
                }
                else // 아이템이 없었다면 
                {
                    if (Inventory.Instance.slots[slot_Number + 10].slot_Item == null) // 아래 칸까지 아이템이 없었다면 
                    {
                        Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                        Inventory.Instance.slots[slot_Number + 10].slot_Item = slot_Item;
                        Slot_Resize();

                        if (!DragSlot.Instance.draging_slot.isConnect)
                        {
                            Debug.Log("나없어");
                            int tempNum = DragSlot.Instance.draging_slot.slot_Number;

                            DragSlot.Instance.draging_slot.Slot_Reset();
                            DragSlot.Instance.draging_slot.ClearSlot();
                            Inventory.Instance.slots[tempNum + 10].ClearSlot();
                        }
                        else
                        {
                            Debug.Log("나있어");
                            int tempNum = DragSlot.Instance.draging_slot.slot_Number;

                            Inventory.Instance.slots[tempNum - 10].Slot_Reset();
                            Inventory.Instance.slots[tempNum - 10].ClearSlot();

                            DragSlot.Instance.draging_slot.ClearSlot();
                        }
                    }
                    else // 아래 칸 아이템이 있었다면 
                    {
                        int tempNumber = DragSlot.Instance.draging_slot.slot_Number; // 드래그 중인 슬롯의 번호를 받아옴.
                        if (Inventory.Instance.slots[slot_Number + 10]?.slot_Number == tempNumber
                            || Inventory.Instance.slots[slot_Number + 20]?.slot_Number == tempNumber) 
                        {
                            Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                            Slot_Resize();

                            if (Inventory.Instance.slots[slot_Number + 10]?.slot_Number == tempNumber)
                            {
                                Inventory.Instance.slots[tempNumber + 10].ClearSlot();
                                Inventory.Instance.slots[tempNumber].ClearSlot();
                                Inventory.Instance.slots[tempNumber].Slot_Reset();

                                Inventory.Instance.slots[tempNumber].slot_Item = slot_Item;
                            }
                            else
                            {
                                Inventory.Instance.slots[tempNumber].ClearSlot();
                                Inventory.Instance.slots[tempNumber - 10].ClearSlot();
                                Inventory.Instance.slots[tempNumber - 10].Slot_Reset();

                                Inventory.Instance.slots[tempNumber - 10].slot_Item = slot_Item;
                            }

                        }
                    }
                }
                break;
        }
    }

    // 해당 슬롯 하나 삭제
    private void ClearSlot()
    {
        slot_Item = null;
        // isConnect = false;
        image.sprite = null;
        image.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
    }

    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData) // 마우스 포인터가 들어오면
    {
        if (slot_Item == null) // 아이템이 없다면 return
            return;

        GameManager.Instance.item_Explain_window.Item_Explain(slot_Item); // 아이템 툴팁 창을 띄움.
    }

    public void OnPointerExit(PointerEventData eventData) // 마우스 포인터가 나가면  
    {
        if (slot_Item == null) // 아이템이 없다면 return 
            return;

        GameManager.Instance.item_Explain_window.Item_Explain_Off(); // 아이템 툴팁 창을 끔.
    }
}

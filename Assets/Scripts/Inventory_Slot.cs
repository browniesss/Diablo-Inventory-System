using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// �κ��丮 ���� ��ũ��Ʈ
public class Inventory_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemInfo slot_Item; // �ش� ���� ������

    public bool isEquip; // ��� �������� �˻��ϴ� bool ����

    public ITEM_TYPE slot_Type; // �ش� ������ ��� Ÿ��. ��� â���� ��� 

    public int slot_Number; // ���� ��ȣ

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

    public void Slot_Resize() // ���η� 2ĭ¥�� �������� �־��� ��� ������ ������ ����
    {
        RectTransform rect = this.GetComponent<RectTransform>();

        rect.pivot = new Vector2(0.5f, 1.0f);
        rect.localScale = new Vector3(2, 2, 1);

        isConnect = false;
        Inventory.Instance.slots[slot_Number + 10].isConnect = true;
    }

    public void Item_Add_Slot(ItemInfo item) // ���Կ� �������� �߰����ִ� �Լ�
    {
        slot_Item = item;

        image.sprite = item.item_Sprite; // �̹����� ����
        image.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }

    public void OnBeginDrag(PointerEventData eventData) // �巡�� ����
    {
        if (slot_Item != null) // �������� ���� ��쿡��
        {
            DragSlot.Instance.Drag_Image_Set(this); // �巡�� �ϴ� ���� ��ũ��Ʈ�� �巡�� �̹��� ���� �Լ��� ȣ��
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) // �巡�� ��
    {
        if (slot_Item != null) // �������� �ִٸ� 
        {
            DragSlot.Instance.transform.position = eventData.position; // �巡�� ������ ��ǥ�� ����.
        }
    }

    public void OnEndDrag(PointerEventData eventData) // �巡�� ����
    {
        DragSlot.Instance.Drag_Image_End(); // �巡�� �ϴ� ��ų���� �巡�װ� �����ٴ� �Լ��� ȣ����. 
    }

    public void OnDrop(PointerEventData eventData) // �ش� ���Կ� ����� �Ͼ�� ���
    {
        if (DragSlot.Instance.draging_slot == this) // ���� �ڱ� ���Կ� ���� ������ ���
            return;

        if (DragSlot.Instance.draging_slot != null) 
        {
            if (DragSlot.Instance.draging_slot.isEquip == false && isEquip == false) // �κ��丮 ���� �巡�׾� ���
                Slot_Drop();
            else if (DragSlot.Instance.draging_slot.isEquip == false && isEquip == true) // �ش� ������ ��� �����̰� �κ��丮���� �巡�� �ߴٸ�
                Item_Equip();
            else if (DragSlot.Instance.draging_slot.isEquip == true && isEquip == false) // �ش� ������ �κ��丮�̰� ���â���� �巡�� �ߴٸ�
                Item_Equip_Off();
        }
    }

    void Item_Equip() // ��� ���� �Լ�
    {
        if (DragSlot.Instance.draging_slot.slot_Item.item_Type != slot_Type) // �ش� ��� ������ Ÿ�԰� �巡�� �������� Ÿ���� �ٸ��ٸ� return
            return;

        if (slot_Item != null) // �������� �־��ٸ�
        {
            switch (slot_Item.item_Shape) // ������ ���¿� ���� ����
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

                    if (!DragSlot.Instance.draging_slot.isConnect) // ������ִ� ���� ( 2ĭ�� �Ʒ����� ) �� �ƴ϶��
                    {
                        int tempn = DragSlot.Instance.draging_slot.slot_Number;

                        // �Ʒ� ���Ա��� ó��
                        DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem2);
                        Inventory.Instance.slots[tempn + 10].slot_Item = tempItem2;
                    }
                    else  // ������ִ� ���� ( 2ĭ�� �Ʒ����� ) �̶��
                    {
                        int tempn = DragSlot.Instance.draging_slot.slot_Number;

                        // �� ���Ա��� ó��
                        Inventory.Instance.slots[tempn - 10].Item_Add_Slot(tempItem2);
                        DragSlot.Instance.draging_slot.slot_Item = tempItem2;
                    }
                    break;
            }
        }
        else // �������� �����ٸ�
        {
            //this.Slot_Reset();
            this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item); // ��� ���Կ� �������� �־���

            switch (DragSlot.Instance.draging_slot.slot_Item.item_Shape) // ������ ���¿� ���� ����
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

    void Item_Equip_Off() // ������ ���� ���� �Լ�
    {
        if (slot_Item != null) // �������� �־��ٸ� 
        {
            if (slot_Item.item_Type != DragSlot.Instance.draging_slot.slot_Item.item_Type) // �巡���ϴ� �����۰� �ش� �������� Ÿ���� �ٸ��� return 
                return;

            switch (DragSlot.Instance.draging_slot.slot_Item.item_Shape) // ������ ���¿� ���� 
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
        else // �������� �����ٸ� 
        {
            switch (DragSlot.Instance.draging_slot.slot_Item.item_Shape) 
            {
                case ITEM_SHAPE.DOT:
                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);

                    DragSlot.Instance.draging_slot.ClearSlot();
                    break;
                case ITEM_SHAPE.COLUMN:
                    if (Inventory.Instance.slots[slot_Number + 10].slot_Item == null) // �Ʒ� ���Ա��� ����־����� ����
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

    void Slot_Drop() // �κ��丮 ���� �巡�׾� ����� �Ͼ�� ���
    {
        switch (DragSlot.Instance.draging_slot.slot_Item.item_Shape)
        {
            case ITEM_SHAPE.DOT:
                if (slot_Item != null) // �������� �־��ٸ� 
                {
                    switch (slot_Item.item_Shape) // �ش� ������ ���¿� ���� ����
                    {
                        case ITEM_SHAPE.DOT: // ���� ���¶�� ������ ���� ü����
                            ItemInfo tempItem = slot_Item;

                            Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                            DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem);
                            break;
                        case ITEM_SHAPE.COLUMN: // �ش� �������� 2ĭ �� ��� �������̶�� 
                            int tempNum = DragSlot.Instance.draging_slot.slot_Number;  

                            if (Inventory.Instance.slots[tempNum + 10].slot_Item == null) // �Ʒ� ���Ա��� �˻�
                            {
                                ItemInfo tempItem2 = slot_Item;

                                if (!isConnect) // �ش� �������� �Ʒ� �κ��� �ƴ� �� = 2ĭ �� �� �κп� ����ߴ���
                                {
                                    this.ClearSlot();
                                    this.Slot_Reset();
                                    Inventory.Instance.slots[slot_Number + 10].ClearSlot(); // �Ʒ��κб��� �����.

                                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                }
                                else // �ش� �������� �Ʒ��κ��ΰ� = 2ĭ �� �Ʒ� �κп� ����ߴٸ�
                                {
                                    // �� �κб��� �����.
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
                else // �������� �����ٸ�
                {
                    Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                    DragSlot.Instance.draging_slot.ClearSlot();
                }
                break;
            case ITEM_SHAPE.COLUMN: // 2ĭ¥�� �� ������ �������̶��
                if (slot_Item != null) // �������� �־��ٸ�
                {
                    switch (slot_Item.item_Shape) // �ش� �������� ���¿� ����
                    {
                        case ITEM_SHAPE.DOT: // �� ������ ��ĭ �������̶��
                            if (Inventory.Instance.slots[slot_Number + 10].slot_Item == null) // �ش� ���� �Ʒ�ĭ�� ������� 
                            {
                                ItemInfo tempItem = slot_Item;

                                Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                Inventory.Instance.slots[slot_Number + 10].slot_Item = slot_Item;
                                Slot_Resize();

                                if (!DragSlot.Instance.draging_slot.isConnect) // 2ĭ¥�� �� ������ ������ ĭ�� �� �κ��̶��
                                {
                                    int tempNum = DragSlot.Instance.draging_slot.slot_Number;

                                    DragSlot.Instance.draging_slot.Item_Add_Slot(tempItem);
                                    DragSlot.Instance.draging_slot.Slot_Reset();

                                    Inventory.Instance.slots[tempNum + 10].ClearSlot();
                                }
                                else  // 2ĭ¥�� �� ������ ������ ĭ�� �Ʒ� �κ��̶��
                                {
                                    int tempNum = DragSlot.Instance.draging_slot.slot_Number;

                                    Inventory.Instance.slots[tempNum - 10].Item_Add_Slot(tempItem);
                                    Inventory.Instance.slots[tempNum - 10].Slot_Reset();

                                    DragSlot.Instance.draging_slot.ClearSlot();
                                }

                            }
                            break;
                        case ITEM_SHAPE.COLUMN: // �� ���� �������̶��
                            ItemInfo tempItem2 = slot_Item;

                            if (!DragSlot.Instance.draging_slot.isConnect)  // �巡�� �������� 2ĭ¥�� �� ������ ������ ĭ�� �� �κ��̶��
                            {
                                if (!isConnect)  // 2ĭ¥�� �� ������ ������ ĭ�� �� �κ��̶��
                                {
                                    this.ClearSlot();
                                    this.Slot_Resize();
                                    Inventory.Instance.slots[slot_Number + 10].ClearSlot();

                                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                    Inventory.Instance.slots[slot_Number + 10].slot_Item = DragSlot.Instance.draging_slot.slot_Item;
                                }
                                else // 2ĭ¥�� �� ������ ������ ĭ�� �Ʒ� �κ��̶��
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
                            else  // �巡�� �������� 2ĭ¥�� �� ������ ������ ĭ�� �Ʒ� �κ��̶��
                            {
                                if (!isConnect) // 2ĭ¥�� �� ������ ������ ĭ�� �� �κ��̶��
                                {
                                    this.ClearSlot();
                                    this.Slot_Resize();
                                    Inventory.Instance.slots[slot_Number + 10].ClearSlot();

                                    this.Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                                    Inventory.Instance.slots[slot_Number + 10].slot_Item = DragSlot.Instance.draging_slot.slot_Item;

                                }
                                else // 2ĭ¥�� �� ������ ������ ĭ�� �Ʒ� �κ��̶��
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
                else // �������� �����ٸ� 
                {
                    if (Inventory.Instance.slots[slot_Number + 10].slot_Item == null) // �Ʒ� ĭ���� �������� �����ٸ� 
                    {
                        Item_Add_Slot(DragSlot.Instance.draging_slot.slot_Item);
                        Inventory.Instance.slots[slot_Number + 10].slot_Item = slot_Item;
                        Slot_Resize();

                        if (!DragSlot.Instance.draging_slot.isConnect)
                        {
                            Debug.Log("������");
                            int tempNum = DragSlot.Instance.draging_slot.slot_Number;

                            DragSlot.Instance.draging_slot.Slot_Reset();
                            DragSlot.Instance.draging_slot.ClearSlot();
                            Inventory.Instance.slots[tempNum + 10].ClearSlot();
                        }
                        else
                        {
                            Debug.Log("���־�");
                            int tempNum = DragSlot.Instance.draging_slot.slot_Number;

                            Inventory.Instance.slots[tempNum - 10].Slot_Reset();
                            Inventory.Instance.slots[tempNum - 10].ClearSlot();

                            DragSlot.Instance.draging_slot.ClearSlot();
                        }
                    }
                    else // �Ʒ� ĭ �������� �־��ٸ� 
                    {
                        int tempNumber = DragSlot.Instance.draging_slot.slot_Number; // �巡�� ���� ������ ��ȣ�� �޾ƿ�.
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

    // �ش� ���� �ϳ� ����
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

    public void OnPointerEnter(PointerEventData eventData) // ���콺 �����Ͱ� ������
    {
        if (slot_Item == null) // �������� ���ٸ� return
            return;

        GameManager.Instance.item_Explain_window.Item_Explain(slot_Item); // ������ ���� â�� ���.
    }

    public void OnPointerExit(PointerEventData eventData) // ���콺 �����Ͱ� ������  
    {
        if (slot_Item == null) // �������� ���ٸ� return 
            return;

        GameManager.Instance.item_Explain_window.Item_Explain_Off(); // ������ ���� â�� ��.
    }
}

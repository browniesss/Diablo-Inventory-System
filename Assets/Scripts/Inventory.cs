using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �κ��丮 ��ũ��Ʈ
public class Inventory : Singleton<Inventory>
{
    public Inventory_Slot[] slots; // �κ��丮 ���Ե� 

    void Start()
    {
        slots = GetComponentsInChildren<Inventory_Slot>(); // ���Ե� �޾ƿ�.

        for (int i = 0; i < slots.Length; i++) // ���� ��ȣ�� �Ű���.
            slots[i].slot_Number = i;
    }

    public void Item_Add(ItemInfo item) // ���Կ� �������� �߰����ִ� �Լ�.
    {
        switch (item.item_Shape) // ������ ���¿� ���� ������ �߰� 
        {
            case ITEM_SHAPE.DOT: // �� ĭ �� ����� ������
                foreach (var slot in slots)
                {
                    if (slot.slot_Item == null) // �� ���Կ� �������� �߰�����.
                    {
                        slot.Item_Add_Slot(item);
                        break;
                    }
                }
                break; 
            case ITEM_SHAPE.COLUMN: // ���η� 2ĭ�� �����ϴ� ������ ������.
                for (int i = 0; i < slots.Length; i++) // ���Ե� �˻�
                {
                    if (slots[i].slot_Item == null && slots[i + 10]?.slot_Item == null) // �Ʒ�ĭ�� �ش�ĭ�� ����ִ��� �˻� �� �������� �־���.
                    {
                        slots[i].Item_Add_Slot(item);
                        slots[i + 10].slot_Item = item; // �Ʒ� ���Կ� �������� �߰����� ��
                        slots[i + 10].isConnect = true; // �� ���԰� ������ִٴ� ���� �˷��ִ� bool ������ true�� ����.

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

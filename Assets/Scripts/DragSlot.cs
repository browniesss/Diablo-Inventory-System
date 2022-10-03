using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �巡�� �ϰ� �ִ� ���� ��ũ��Ʈ
public class DragSlot : Singleton<DragSlot>
{
    [Header("Draging slot Info")]
    public Inventory_Slot draging_slot; // �巡�� �ϴ� ������ ���� ����.

    [Header("own Info")]
    public Image own_Image;

    public void Drag_Image_Set(Inventory_Slot slot) // �巡�� ���� ������ �̹����� �������ִ� �Լ�.
    {
        draging_slot = slot;

        own_Image.sprite = slot.slot_Item.item_Sprite;

        own_Image.color = new Color(255, 255, 255, 1f);
    }

    public void Drag_Image_End() // �巡�� ����� ȣ�� ���ִ� �Լ�
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

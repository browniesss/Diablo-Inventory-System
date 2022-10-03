using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �� ��ũ��Ʈ
public class ItemSphere : MonoBehaviour
{
    [Header("Sphere Items")]
    public ItemInfo item;

    [SerializeField]
    private TextMesh item_Text;

    void Start()
    {
        Item_Name_Show();
    }

    void Item_Name_Show() // ������ ��޿� ���� ������ �̸� �ؽ�Ʈ �� ����
    {
        switch (item.item_Class)
        {
            case ITEM_CLASS.LOW_LEVEL:
                item_Text.color = new Color(116 / 255f, 116 / 255f, 116 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.NORMAL:
                item_Text.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.HIGH_LEVEL:
                item_Text.color = new Color(0 / 255f, 56 / 255f, 255 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.MAGIC:
                item_Text.color = new Color(255 / 255f, 234 / 255f, 0 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.EPIC:
                item_Text.color = new Color(255 / 255f, 122 / 255f, 0 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.LEGEND:
                item_Text.color = new Color(47 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);
                break;
        }

        item_Text.text = item.item_Name;
    }

    void Update()
    {

    }
}

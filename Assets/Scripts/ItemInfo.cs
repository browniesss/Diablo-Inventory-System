using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��� 
public enum ITEM_CLASS
{
    LOW_LEVEL = 5000,
    NORMAL,
    HIGH_LEVEL,
    MAGIC,
    EPIC,
    LEGEND
}

// ������ ���
public enum ITEM_SHAPE
{
    DOT = 3000, // �� ĭ
    COLUMN // ��ĭ ,����
}

// ������ Ÿ��
public enum ITEM_TYPE
{
    CHEST = 1000,
    WEAPON,
    RING,
    SHOULDER,
    PANTS,
    GLOVES,
    SHOES,
    WRIST,
    HEAD,
    SUB_EQUIP,
}

public class ItemInfo : MonoBehaviour
{
    [Header("Item Info")]
    public string item_Name;
    public Sprite item_Sprite;
    public string item_Grade;
    public string item_real_Type;
    public string item_Dps;
    public string item_Damage;
    public string item_Attack_Speed;
    [TextArea]
    public string item_Option;
    [TextArea]
    public string item_Explain;

    public ITEM_CLASS item_Class;

    public ITEM_SHAPE item_Shape;

    public ITEM_TYPE item_Type;

    void Start()
    {

    }

    void Update()
    {

    }
}

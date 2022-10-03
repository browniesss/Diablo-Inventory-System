using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ ���� â ��ũ��Ʈ
public class Item_ExplainWindow : MonoBehaviour
{
    [Header("Item Image")]
    public Image item_Image;

    [Header("Item Explain Text")] // �������� ���� �ؽ�Ʈ��
    public Text item_Name_Text;
    public Text item_Grade_Text;
    public Text item_Type_Text;
    public Text item_Dps_Text;
    public Text item_Damage_or_Shield_Text;
    public Text item_Damage_Text;
    public Text item_Damage;
    public Text item_Attack_Speed_Text;
    public Text item_Attack_Speed;
    public Text item_Option_Text;
    public Text item_Explain_Text;

    public void Item_Explain(ItemInfo item) // ������ ����â �����ִ� �Լ�
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        RectTransform item_rect = item_Image.GetComponent<RectTransform>();

        switch (item.item_Shape) // ������ ��翡 ���� ����â ũ�� ����
        {
            case ITEM_SHAPE.DOT:
                rect.localScale = new Vector3(1.5f, 1.0f, 1);
                item_rect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                item_rect.anchoredPosition = new Vector3(22.2f, -61.0f, 0.0f);
                break;
            case ITEM_SHAPE.COLUMN:
                rect.localScale = new Vector3(1.5f, 1.5f, 1);
                item_rect.localScale = new Vector3(1.5f, 1.0f, 1.0f);
                item_rect.anchoredPosition = new Vector3(12.2f, -61.0f, 0.0f);

                break;
        }

        switch (item.item_Class) // ������ ��޿� ���� �ؽ�Ʈ ���� ����
        {
            case ITEM_CLASS.LOW_LEVEL:
                item_Name_Text.color = new Color(116 / 255f, 116 / 255f, 116 / 255f, 255 / 255f);
                item_Grade_Text.color = new Color(116 / 255f, 116 / 255f, 116 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.HIGH_LEVEL:
                item_Name_Text.color = new Color(0 / 255f, 56 / 255f, 255 / 255f, 255 / 255f);
                item_Grade_Text.color = new Color(0 / 255f, 56 / 255f, 255 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.NORMAL:
                item_Name_Text.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
                item_Grade_Text.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.MAGIC:
                item_Name_Text.color = new Color(255 / 255f, 234 / 255f, 0 / 255f, 255 / 255f);
                item_Grade_Text.color = new Color(255 / 255f, 234 / 255f, 0 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.EPIC:
                item_Name_Text.color = new Color(255 / 255f, 122 / 255f, 0 / 255f, 255 / 255f);
                item_Grade_Text.color = new Color(255 / 255f, 122 / 255f, 0 / 255f, 255 / 255f);
                break;
            case ITEM_CLASS.LEGEND:
                item_Name_Text.color = new Color(47 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);
                item_Grade_Text.color = new Color(47 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);
                break;
        }


        this.gameObject.SetActive(true);

        item_Image.sprite = item.item_Sprite;
        item_Name_Text.text = item.item_Name;
        item_Grade_Text.text = item.item_Grade;
        item_Type_Text.text = item.item_real_Type;
        item_Dps_Text.text = item.item_Dps;
        item_Option_Text.text.Replace("\\n", "\n");
        item_Option_Text.text = item.item_Option;
        item_Explain_Text.text = item.item_Explain;

        switch (item.item_Type) // ������ Ÿ�Կ� ���� �ؽ�Ʈ ����
        {
            case ITEM_TYPE.WEAPON:
                item_Damage_or_Shield_Text.text = "�ʴ� ���ݷ�";
                item_Damage_Text.text = "���� ���ݷ�";
                item_Damage.text = item.item_Damage;
                item_Attack_Speed_Text.text = "�ʴ� ���� �ӵ�";
                item_Attack_Speed.text = item.item_Attack_Speed;
                break;
            default:
                item_Damage_or_Shield_Text.text = "��";
                item_Damage_Text.text = null;
                item_Damage.text = null;
                item_Attack_Speed_Text.text = null;
                item_Attack_Speed.text = null;
                break;
        }
    }

    public void Item_Explain_Off()
    {
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {

    }
}

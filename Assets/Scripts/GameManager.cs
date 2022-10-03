using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ӸŴ��� ��ũ��Ʈ
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject inventory_UI; 

    public Item_ExplainWindow item_Explain_window; // ������ ���� â

    void Start()
    {

    }

    void Keyboard_Shortcut() // ����Ű
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            bool active = inventory_UI.activeSelf ? false : true;

            inventory_UI.SetActive(active);
        }
    }

    void Update()
    {
        Keyboard_Shortcut();
    }
}

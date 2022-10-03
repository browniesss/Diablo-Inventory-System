using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임매니저 스크립트
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject inventory_UI; 

    public Item_ExplainWindow item_Explain_window; // 아이템 툴팁 창

    void Start()
    {

    }

    void Keyboard_Shortcut() // 단축키
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

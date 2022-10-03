using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 스크립트
public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;

    Rigidbody rigid;
    Animator anim;

    [SerializeField]
    private GameObject scanObject;

    [SerializeField]
    private Inventory inventory;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x == 0 && z == 0)
        {
            anim.SetBool("Move", false);
            anim.SetBool("Idle", true);
            return;
        }

        anim.SetBool("Move", true);
        anim.SetBool("Idle", false);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(x, 0, z)), Time.deltaTime * rotateSpeed);

        Vector3 velocity = new Vector3(x, 0, z) * moveSpeed;

        rigid.velocity = velocity;
    }

    void Item_Acquire()
    {
        if (Input.GetKeyDown(KeyCode.F) && scanObject != null) // 아이템 구에 붙어서 f키(상호작용)를 누르면 아이템을 획득함.
        {
            inventory.Item_Add(scanObject.GetComponent<ItemSphere>().item);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ItemSphere")
            scanObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ItemSphere")
            scanObject = null;
    }

    void Update()
    {
        Move();

        Item_Acquire();
    }
}

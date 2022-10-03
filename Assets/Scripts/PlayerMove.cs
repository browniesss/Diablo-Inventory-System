using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� ��ũ��Ʈ
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
        if (Input.GetKeyDown(KeyCode.F) && scanObject != null) // ������ ���� �پ fŰ(��ȣ�ۿ�)�� ������ �������� ȹ����.
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

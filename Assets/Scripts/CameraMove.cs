using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�޶� ������ ��ũ��Ʈ
// �����ϰ� �÷��̾ ����ٴ�.
public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 offset;

    void Target_Follow()
    {
        if (!target)
            return;

        this.transform.position = target.transform.position + offset;
    }

    void Update()
    {
        Target_Follow();
    }
}

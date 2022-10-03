using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라 움직임 스크립트
// 간단하게 플레이어를 따라다님.
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

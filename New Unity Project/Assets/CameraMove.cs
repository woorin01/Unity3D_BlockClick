using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera camera;
    private Vector3 gap;
    private Quaternion targetRotation;

    void Start()
    {
        
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        camera.transform.position += camera.transform.forward * y;
        camera.transform.position += camera.transform.right * x;

        gap.x += Input.GetAxis("Mouse Y") * 1.5f * -1;
        gap.y += Input.GetAxis("Mouse X") * 1.5f;

        // 카메라 회전범위 제한.
        gap.x = Mathf.Clamp(gap.x, -90f, 90f);
        // 회전 값을 변수에 저장.
        targetRotation = Quaternion.Euler(gap);

        // 카메라벡터 객체에 Axis객체의 x,z회전 값을 제외한 y값만을 넘긴다.
        Quaternion q = targetRotation;
        camera.transform.rotation = q;
    }
}

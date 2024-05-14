using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 5f; //자동차의 이동 속도
    public float rotationSpeed = 100f; //자동차의 회전 속도

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //자동차에 대한 선택 여부 확인
        if (!gameObject.CompareTag("Player"))
        {
            //선택되지 않음->비활성화
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        //선택된 자동차에 대해서만 입력 처리
        if (gameObject.activeSelf)
        {
            //이동 입력 처리
            float moveInput = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.forward * moveInput * moveSpeed;
            rb.velocity = moveDirection;

            //회전 입력 처리
            float rotationInput = Input.GetAxis("Horizontal");
            float rotation = rotationInput * rotationSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}


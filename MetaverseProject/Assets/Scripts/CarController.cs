using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 5f; //�ڵ����� �̵� �ӵ�
    public float rotationSpeed = 100f; //�ڵ����� ȸ�� �ӵ�

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //�ڵ����� ���� ���� ���� Ȯ��
        if (!gameObject.CompareTag("Player"))
        {
            //���õ��� ����->��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        //���õ� �ڵ����� ���ؼ��� �Է� ó��
        if (gameObject.activeSelf)
        {
            //�̵� �Է� ó��
            float moveInput = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.forward * moveInput * moveSpeed;
            rb.velocity = moveDirection;

            //ȸ�� �Է� ó��
            float rotationInput = Input.GetAxis("Horizontal");
            float rotation = rotationInput * rotationSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}


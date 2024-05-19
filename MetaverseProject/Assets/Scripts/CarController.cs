using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 5f; //�ڵ����� �̵� �ӵ�
    public float rotationSpeed = 100f; //�ڵ����� ȸ�� �ӵ�
    private string selectedCarTag; //���ǿ��� ���õ� �ڵ����� �±�

    private Rigidbody rb;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float suspensionHeight = 0.8f; //���ϴ� ������� ����

    void Start()
    {
        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; //���� ���� ���
        rb.mass = 1500f; //�ڵ����� ���� ����
        rb.drag = 0.5f; //���� ���� ����
        rb.angularDrag = 0.5f; //���ӵ��� ���� ���� ���� ����

        frontLeftWheel.suspensionDistance = suspensionHeight;
        frontRightWheel.suspensionDistance = suspensionHeight;
        rearLeftWheel.suspensionDistance = suspensionHeight;
        rearRightWheel.suspensionDistance = suspensionHeight; //������� ���� ����

        //�ڵ����� ���� ���� ���� Ȯ��
        if (!gameObject.CompareTag(selectedCarTag))
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


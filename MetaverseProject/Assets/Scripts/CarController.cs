using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 8f; //�ڵ����� �̵� �ӵ�
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

    void Update()
    {

        //���õ� �ڵ����� ���ؼ��� �Է� ó��
        if (gameObject.activeSelf)
        {
            //�̵� �Է� ó��
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Vector3 moveDirection = transform.forward * moveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                Vector3 moveDirection = -transform.forward * moveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);
            }

            //ȸ�� �Է� ó��
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                float rotation = -rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                float rotation = rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }

        }
    }
}


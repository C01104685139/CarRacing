using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10f; //�ڵ����� �̵� �ӵ�
    public float rotationSpeed = 50f; //�ڵ����� ȸ�� �ӵ�
    private string selectedCarTag; //���ǿ��� ���õ� �ڵ����� �±�

    private Rigidbody rb;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float suspensionHeight; //���ϴ� ������� ����

    private float currentYAngle;
    private float yAngleVelocity;
    private float rotationSmoothTime = 0.1f; // ȸ�� ���� �ð�

    private bool stopMovingAtStart; // ���� �� ���� �� �ڵ��� ������ ����

    void Start()
    {
        stopMovingAtStart = false;

        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        suspensionHeight = 0.1f;

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
        // ���� �� ó�� ���� �� �ڵ��� ������ ����
        if (!stopMovingAtStart)
        {
            return;
        }

        //���õ� �ڵ����� ���ؼ��� �Է� ó��
        if (gameObject.activeSelf)
        {
            //�̵� �Է� ó��
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //�� ����Ű
            if (Input.GetKey(KeyCode.UpArrow)) {
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    //���� �̵�
                    Vector3 moveDirection = (transform.forward + -transform.right).normalized * moveSpeed;
                    float rotation = -rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.RightArrow)) {
                    //��� �̵�
                    Vector3 moveDirection = (transform.forward + transform.right).normalized * moveSpeed;
                    float rotation = rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else {
                    Vector3 moveDirection = transform.forward * moveSpeed;
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
            }
            //�� ����Ű
            else if (Input.GetKey(KeyCode.DownArrow)) {
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    //���� �̵�
                    Vector3 moveDirection = (-transform.forward + -transform.right).normalized * moveSpeed;
                    float rotation = -rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.RightArrow)) {
                    //�Ͽ� �̵�
                    Vector3 moveDirection = (-transform.forward + transform.right).normalized * moveSpeed;
                    float rotation = rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else {
                    Vector3 moveDirection = -transform.forward * moveSpeed;
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
            }
            //�� ����Ű
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                float rotation = -rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
            //�� ����Ű
            else if (Input.GetKey(KeyCode.RightArrow)) {
                float rotation = rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        }
    }

    // ���� �� ó�� ���� �� �ڵ��� ������ ���� (ī��Ʈ�ٿ� ���Ŀ� ����)
    public void StartMoving(bool status)
    {
        stopMovingAtStart = status;
    }
}


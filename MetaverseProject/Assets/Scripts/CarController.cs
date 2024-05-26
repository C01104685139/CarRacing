using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed; //�ڵ����� �̵� �ӵ�
    public float rotationSpeed; //�ڵ����� ȸ�� �ӵ�
    public float maxSpeed; //�ڵ����� �ִ� �ӵ�
    public float accelerationFactor; //���� ���
    private string selectedCarTag; //���ǿ��� ���õ� �ڵ����� �±�

    private Rigidbody rb;

    private float upKeyTime;
    private float downKeyTime;
    private float leftKeyTime;
    private float rightKeyTime;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float suspensionHeight; //���ϴ� ������� ����

    private float currentYAngle;
    private float yAngleVelocity;
    private float rotationSmoothTime = 0.1f; // ȸ�� ���� �ð�

    private bool stopMovingAtStart; // ���� �� ���� �� �ڵ��� ������ ����
    
    public float upDrag = 1.5f;
    float dragSaved;
    public bool isShift = false;
    public float slowSpeed = 100f;

    Vector3 firstPos;

    void Start()
    {
        firstPos=transform.position;
        stopMovingAtStart = false;

        moveSpeed = 10f;
        rotationSpeed = 30f;
        maxSpeed = 15f;
        accelerationFactor = 2f;

        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        suspensionHeight = 0.1f;

        rb = GetComponent<Rigidbody>();
        //playerTrail = GetComponent<PlayerTrail>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; //���� ���� ���
        rb.mass = 2000f; //�ڵ����� ���� ����
        rb.drag = 0.5f; //���� ���� ����
        rb.angularDrag = 0.5f; //���ӵ��� ���� ���� ���� ����

        frontLeftWheel.suspensionDistance = suspensionHeight;
        frontRightWheel.suspensionDistance = suspensionHeight;
        rearLeftWheel.suspensionDistance = suspensionHeight;
        rearRightWheel.suspensionDistance = suspensionHeight; //������� ���� ����

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);

        dragSaved = rb.drag;

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
            // ����Ű�� ���� �ð� ����
            if (Input.GetKey(KeyCode.UpArrow))
                upKeyTime += Time.deltaTime;
            else
                upKeyTime = 0;

            if (Input.GetKey(KeyCode.DownArrow))
                downKeyTime += Time.deltaTime;
            else
            downKeyTime = 0;

            if (Input.GetKey(KeyCode.LeftArrow))
                leftKeyTime += Time.deltaTime;
            else
                leftKeyTime = 0;

            if (Input.GetKey(KeyCode.RightArrow))
                rightKeyTime += Time.deltaTime;
            else
                rightKeyTime = 0;

            float currentMoveSpeed = moveSpeed + Mathf.Min(upKeyTime, downKeyTime) * accelerationFactor;
            currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, moveSpeed, maxSpeed);

            //rb.velocity = transform.forward * moveSpeed * 5;
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Force);

            //�̵� �Է� ó��
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //�� ����Ű
            if (Input.GetKey(KeyCode.UpArrow)) {
                
                //�� ����Ű ���� �� ���ӵ� ����
                Vector3 moveDirection = transform.forward * currentMoveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);

                if (Input.GetKey(KeyCode.LeftArrow)) {
                    //���� �̵�
                    Vector3 upleft_moveDirection = (transform.forward + -transform.right).normalized * moveSpeed;
                    float rotation = -rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.RightArrow)) {
                    //��� �̵�
                    Vector3 upright_moveDirection = (transform.forward + transform.right).normalized * moveSpeed;
                    float rotation = rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else {
                    Vector3 up_moveDirection = transform.forward * moveSpeed;
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
            }
            //�� ����Ű
            else if (Input.GetKey(KeyCode.DownArrow)) {

                //�� ����Ű ���� �� ���ӵ� ����
                Vector3 moveDirection = -transform.forward * currentMoveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);

                if (Input.GetKey(KeyCode.LeftArrow)) {
                    //���� �̵�
                    Vector3 downleft_moveDirection = (-transform.forward + -transform.right).normalized * moveSpeed;
                    float rotation = -rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.RightArrow)) {
                    //�Ͽ� �̵�
                    Vector3 downright_moveDirection = (-transform.forward + transform.right).normalized * moveSpeed;
                    float rotation = rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else {
                    Vector3 down_moveDirection = -transform.forward * moveSpeed;
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
            }
            //�� ����Ű
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                //�� ����Ű�� ������ ������ ���ӵ� ����
                Vector3 moveDirection = -transform.right * currentMoveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);

                float rotation = -rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
            //�� ����Ű
            else if (Input.GetKey(KeyCode.RightArrow)) {
                //�� ����Ű�� ������ ������ ���ӵ� ����
                Vector3 moveDirection = -transform.forward * currentMoveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);

                float rotation = rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow))
            {
                isShift = true;
                rb.drag = upDrag;
                //playerTrail.DriftDraw();
                rb.AddForce(-transform.right * (moveSpeed * 100 - (slowSpeed * 100)) * Time.deltaTime);
                transform.Rotate(0, -1, 0);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow))
            {
                isShift = true;
                rb.drag = upDrag;
                //playerTrail.DriftDraw();
                rb.AddForce(transform.right * (moveSpeed * 100 - (slowSpeed * 100)) * Time.deltaTime);
                transform.Rotate(0, 1, 0);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isShift = false;
                rb.drag = dragSaved; // �ٽ� ���� drag��
                //playerTrail.DriftRemove();
            }
        }
    }

    // ���� �� ó�� ���� �� �ڵ��� ������ ���� (ī��Ʈ�ٿ� ���Ŀ� ����)
    public void StartMoving(bool status)
    {
        stopMovingAtStart = status;
    }

    private void OnTriggerEnter(Collider other){
        if (other.tag == "Ground") transform.position = firstPos; 
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10f; //자동차의 이동 속도
    public float rotationSpeed = 50f; //자동차의 회전 속도
    private string selectedCarTag; //대기실에서 선택된 자동차의 태그

    private Rigidbody rb;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float suspensionHeight; //원하는 서스펜션 높이

    private float currentYAngle;
    private float yAngleVelocity;
    private float rotationSmoothTime = 0.1f; // 회전 보간 시간

    private bool stopMovingAtStart; // 게임 맵 시작 시 자동차 움직임 제한

    void Start()
    {
        stopMovingAtStart = false;

        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        suspensionHeight = 0.1f;

        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; //보간 설정 사용
        rb.mass = 1500f; //자동차의 질량 조정
        rb.drag = 0.5f; //공기 저항 설정
        rb.angularDrag = 0.5f; //각속도에 대한 공기 저항 설정

        frontLeftWheel.suspensionDistance = suspensionHeight;
        frontRightWheel.suspensionDistance = suspensionHeight;
        rearLeftWheel.suspensionDistance = suspensionHeight;
        rearRightWheel.suspensionDistance = suspensionHeight; //서스펜션 높이 설정

        //자동차에 대한 선택 여부 확인
        if (!gameObject.CompareTag(selectedCarTag))
        {
            //선택되지 않음->비활성화
            gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        // 게임 맵 처음 시작 시 자동차 움직임 제한
        if (!stopMovingAtStart)
        {
            return;
        }

        //선택된 자동차에 대해서만 입력 처리
        if (gameObject.activeSelf)
        {
            //이동 입력 처리
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //상 방향키
            if (Input.GetKey(KeyCode.UpArrow)) {
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    //상좌 이동
                    Vector3 moveDirection = (transform.forward + -transform.right).normalized * moveSpeed;
                    float rotation = -rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.RightArrow)) {
                    //상우 이동
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
            //하 방향키
            else if (Input.GetKey(KeyCode.DownArrow)) {
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    //하좌 이동
                    Vector3 moveDirection = (-transform.forward + -transform.right).normalized * moveSpeed;
                    float rotation = -rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.RightArrow)) {
                    //하우 이동
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
            //좌 방향키
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                float rotation = -rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
            //우 방향키
            else if (Input.GetKey(KeyCode.RightArrow)) {
                float rotation = rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        }
    }

    // 게임 맵 처음 시작 시 자동차 움직임 제한 (카운트다운 이후에 동작)
    public void StartMoving(bool status)
    {
        stopMovingAtStart = status;
    }
}


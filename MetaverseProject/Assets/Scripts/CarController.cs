using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 8f; //자동차의 이동 속도
    public float rotationSpeed = 100f; //자동차의 회전 속도
    private string selectedCarTag; //대기실에서 선택된 자동차의 태그

    private Rigidbody rb;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float suspensionHeight = 0.8f; //원하는 서스펜션 높이


    void Start()
    {
        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");

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

        //선택된 자동차에 대해서만 입력 처리
        if (gameObject.activeSelf)
        {
            //이동 입력 처리
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

            //회전 입력 처리
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


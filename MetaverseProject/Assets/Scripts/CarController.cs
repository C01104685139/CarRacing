using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed; //자동차의 이동 속도
    public float rotationSpeed; //자동차의 회전 속도
    public float maxSpeed; //자동차의 최대 속도
    public float accelerationFactor; //가속 계수
    private string selectedCarTag; //대기실에서 선택된 자동차의 태그

    private Rigidbody rb;

    private float upKeyTime;
    private float downKeyTime;
    private float leftKeyTime;
    private float rightKeyTime;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public float suspensionHeight; //원하는 서스펜션 높이

    private float currentYAngle;
    private float yAngleVelocity;
    private float rotationSmoothTime = 0.1f; // 회전 보간 시간

    private bool stopMovingAtStart; // 게임 맵 시작 시 자동차 움직임 제한
    
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
        rb.interpolation = RigidbodyInterpolation.Interpolate; //보간 설정 사용
        rb.mass = 2000f; //자동차의 질량 조정
        rb.drag = 0.5f; //공기 저항 설정
        rb.angularDrag = 0.5f; //각속도에 대한 공기 저항 설정

        frontLeftWheel.suspensionDistance = suspensionHeight;
        frontRightWheel.suspensionDistance = suspensionHeight;
        rearLeftWheel.suspensionDistance = suspensionHeight;
        rearRightWheel.suspensionDistance = suspensionHeight; //서스펜션 높이 설정

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);

        dragSaved = rb.drag;

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
            // 방향키가 눌린 시간 추적
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

            //이동 입력 처리
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //상 방향키
            if (Input.GetKey(KeyCode.UpArrow)) {
                
                //상 방향키 누를 때 가속도 적용
                Vector3 moveDirection = transform.forward * currentMoveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);

                if (Input.GetKey(KeyCode.LeftArrow)) {
                    //상좌 이동
                    Vector3 upleft_moveDirection = (transform.forward + -transform.right).normalized * moveSpeed;
                    float rotation = -rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.RightArrow)) {
                    //상우 이동
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
            //하 방향키
            else if (Input.GetKey(KeyCode.DownArrow)) {

                //하 방향키 누를 때 가속도 적용
                Vector3 moveDirection = -transform.forward * currentMoveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);

                if (Input.GetKey(KeyCode.LeftArrow)) {
                    //하좌 이동
                    Vector3 downleft_moveDirection = (-transform.forward + -transform.right).normalized * moveSpeed;
                    float rotation = -rotationSpeed * Time.fixedDeltaTime;
                    Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    rb.AddForce(moveDirection, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.RightArrow)) {
                    //하우 이동
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
            //좌 방향키
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                //좌 방향키를 누르고 있으면 가속도 적용
                Vector3 moveDirection = -transform.right * currentMoveSpeed;
                rb.AddForce(moveDirection, ForceMode.Acceleration);

                float rotation = -rotationSpeed * Time.fixedDeltaTime;
                Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
            //우 방향키
            else if (Input.GetKey(KeyCode.RightArrow)) {
                //우 방향키를 누르고 있으면 가속도 적용
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
                rb.drag = dragSaved; // 다시 원래 drag로
                //playerTrail.DriftRemove();
            }
        }
    }

    // 게임 맵 처음 시작 시 자동차 움직임 제한 (카운트다운 이후에 동작)
    public void StartMoving(bool status)
    {
        stopMovingAtStart = status;
    }

    private void OnTriggerEnter(Collider other){
        if (other.tag == "Ground") transform.position = firstPos; 
    }
}


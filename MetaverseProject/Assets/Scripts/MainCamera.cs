using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    private string selectedCarTag;
    public GameObject targetCar; // 따라다닐 대상
    Vector3 targetCarPosition; // 대상 위치

    private float offsetX;
    private float offsetY;
    private float offsetZ;

    private float rotationOffsetX;
    private float rotationOffsetY;
    private float rotationOffsetZ;

    public float speed = 10.0f;
    public float rotateSpeed = 10.0f;

    private Vector3 mainCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        targetCar = GameObject.FindWithTag(selectedCarTag);

        mainCameraPosition = GameObject.FindWithTag("MainCamera").transform.position;
        MapSelection();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // 카메라 위치
        targetCarPosition = new Vector3(
            targetCar.transform.position.x + offsetX,
            targetCar.transform.position.y + offsetY,
            targetCar.transform.position.z + offsetZ
            );

        // 카메라 회전
        Quaternion targetRotation = Quaternion.Euler(
            rotationOffsetX,
            targetCar.transform.eulerAngles.y + rotationOffsetY,
            rotationOffsetZ
        );

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, targetCarPosition, Time.deltaTime * speed);

        // 카메라의 회전을 부드럽게 설정
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    private void MapSelection()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (scene == "RaceScene01" || scene == "RaceScene02")
        {
            offsetX = targetCar.transform.position.x - mainCameraPosition.x;
            offsetY = -targetCar.transform.position.y + mainCameraPosition.y;
            offsetZ = -targetCar.transform.position.z + mainCameraPosition.z;

            rotationOffsetX = 25.0f;
            rotationOffsetY = 0;
            rotationOffsetZ = 0;
        } else
        {
            offsetX = -targetCar.transform.position.x + mainCameraPosition.x;
            offsetY = -targetCar.transform.position.y + mainCameraPosition.y;
            offsetZ = -targetCar.transform.position.z + mainCameraPosition.z;

            rotationOffsetX = 25.0f;
            rotationOffsetY = 90.0f;
            rotationOffsetZ = 0;
        }
    }
}

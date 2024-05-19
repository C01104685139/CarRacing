using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private string selectedCarTag;
    public GameObject targetCar; // 따라다닐 대상
    Vector3 targetCarPosition; // 대상 위치

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public float speed = 10.0f;

    private Vector3 mainCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        targetCar = GameObject.FindWithTag(selectedCarTag);

        mainCameraPosition = GameObject.FindWithTag("MainCamera").transform.position;
        offsetX = targetCar.transform.position.x - mainCameraPosition.x;
        offsetY = - targetCar.transform.position.y + mainCameraPosition.y;
        offsetZ = - targetCar.transform.position.z + mainCameraPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        targetCarPosition = new Vector3(
            targetCar.transform.position.x + offsetX,
            targetCar.transform.position.y + offsetY,
            targetCar.transform.position.z + offsetZ
            );

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, targetCarPosition, Time.deltaTime * speed);
    }
}

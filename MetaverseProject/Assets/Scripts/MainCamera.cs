using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private string selectedCarTag;
    public GameObject targetCar; // ����ٴ� ���
    Vector3 targetCarPosition; // ��� ��ġ

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

        // ī�޶��� �������� �ε巴�� �ϴ� �Լ�(Lerp)
        transform.position = Vector3.Lerp(transform.position, targetCarPosition, Time.deltaTime * speed);
    }
}

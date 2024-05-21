using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    private string selectedCarTag;
    public GameObject targetCar; // ����ٴ� ���
    private Vector3 cameraPosition;

    public float distance;
    public float height;
    public float damping;
    public bool smoothRotation;
    public float rotationDamping;

    // Start is called before the first frame update
    void Start()
    {
        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        targetCar = GameObject.FindWithTag(selectedCarTag);

        distance = 25.0f;
        height = 25.0f;
        damping = 1.0f;
        smoothRotation = true;
        rotationDamping = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        cameraPosition = targetCar.transform.position + targetCar.transform.TransformDirection(0, height, -distance);

        // ī�޶� ��ġ�� �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * damping);

        if (smoothRotation)
        {
            // ī�޶� ȸ���� �ε巴�� ��ǥ ������ ���ϰ� ����
            Quaternion wantedRotation = Quaternion.LookRotation(targetCar.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }
        else
        {
            // ī�޶� ȸ���� ��ǥ ������ ���ϰ� ����
            transform.LookAt(targetCar.transform);
        }
    }
}

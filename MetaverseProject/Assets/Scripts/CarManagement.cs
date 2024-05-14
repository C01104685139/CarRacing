using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManagement : MonoBehaviour
{
    public string selectedCarTag; //���ǿ��� ���õ� �ڵ����� �±�
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        //��� �ڵ��� GameObject �迭 �ݺ�
        foreach (GameObject car in cars)
        {
            //���� �ڵ����� �±װ� ���õ� �ڵ����� �±׿� ��ġ�ϴ��� Ȯ��
            if (car.CompareTag(selectedCarTag))
            {
                //���õ� �ڵ���->Ȱ��ȭ
                car.SetActive(true);
            }
            else
            {
                //���õ��� ����->��Ȱ��ȭ
                car.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

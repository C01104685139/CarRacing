using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManagement : MonoBehaviour
{
    public string selectedCarTag; //���õ� ������ �±� ����

    void Start()
    {
        //PlayerPrefs���� ���õ� ������ �±׸� ������
        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");

        //��� ���� ����� �Լ�
        HideAllCars();

        //���õ� ���� ���̴� �Լ�
        ShowSelectedCar();
    }

    void HideAllCars()
    {
        //"Car" �±׸� ���� ��� ������ �迭�� ������
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        //�迭�� �� ������ ���� �ݺ�
        foreach (GameObject car in cars)
        {
            //���� ��Ȱ��ȭ
            car.SetActive(false);
        }
    }

    void ShowSelectedCar()
    {
        //���õ� ������ �±� ���� ���� ������Ʈ ������
        GameObject selectedCar = GameObject.FindGameObjectWithTag(selectedCarTag);

        //���õ� ������ �����ϴ� ���
        if (selectedCar != null)
        {
            //���õ� ���� Ȱ��ȭ
            selectedCar.SetActive(true);
        }
    }
}


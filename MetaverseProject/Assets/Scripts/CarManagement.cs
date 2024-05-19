using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManagement : MonoBehaviour
{
    public string selectedCarTag; //선택된 차량의 태그 저장

    void Start()
    {
        //PlayerPrefs에서 선택된 차량의 태그를 가져옴
        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");

        //모든 차량 숨기는 함수
        HideAllCars();

        //선택된 차량 보이는 함수
        ShowSelectedCar();
    }

    void HideAllCars()
    {
        //"Car" 태그를 가진 모든 차량을 배열로 가져옴
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        //배열의 각 차량에 대해 반복
        foreach (GameObject car in cars)
        {
            //차량 비활성화
            car.SetActive(false);
        }
    }

    void ShowSelectedCar()
    {
        //선택된 차량의 태그 가진 게임 오브젝트 가져옴
        GameObject selectedCar = GameObject.FindGameObjectWithTag(selectedCarTag);

        //선택된 차량이 존재하는 경우
        if (selectedCar != null)
        {
            //선택된 차량 활성화
            selectedCar.SetActive(true);
        }
    }
}


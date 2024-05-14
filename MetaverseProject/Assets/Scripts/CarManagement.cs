using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManagement : MonoBehaviour
{
    public string selectedCarTag; //대기실에서 선택된 자동차의 태그
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        //모든 자동차 GameObject 배열 반복
        foreach (GameObject car in cars)
        {
            //현재 자동차의 태그가 선택된 자동차의 태그와 일치하는지 확인
            if (car.CompareTag(selectedCarTag))
            {
                //선택된 자동차->활성화
                car.SetActive(true);
            }
            else
            {
                //선택되지 않음->비활성화
                car.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

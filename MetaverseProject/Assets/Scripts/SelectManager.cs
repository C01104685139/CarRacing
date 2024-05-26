using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class SelectManager : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public void YesLogout() //로그아웃 Yes버튼 클릭 시 로그아웃
    {
        auth.SignOut();
    }

    public void selectCar() // 해당 차의 태그 저장
    {
        GameObject carButton = EventSystem.current.currentSelectedGameObject;
        string carTag = carButton.tag;
        
        PlayerPrefs.SetString("selectedCarTag", carTag);
        Debug.Log("선택된 차 : " + carTag);
    }

    public void selectMode() // 모드 선택
    {
        GameObject modeButton = EventSystem.current.currentSelectedGameObject;
        string modeTag = modeButton.tag;

        PlayerPrefs.SetString("selectedModeTag", modeTag);
        Debug.Log("선택된 모드 : " + modeTag);
    }

    public void race1SceneChange() //race1으로 씬 변경
    {
        SceneManager.LoadScene("RaceScene01");
    }
    public void race2SceneChange() //race2으로 씬 변경
    {
        SceneManager.LoadScene("RaceScene02");
    }
    public void race3SceneChange() //race3으로 씬 변경
    {
        SceneManager.LoadScene("RaceScene03");
    }
}

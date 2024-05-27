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

    void Start ()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void YesLogout() //�α׾ƿ� Yes��ư Ŭ�� �� �α׾ƿ�
    {
        auth.SignOut();
        SceneManager.LoadScene("LoginScene");
    }

    public void selectCar() // �ش� ���� �±� ����
    {
        GameObject carButton = EventSystem.current.currentSelectedGameObject;
        string carTag = carButton.tag;
        
        PlayerPrefs.SetString("selectedCarTag", carTag);
        Debug.Log("���õ� �� : " + carTag);
    }

    public void selectMode() // ��� ����
    {
        GameObject modeButton = EventSystem.current.currentSelectedGameObject;
        string modeTag = modeButton.tag;

        PlayerPrefs.SetString("selectedModeTag", modeTag);
        Debug.Log("���õ� ��� : " + modeTag);
    }

    public void race1SceneChange() //race1���� �� ����
    {
        SceneManager.LoadScene("RaceScene01");
    }
    public void race2SceneChange() //race2���� �� ����
    {
        SceneManager.LoadScene("RaceScene02");
    }
    public void race3SceneChange() //race3���� �� ����
    {
        SceneManager.LoadScene("RaceScene03");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class SelectManager : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    

    public void YesLogout() //�α׾ƿ� Yes��ư Ŭ�� �� �α׾ƿ�
    {
        auth.SignOut();
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

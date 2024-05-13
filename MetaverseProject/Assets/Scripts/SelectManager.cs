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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void YesLogout() //로그아웃 Yes버튼 클릭 시 로그아웃
    {
        auth.SignOut();
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

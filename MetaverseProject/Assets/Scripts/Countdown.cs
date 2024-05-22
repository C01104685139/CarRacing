using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Countdown : MonoBehaviour
{
    public int countdowntime = 3;
    public TMP_Text countdown;

    void OnEnable() // 씬 처음 호출 시 로드
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() // 씬 로드 시 호출되는 메서드 해제
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        CarController carController = GameObject.Find("Devil").GetComponent<CarController>();
        carController.StartMoving(false);

        while (countdowntime > 0)
        {
            countdown.text = countdowntime.ToString();
            SoundController.instance.PlaySound("CountBeep");//스타트 효과음
            yield return new WaitForSeconds(1f);

            countdowntime--;
        }
        countdown.text = "Go!";
        SoundController.instance.PlaySound("StartBeep");//스타트 효과음
        RectTransform rectTransform = countdown.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(150, 0);
        yield return new WaitForSeconds(1f);
        countdown.gameObject.SetActive(false);

        carController.StartMoving(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Countdown : MonoBehaviour
{
    public int countdowntime = 3;
    public TMP_Text countdown;
    private string selectedCarTag;

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
        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        CarController carController = GameObject.Find(selectedCarTag).GetComponent<CarController>();
        carController.StartMoving(false);

        ShadowCarController shadowCarController = GameObject.FindWithTag("ShadowCar").GetComponent<ShadowCarController>();

        RaceManager raceManager = GameObject.Find("RaceManager").GetComponent<RaceManager>();

        while (countdowntime > 0)
        {
            countdown.text = countdowntime.ToString();
            SoundController.instance.PlaySound("CountBeep");//스타트 효과음
            yield return new WaitForSeconds(1f);

            countdowntime--;
        }
        countdown.text = "Go!";
        RectTransform rectTransform = countdown.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(150, 0);
        SoundController.instance.PlaySound("StartBeep");//스타트 효과음
        yield return new WaitForSeconds(1f);
        countdown.gameObject.SetActive(false);

        carController.StartMoving(true);
        shadowCarController.stopMoving = false;
        raceManager.StartRace();
    }
}

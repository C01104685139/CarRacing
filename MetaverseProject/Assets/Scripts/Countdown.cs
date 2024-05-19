using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Countdown : MonoBehaviour
{
    public int countdowntime = 3;
    public TMP_Text countdown;

    void OnEnable() // �� ó�� ȣ�� �� �ε�
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() // �� �ε� �� ȣ��Ǵ� �޼��� ����
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        while (countdowntime > 0)
        {
            countdown.text = countdowntime.ToString();
            SoundController.instance.PlaySound("CountBeep");//��ŸƮ ȿ����
            yield return new WaitForSeconds(1f);

            countdowntime--;
        }
        countdown.text = "Go!";
        SoundController.instance.PlaySound("StartBeep");//��ŸƮ ȿ����
        RectTransform rectTransform = countdown.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(150, 0);
        yield return new WaitForSeconds(1f);
        countdown.gameObject.SetActive(false);
    }
}

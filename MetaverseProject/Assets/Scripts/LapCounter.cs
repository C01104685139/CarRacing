using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RaceManager : MonoBehaviour
{
    public string checkpointTag = "Checkpoint";
    private int currentLapCount = 0;
    private int totalLapCount = 3;
    private int lastCheckpointIndex = -1;
    private bool isRaceFinished = false;

    public GameObject finishGamePanel;
    private bool isreplayed;
    private bool isfinished;

    public TMP_Text timeText;
    private float racingTime;
    private bool raceStarted;

    void Start()
    {
        isreplayed = false;
        isfinished = false;
        raceStarted = false;

        racingTime = 0;
    }

    void Update()
    {
        // �ð� ����
        if (raceStarted && !isRaceFinished)
        {
            racingTime += Time.deltaTime;
            timeText.text = "Time : " + float.Parse(racingTime.ToString("F2"));
        }

        // �ٽ� �ϱ� ��ư Ŭ��
        if (isreplayed)
        {
            SceneManager.LoadScene("SampleScene");
        }

        // ���� ���� ��ư Ŭ��
        if (isfinished)
        {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }

    /// <summary>
    /// �ǴϽ� ���ο� �������� �� ȣ��Ǵ� �̺�Ʈ �Լ�
    /// </summary>
    /// <param name="other">�浹�� �ݶ��̴� ��ü</param>
    private void OnTriggerEnter(Collider other)
    {
        //�ǴϽ� ���� �±׿� ��ġ�ϴ� ���
        if (other.CompareTag(checkpointTag)) {
            int checkpointIndex = System.Array.IndexOf(other.GetComponentsInChildren<Transform>(), other.transform);
            
            if (checkpointIndex == lastCheckpointIndex + 1) {
                currentLapCount++;
                lastCheckpointIndex = checkpointIndex;

                // ���� �� ���� �� �� �� �̻��� ��� ���̽� ����
                if (currentLapCount >= totalLapCount)
                {
                    FinishRace();
                }
            }
        }
    }

    private void FinishRace()
    {
        //���̽� ���� ���� ����
        isRaceFinished = true;
        //���̽� ���� �޽��� ���
        Debug.Log("Race finished!");
        //���� ���ϰ� �ϴ� �ڵ� �߰�

        // ����� or ���� ����
        finishGamePanel.SetActive(true);
    }

    public void ReplayButton()
    {
        isreplayed = true;
    }

    public void FinishButton()
    {
        isfinished = true;
    }

    public void StartRace()
    {
        raceStarted = true;
    }
}


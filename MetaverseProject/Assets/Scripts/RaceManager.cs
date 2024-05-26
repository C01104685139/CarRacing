using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase;
using TMPro;
using System.Linq;
using System.Collections.Generic;
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

    private DatabaseReference reference;
    private string userEmail;

    public TMP_Text first;
    public TMP_Text second;
    public TMP_Text third;
    public TMP_Text fourth;
    public TMP_Text fifth;

    void Start()
    {
        isreplayed = false;
        isfinished = false;
        raceStarted = false;

        racingTime = 0;

        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Update()
    {
        // �ð� ����
        if (raceStarted && !isRaceFinished)
        {
            racingTime += Time.deltaTime;
            timeText.text = "Time : " + racingTime.ToString("F2");
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


        // ���̽� ��� ����
        // ������ ������ : ����� �̸���, ���̽� �� ��ȣ, �ð� ���
        SaveData();

        // ���� �ؽ�Ʈ ����
        UpdateRank();

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

    private void SaveData()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            // ���� ������� �̸���
            userEmail = user.Email.Replace(".", "_");
            Debug.Log("User Email: " + userEmail);
        }
        else
        {
            Debug.Log("No user information");
        }

        string mapName = SceneManager.GetActiveScene().name;
        float currentTime = float.Parse(racingTime.ToString("F2"));

        // ������ �ε�
        FirebaseDatabase.DefaultInstance.GetReference("Records").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error : " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // �����ͺ��̽��� ���� ���� ��� ���ο� ��� �߰�
                if (!snapshot.Exists)
                {
                    reference.Child("Records").Child(userEmail).Child(mapName).Push().SetRawJsonValueAsync(currentTime.ToString());
                    return;
                }

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string savedUser = userSnapshot.Key;

                    if (savedUser == userEmail) // ���� ����ڰ� ����Ǿ� ���� ��
                    {
                        foreach (DataSnapshot mapSnapshot in userSnapshot.Children)
                        {
                            string savedMapName = mapSnapshot.Key;

                            if (savedMapName == mapName) // ���� �� ����� ����Ǿ� ���� ��
                            {
                                foreach (DataSnapshot recordSnapshot in mapSnapshot.Children)
                                {
                                    float savedRecord = float.Parse(recordSnapshot.Value.ToString());

                                    Debug.Log("Map: " + savedMapName + ", User: " + savedUser + ", Time Record: " + savedRecord);
                                    Debug.Log("savedTime: " + savedRecord);
                                    Debug.Log("currentTime: " + currentTime);

                                    if (currentTime < savedRecord)
                                    { // ����� �� ���� �� ����
                                        reference.Child("Records").Child(userEmail).Child(mapName).Child(recordSnapshot.Key).SetValueAsync(currentTime);
                                    }
                                }
                            }
                            else
                            { // ����ڰ� �ش� �ʿ��� ó�� ����� ���� (���� ����)
                                reference.Child("Records").Child(userEmail).Child(mapName).Push().SetRawJsonValueAsync(currentTime.ToString());
                            }
                        }
                    }
                    else
                    { // ������� ����� ���� (���� ����)
                        reference.Child("Records").Child(userEmail).Child(mapName).Push().SetRawJsonValueAsync(currentTime.ToString());
                    }
                }
            }
        });
    }

    private void UpdateRank()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Records").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error : " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // �ҷ��� ���� ��� ������ ����Ʈ
                List<RaceRecords> recordList = new List<RaceRecords>();

                // ������ ��� ��������
                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string userEmail = userSnapshot.Key;

                    foreach (DataSnapshot mapSnapshot in userSnapshot.Children)
                    {
                        string mapName = mapSnapshot.Key;

                        if (mapName == SceneManager.GetActiveScene().name) // ���� �ʿ� �ش��ϴ� ���� ��ϸ� ������
                        {
                            foreach (DataSnapshot recordSnapshot in mapSnapshot.Children)
                            {
                                float timeRecord = float.Parse(recordSnapshot.Value.ToString());

                                // ����Ʈ�� ��� �߰�
                                recordList.Add(new RaceRecords { userEmail = userEmail, mapName = mapName, timeRecord = timeRecord });
                            }
                        }
                    }
                }

                // 5�� �̱�
                var top5 = recordList.OrderBy(r => r.timeRecord).Take(5);

                string[] tmp = new string[] { "-", "-", "-", "-", "-", "-" };
                int cnt = 1;
                // ���� �ݿ�
                foreach (var user in top5)
                {
                    tmp[cnt] = cnt++ + "�� : "+ user.userEmail.Replace("_", ".") + " �� "+ user.timeRecord + "��";
                    Debug.Log("User: " + user.userEmail + ", Time: " + user.timeRecord);
                }

                first.text = tmp[1];
                second.text = tmp[2];
                third.text = tmp[3];
                fourth.text = tmp[4];
                fifth.text = tmp[5];
            }
        });
     }
}

public class RaceRecords
{
    public string userEmail { get; set; }
    public string mapName { get; set; }
    public float timeRecord { get; set; }
}
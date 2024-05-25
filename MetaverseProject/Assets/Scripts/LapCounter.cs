using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase;
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

    private DatabaseReference reference;
    private string userEmail;

    void Start()
    {
        isreplayed = false;
        isfinished = false;
        raceStarted = false;

        racingTime = 0;

        reference = FirebaseDatabase.DefaultInstance.RootReference;

        finishGamePanel.SetActive(true);
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
            SaveData();
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

        // ���̽� ��� ����
        // ������ ������ : ����� �̸���, ���̽� �� ��ȣ, �ð� ���
        SaveData();
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
}
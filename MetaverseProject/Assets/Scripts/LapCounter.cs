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
        // 시간 측정
        if (raceStarted && !isRaceFinished)
        {
            racingTime += Time.deltaTime;
            timeText.text = "Time : " + racingTime.ToString("F2");
        }

        // 다시 하기 버튼 클릭
        if (isreplayed)
        {
            SceneManager.LoadScene("SampleScene");
            SaveData();
        }

        // 게임 종료 버튼 클릭
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
    /// 피니시 라인에 도달했을 때 호출되는 이벤트 함수
    /// </summary>
    /// <param name="other">충돌한 콜라이더 객체</param>
    private void OnTriggerEnter(Collider other)
    {
        //피니시 라인 태그와 일치하는 경우
        if (other.CompareTag(checkpointTag)) {
            int checkpointIndex = System.Array.IndexOf(other.GetComponentsInChildren<Transform>(), other.transform);
            
            if (checkpointIndex == lastCheckpointIndex + 1) {
                currentLapCount++;
                lastCheckpointIndex = checkpointIndex;

                // 현재 랩 수가 총 랩 수 이상인 경우 레이스 종료
                if (currentLapCount >= totalLapCount)
                {
                    FinishRace();
                }
            }
        }
    }

    private void FinishRace()
    {
        //레이스 종료 상태 설정
        isRaceFinished = true;
        //레이스 종료 메시지 출력
        Debug.Log("Race finished!");
        //운전 못하게 하는 코드 추가

        // 재시작 or 종료 선택
        finishGamePanel.SetActive(true);

        // 레이스 결과 저장
        // 저장할 데이터 : 사용자 이메일, 레이싱 맵 번호, 시간 기록
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
            // 현재 사용자의 이메일
            userEmail = user.Email.Replace(".", "_");
            Debug.Log("User Email: " + userEmail);
        }
        else
        {
            Debug.Log("No user information");
        }

        string mapName = SceneManager.GetActiveScene().name;
        float currentTime = float.Parse(racingTime.ToString("F2"));

        // 데이터 로드
        FirebaseDatabase.DefaultInstance.GetReference("Records").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error : " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // 데이터베이스에 값이 없는 경우 새로운 기록 추가
                if (!snapshot.Exists)
                {
                    reference.Child("Records").Child(userEmail).Child(mapName).Push().SetRawJsonValueAsync(currentTime.ToString());
                    return;
                }

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string savedUser = userSnapshot.Key;

                    if (savedUser == userEmail) // 현재 사용자가 저장되어 있을 때
                    {
                        foreach (DataSnapshot mapSnapshot in userSnapshot.Children)
                        {
                            string savedMapName = mapSnapshot.Key;

                            if (savedMapName == mapName) // 현재 맵 기록이 저장되어 있을 때
                            {
                                foreach (DataSnapshot recordSnapshot in mapSnapshot.Children)
                                {
                                    float savedRecord = float.Parse(recordSnapshot.Value.ToString());

                                    Debug.Log("Map: " + savedMapName + ", User: " + savedUser + ", Time Record: " + savedRecord);
                                    Debug.Log("savedTime: " + savedRecord);
                                    Debug.Log("currentTime: " + currentTime);

                                    if (currentTime < savedRecord)
                                    { // 기록이 더 좋을 때 갱신
                                        reference.Child("Records").Child(userEmail).Child(mapName).Child(recordSnapshot.Key).SetValueAsync(currentTime);
                                    }
                                    
                                }
                            }
                            else
                            { // 사용자가 해당 맵에서 처음 기록을 세움 (새로 저장)
                                reference.Child("Records").Child(userEmail).Child(mapName).Push().SetRawJsonValueAsync(currentTime.ToString());
                            }
                        }
                    }
                    else
                    { // 사용자의 기록이 없음 (새로 저장)
                        reference.Child("Records").Child(userEmail).Child(mapName).Push().SetRawJsonValueAsync(currentTime.ToString());
                    }
                }
            }
        });
        
    }
}
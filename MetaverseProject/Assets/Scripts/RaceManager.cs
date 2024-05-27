using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RaceManager : MonoBehaviour
{
    private bool isRaceFinished = false;

    public GameObject finishGamePanel;
    private bool isreplayed;
    private bool isfinished;

    public TMP_Text timeText;
    private float racingTime;
    private bool raceStarted;

    private DatabaseReference reference;
    private string userEmail;

    private List<RaceRecords> recordList; // 불러온 주행 기록 저장할 리스트
    public TMP_Text first;
    public TMP_Text second;
    public TMP_Text third;
    public TMP_Text fourth;
    public TMP_Text fifth;

    private LapCounter lapCounter;
    private string selectedCarTag;

    void Start()
    {
        isreplayed = false;
        isfinished = false;
        raceStarted = false;
        recordList = new List<RaceRecords>();

        racingTime = 0;

        reference = FirebaseDatabase.DefaultInstance.RootReference;

        selectedCarTag = PlayerPrefs.GetString("selectedCarTag");
        lapCounter = GameObject.Find(selectedCarTag).GetComponent<LapCounter>();

    }

    void Update()
    {
        // 주행 끝났는지 확인
        if (lapCounter.isLapFinished && !isRaceFinished)
        {
            FinishRace();
        }

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

    private void FinishRace()
    {
        //레이스 종료 상태 설정
        isRaceFinished = true;

        // 레이스 결과 저장
        // 저장할 데이터 : 사용자 이메일, 레이싱 맵 번호, 시간 기록
        SaveData();

        // 순위 텍스트 조정
        UpdateRank();

        // 재시작 or 종료 선택
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

                // 데이터 기록 가져오기
                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string userEmail = userSnapshot.Key;

                    foreach (DataSnapshot mapSnapshot in userSnapshot.Children)
                    {
                        string mapName = mapSnapshot.Key;

                        if (mapName == SceneManager.GetActiveScene().name) // 현재 맵에 해당하는 맵의 기록만 가져옴
                        {
                            foreach (DataSnapshot recordSnapshot in mapSnapshot.Children)
                            {
                                float timeRecord = float.Parse(recordSnapshot.Value.ToString());

                                // 리스트에 기록 추가
                                recordList.Add(new RaceRecords { userEmail = userEmail, mapName = mapName, timeRecord = timeRecord });
                            }
                        }
                    }
                }

                // 5명 뽑기
                var top5 = recordList.OrderBy(r => r.timeRecord).Take(5);

                string[] tmp = new string[] { "-", "-", "-", "-", "-", "-" };
                int cnt = 1;
                // 순위 반영
                foreach (var user in top5)
                {
                    tmp[cnt] = cnt++ + "등 : "+ user.userEmail.Replace("_", ".") + " → "+ user.timeRecord + "초";
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using Cinemachine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;

public class ShadowCarController : MonoBehaviour
{
    private int lapCount;
    private int currentLap;
    private bool isfinished;
    private CinemachineDollyCart cart;
    private CinemachinePathBase path;
    
    private float firstRecord;
    private float totalDistance;
    private float shadowCarspeed;
    private List<RaceRecords> recordList; // �ҷ��� ���� ��� ������ ����Ʈ

    public bool stopMoving;

    // Start is called before the first frame update
    void Start()
    {
        lapCount = 4; // ������ �� + 3����
        currentLap = 0;
        isfinished = false;
        stopMoving = true;
        recordList = new List<RaceRecords>();

        cart = GameObject.FindWithTag("ShadowCar").GetComponent<CinemachineDollyCart>();
        path = cart.m_Path;
        totalDistance = path.PathLength * 3;

        // �ش� ���� 1�� ��� ��������
        loadRecord();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopMoving)
        {
            cart.m_Speed = 0f;
        }
        else
        {
            shadowCarspeed = totalDistance / firstRecord;
            cart.m_Speed = shadowCarspeed;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LapCounter") && !isfinished)
        {
            currentLap++;
            Debug.Log("shadowModeCurrentLap: " + currentLap);
            if (currentLap == lapCount)
            {
                isfinished = true;
                cart.m_Speed = 0f;
            }
        }
    }

    private void loadRecord()
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

                // 1�� �̱�
                var top1 = recordList.OrderBy(r => r.timeRecord).FirstOrDefault();

                if (top1 != null)
                {
                    firstRecord = top1.timeRecord;
                    Debug.Log("1�� ��� : "+firstRecord+"��");
                }
                else
                {
                    firstRecord = 0f;
                    Debug.Log("1�� ����� �����ϴ�");
                }
            }
        });
    }
}
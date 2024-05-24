using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public string checkpointTag = "Checkpoint";
    private int currentLapCount = 0;
    private int totalLapCount = 3;
    private int lastCheckpointIndex = -1;
    private bool isRaceFinished = false;

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
    }
}


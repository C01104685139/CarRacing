using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public string checkpointTag = "Checkpoint";
    private int currentLapCount = 0;
    private int totalLapCount = 3;
    private int lastCheckpointIndex = -1;
    private bool isRaceFinished = false;

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
    }
}


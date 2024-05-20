using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    public int currentLap = 0;
    public int totalLaps = 3;
    private bool isRaceFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentLap++;
            Debug.Log("Lap " + currentLap + " completed!");

            if (currentLap >= totalLaps)
            {
                FinishRace();
            }
        }
    }

    private void FinishRace()
    {
        isRaceFinished = true;
        Debug.Log("Race finished!");
    }
}

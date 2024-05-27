using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapCounter : MonoBehaviour
{
    private int currentLapCount;
    private int totalLapCount;
    private int tmpCount;
    public TMP_Text LapText;
    public bool isLapFinished;

    // Start is called before the first frame update
    void Start()
    {
        currentLapCount = 0;
        totalLapCount = 3;
        tmpCount = 4;

        isLapFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LapCounter"))
        {
            currentLapCount++;
            UpdateLapText();
            Debug.Log("currentLap: " + currentLapCount);
            if (currentLapCount == tmpCount)
            {
                isLapFinished = true;
            }
        }
    }

    private void UpdateLapText()
    {
        if (LapText != null)
        {
            LapText.text = "Lap: " + currentLapCount + "/" + totalLapCount;
        }
    }
}

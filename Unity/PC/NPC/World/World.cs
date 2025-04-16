using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class World : MonoBehaviour
{
    [Header("NPC")]
    public NPCManager nm;

    [Header("World Time")]
    public int CurrentTime;
    public int TimeIncrease;
    public float TimeUntilTimeIncrease;
    private float OGTimeUntilTimeIncrease;

    [Header("Text")]
    public TextMeshProUGUI TimerText;

    // Start is called before the first frame update
    void Start()
    {
        OGTimeUntilTimeIncrease = TimeUntilTimeIncrease;
    }

    // Update is called once per frame
    void Update()
    {
        TimerText.text = "Time: " + CurrentTime;
        if(CurrentTime < 24)
        {
            if(TimeUntilTimeIncrease > OGTimeUntilTimeIncrease)
            {
                CurrentTime += TimeIncrease;
                nm.CheckMovement(CurrentTime);
                nm.CheckActivity(CurrentTime);
                TimeUntilTimeIncrease = 0;
            }
            else
            {
                TimeUntilTimeIncrease += Time.deltaTime;
            }
        }
        else
        {
            CurrentTime = 0;
        }
    }
}

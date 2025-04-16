using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class World : MonoBehaviour
{
    [Header("Player")]
    public PlayerController player;

    [Header("MainCanvas")]
    public MainCanvas mainCanvas;

    [Header("Floats")]
    public float WorldTime;
    public float WorldTimeSpeed;
    public float SunSpeed;
    public float MoonSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TIME //
        DisplayTime();
    }

    void DisplayTime()
    {
        WorldTime += Time.deltaTime * WorldTimeSpeed;

        float minutes = Mathf.FloorToInt(WorldTime / 60);
        float seconds = Mathf.FloorToInt(WorldTime % 60);
        mainCanvas.TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if(minutes > 23)
        {
            WorldTime = 0;
        }
    }
}

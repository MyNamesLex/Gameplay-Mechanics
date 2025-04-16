using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatCanvas : MonoBehaviour
{
    [Header("Player")]
    public PlayerController player;

    [Header("Stats")]
    public TextMeshProUGUI PlayerSpeed;
    public TextMeshProUGUI CurrentEquipped;
    [SerializeField] private TextMeshProUGUI FPSText;
    [SerializeField] private float hudRefreshRate = 1f;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerSpeed.text = "Player Speed: " + player.rb.velocity.magnitude.ToString(); // player speed
        // FPS //

        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            FPSText.text = "FPS: " + fps;
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }
}

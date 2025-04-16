using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Buttons : MonoBehaviour
{
    public TestEnemy enemy;
    public bool ChangeBlocking;
    public bool ChangeSwinging;
    public bool ChangeStunned;
    public bool RestartScene;
    public TextMeshPro SwingingText;
    public TextMeshPro BlockingText;
    public TextMeshPro StunnedText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ChangeBlocking)
            {
                if (enemy.Blocking == false)
                {
                    BlockingText.text = "Change Blocking State To False";
                    enemy.Blocking = true;
                }
                else
                {
                    BlockingText.text = "Change Blocking State To True";
                    enemy.Blocking = false;
                }
            }

            if (ChangeSwinging)
            {
                if (enemy.Swinging == false)
                {
                    SwingingText.text = "Change Swinging State To False";
                    enemy.Swinging = true;
                }
                else
                {
                    SwingingText.text = "Change Swinging State To True";
                    enemy.Swinging = false;
                }
            }

            if (ChangeStunned)
            {
                if (enemy.Stunned == false)
                {
                    StunnedText.text = "Change Stunned State To False";
                    enemy.Stunned = true;
                }
                else
                {
                    StunnedText.text = "Change Stunned State To True";
                    enemy.Stunned = false;
                }
            }

            if (RestartScene)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}

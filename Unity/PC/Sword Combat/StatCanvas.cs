using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatCanvas : MonoBehaviour
{
    public PlayerSwordController player;
    public TextMeshProUGUI Swing;
    public TextMeshProUGUI Block;
    public TextMeshProUGUI Stunned;

    public TestEnemy enemy;
    public TextMeshProUGUI EnemySwing;
    public TextMeshProUGUI EnemyBlock;
    public TextMeshProUGUI EnemyStunned;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Block.text = "Block: " + player.Block;
        Swing.text = "Can Use Sword: " + player.CanUseSword;
        Stunned.text = "Stunned: " + player.Stunned;

        // enemy //

        EnemySwing.text = "Test Enemy Swinging: " + enemy.Swinging;
        EnemyBlock.text = "Test Enemy Blocking: " + enemy.Blocking;
        EnemyStunned.text = "Test Enemy Stunned: " + enemy.Stunned;
    }
}

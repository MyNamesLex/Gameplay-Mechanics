using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public GameObject Player;
    public bool Blocking;
    public bool Swinging;
    public bool Stunned;
    public float SwordRange;
    public LayerMask PlayerMask;
    public float StunnedTimer;
    public Popup popup;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player.transform);
        if (Swinging && Stunned == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, SwordRange, PlayerMask))
            {
                if (hit.transform.GetComponent<PlayerSwordController>().Block)
                {
                    hit.transform.GetComponent<PlayerSwordController>().KnockbackEnemy(transform);
                    hit.transform.GetComponent<PlayerSwordController>().StunEnemy(transform);
                    popup.Popups("Enemy Knockbacked and Stunned, player blocked");
                }
                if (hit.transform.GetComponent<PlayerSwordController>().Swinging)
                {
                    hit.transform.GetComponent<PlayerSwordController>().Stun();
                    hit.transform.GetComponent<PlayerSwordController>().KnockbackEnemy(transform);
                    hit.transform.GetComponent<PlayerSwordController>().StunEnemy(transform);
                    popup.Popups("Player Stunned And Knockbacked, Enemy Knockbacked and Stunned, player swung");
                }

                if (hit.transform.GetComponent<PlayerSwordController>().Swinging == false && hit.transform.GetComponent<PlayerSwordController>().Block == false)
                {
                    hit.transform.GetComponent<PlayerSwordController>().Stun();
                    popup.Popups("Enemy Stunned and Knockbacked Player, Player did not swing or block");
                }
            }
        }
    }

    public IEnumerator StunnedFunction()
    {
        //Debug.Log("stunned enemy");

        if (Stunned)
        {
            popup.Popups("Enemy Stunned and Knockbacked, enemy did not swing or block, enemy was already stunned");
        }
        else
        {
            popup.Popups("Enemy Stunned and Knockbacked, enemy did not swing or block");
        }

        Stunned = true;
        yield return new WaitForSeconds(StunnedTimer);
        Stunned = false;
    }
}

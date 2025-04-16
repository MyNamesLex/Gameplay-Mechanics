using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("Player")]
    public PlayerController player;

    [Header("Knife Stats")]
    public float Damage;
    public float RateOfFire;
    public float TargetHitKnockback;
    public float Range;

    [Header("Gun Bools")]
    public bool CanUse = true;

    [Header("Bullet")]
    public GameObject UseFromPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && CanUse)
        {
            StartCoroutine(Shoot());
        }
    }
    public IEnumerator Shoot()
    {
        CanUse = false;
        RaycastHit hit;
        if(Physics.Raycast(UseFromPos.transform.position, UseFromPos.transform.forward, out hit,Range))
        {
            if(hit.transform.gameObject.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
                Vector3 dif = hit.transform.position - player.transform.position;
                hit.transform.GetComponent<Enemy>().rb.AddForce(dif * TargetHitKnockback, ForceMode.VelocityChange);
            }
        }
        yield return new WaitForSeconds(RateOfFire);
        CanUse = true;
    }

    public void OnDisable()
    {
        CanUse = false;
    }

    public void OnEnable()
    {
        StartCoroutine(StartCanUseEnable());
    }

    public IEnumerator StartCanUseEnable()
    {
        CanUse = false;
        yield return new WaitForSeconds(RateOfFire);
        CanUse = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Player")]
    public PlayerController player;

    [Header("Gun Stats")]
    public float Damage;
    public float RateOfFire;
    public float PelletesAmountLow;
    public float PelletesAmountHigh;
    public float PelletMaxSpread;
   // public float PlayerKnockback;
    public float TargetHitKnockback;

    [Header("Gun Bools")]
    public bool CanShoot = true;

    [Header("Bullet")]
    public GameObject BulletParent;
    public GameObject ShootFromPos;
    public GameObject Bullet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && CanShoot)
        {
            StartCoroutine(Shoot());
        }
    }
    public IEnumerator Shoot()
    {
        CanShoot = false;
        float pellets = Random.Range(PelletesAmountLow, PelletesAmountHigh);
        for(int i = 0; i < pellets; i++)
        {
            GameObject g = Instantiate(Bullet, new Vector3(ShootFromPos.transform.position.x, ShootFromPos.transform.position.y, ShootFromPos.transform.position.z), player.Cam.transform.rotation, BulletParent.transform);
            g.GetComponent<Bullet>().maxSpread = PelletMaxSpread;
            g.GetComponent<Bullet>().Damage = Damage;
            g.GetComponent<Bullet>().player = player;
            g.GetComponent<Bullet>().TargetKnockback = TargetHitKnockback;
            Vector3 dir = player.Cam.transform.forward + new Vector3(Random.Range(-PelletMaxSpread, PelletMaxSpread), Random.Range(-PelletMaxSpread, PelletMaxSpread), Random.Range(-PelletMaxSpread, PelletMaxSpread));
            g.GetComponent<Bullet>().rb.AddForce(dir * g.GetComponent<Bullet>().Speed);
        }
        //Vector3 dif = ShootFromPos.transform.position - player.transform.position; // bit snappy, doesnt feel natural/normal
       // player.rb.AddForce(new Vector3(-dif.x, -dif.y , -dif.z) * PlayerKnockback, ForceMode.Acceleration);
        yield return new WaitForSeconds(RateOfFire);
        CanShoot = true;
    }

    public void OnDisable()
    {
        CanShoot = false;
    }

    public void OnEnable()
    {
        StartCoroutine(StartCanShootEnable());
    }

    public IEnumerator StartCanShootEnable()
    {
        CanShoot = false;
        yield return new WaitForSeconds(RateOfFire);
        CanShoot = true;
    }
}

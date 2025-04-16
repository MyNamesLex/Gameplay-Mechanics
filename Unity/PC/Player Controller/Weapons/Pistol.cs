using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [Header("Player")]
    public PlayerController player;

    [Header("Gun Stats")]
    public float Damage;
    public float RateOfFire;
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
        if(Input.GetMouseButton(0) && CanShoot)
        {
            StartCoroutine(Shoot());
        }
    }
    public IEnumerator Shoot()
    {
        CanShoot = false;
        GameObject g = Instantiate(Bullet,new Vector3(ShootFromPos.transform.position.x, ShootFromPos.transform.position.y, ShootFromPos.transform.position.z), player.Cam.transform.rotation, BulletParent.transform);
        g.GetComponent<Bullet>().Damage = Damage;
        g.GetComponent<Bullet>().player = player;
        g.GetComponent<Bullet>().TargetKnockback = TargetHitKnockback;
        g.GetComponent<Bullet>().rb.AddForce(player.Cam.transform.forward * g.GetComponent<Bullet>().Speed);
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

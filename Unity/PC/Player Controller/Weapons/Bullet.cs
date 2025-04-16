using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Rigidbody")]
    public PlayerController player;
    public Rigidbody rb;
    [Header("Bullet Stats")]
    public float Damage;
    public float Speed;
    public float TargetKnockback;
    public float Timeout;
    public float maxSpread;
    public ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeoutBullet());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if (TargetKnockback != 0)
            {
                Vector3 dif = other.transform.position - transform.position;
                other.GetComponent<Enemy>().rb.AddForce(-dif * TargetKnockback, ForceMode.Impulse);
            }
            other.GetComponent<Enemy>().TakeDamage(Damage);
            //Debug.Log("hit");
            Destroy(gameObject);
        }
        if(other.gameObject.layer == 3) // wall
        {
            Destroy(gameObject);
        }
        if (other.gameObject.layer == 6) // floor
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TimeoutBullet()
    {
        yield return new WaitForSeconds(Timeout);
        Destroy(gameObject);
    }
}

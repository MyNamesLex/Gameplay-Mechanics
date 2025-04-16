using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Rigidbody")]
    public Rigidbody rb;
    [Header("Enemy Stats")]
    [SerializeField] private float Health;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float Damage)
    {
        Health -= Damage;
    }
}

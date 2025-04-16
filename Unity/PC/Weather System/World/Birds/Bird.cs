using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public bool HitCollider1;
    public GameObject Collider1Pos;
    public GameObject Collider2Pos;
    public float Speed;
    public bool DestroyOnCollision = false;
    // Update is called once per frame
    void Update()
    {
        float step = Speed * Time.deltaTime;
        if (HitCollider1 == false)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, Collider1Pos.transform.position, step);
        }
        else if (HitCollider1 == true)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, Collider2Pos.transform.position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collider1"))
        {
            if (DestroyOnCollision == true)
            {
                Destroy(gameObject);
            }
            HitCollider1 = true;
        }
        if (other.CompareTag("Collider2"))
        {
            HitCollider1 = false;
        }
    }
}

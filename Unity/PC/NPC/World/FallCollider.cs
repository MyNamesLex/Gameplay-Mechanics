using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCollider : MonoBehaviour
{
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
        if(other.CompareTag("Player"))
        {
            Debug.Log("Hit Fall Collider, Moving To Spawn");
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = new Vector3(41.4f, 8.08f, 49.7f);
        }
    }
}

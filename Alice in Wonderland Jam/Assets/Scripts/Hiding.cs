using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    public GameObject alice;

    public void Awake()
    {
        alice = GameObject.FindObjectOfType<AliceController>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Alice")
        {
            alice.GetComponent<AliceController>().isHiding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Alice")
        {
            alice.GetComponent<AliceController>().isHiding = false;
        }
    }

}

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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Alice"))
        {
            alice.GetComponent<AliceController>().isHiding = true;
        }
    }
       
    

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Alice"))
        {
            alice.GetComponent<AliceController>().isHiding = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Alice"))
        {
            alice.GetComponent<AliceController>().isHiding = false;
        }
    }

}

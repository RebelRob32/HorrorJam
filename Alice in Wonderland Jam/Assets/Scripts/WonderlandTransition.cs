using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonderlandTransition : MonoBehaviour
{
    public GameManager gm;
    public AliceController alice;
    public RabbitController rabbit;
    public GameObject wonderlandPortal;

    public void Awake()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        alice = GameObject.FindObjectOfType<AliceController>();
        rabbit = GameObject.FindObjectOfType<RabbitController>();
    }

    public void Update()
    {
        if(wonderlandPortal == null)
        {
            return;
        }

        if (alice.fearLevel <= 50f && rabbit.isClose == true)
        {
            bool isActive = wonderlandPortal.activeSelf;
            wonderlandPortal.SetActive(true);
        }
        else
        if(rabbit.isClose == false)
        {
            wonderlandPortal.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Rabbit" && alice.fearLevel <= 50f && rabbit.isClose == true)
        {
            gm.MoveToWonderland();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rabbit" && alice.fearLevel <= 50f && rabbit.isClose == true)
        {
            gm.MoveToWonderland();
        }
    }
}

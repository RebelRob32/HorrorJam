using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonderlandTransition : MonoBehaviour
{
    public GameManager gm;
    public AliceController alice;
    public RabbitController rabbit;

    public void Awake()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Rabbit" && alice.fearLevel <= 50f && rabbit.isClose == true)
        {
            gm.MoveToWonderland();
        }
    }
}

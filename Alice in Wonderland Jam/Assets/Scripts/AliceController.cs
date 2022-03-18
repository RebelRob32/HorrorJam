using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceController : MonoBehaviour
{
    public AliceStats stats;
    public GameObject rabbit;
    public GameObject[] hidingSpots;

    public Transform closestHidingSpot;
    public float fearLevel;

    public bool rabbitClose;
    public bool isScared;
    public bool isHiding;

    #region WireSphere
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stats.range);

    }
    #endregion


    public void Awake()
    {
        fearLevel = stats.fear;
        rabbit = GameObject.FindGameObjectWithTag("Rabbit");
        hidingSpots = GameObject.FindGameObjectsWithTag("Hiding");

        closestHidingSpot = GetHidingSpots();
    }

    public void Update()
    {
        if(fearLevel <= 0f)
        {
            fearLevel = stats.fear;
        }

        if(fearLevel >= 100f)
        {
            fearLevel = 100f;
        }

        FearSystem();
    }

    public void FixedUpdate()
    {
        FollowRabbit();
        Hide();
    }

    public void FollowRabbit()
    {
        if(rabbit.GetComponent<RabbitController>().isCalling == true)
        {
            StartCoroutine(FollowForSeconds());
        }
        else
        {
            transform.position = transform.position;
        }
    }

    public void Hide()
    {
        if(rabbit.GetComponent<RabbitController>().isHiding == true)
        {
            StartCoroutine(FindHidingSpot());
        }
        else
        {
            transform.position = transform.position;
        }
    }

    public void RunAway()
    {

    }

    IEnumerator FollowForSeconds()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, rabbit.transform.position, Time.deltaTime * stats.speed);
        yield return new WaitForSeconds(3);
        transform.position = transform.position;
    }

   

    public Transform GetHidingSpots()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("Hiding");

        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject hiding in hidingSpots)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, hiding.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = hiding.transform;
            }
        }
        return trans;
    }
     IEnumerator FindHidingSpot()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, closestHidingSpot.transform.position, Time.deltaTime * stats.speed);

        yield return new WaitForSeconds(5);
        
    }
    public void FearSystem()
    {
        float distance = Vector3.Distance(transform.position, rabbit.transform.position);
        
        if (distance <= stats.range)
        {
            rabbitClose = true;
           
            if (rabbitClose == true)
            {
                transform.LookAt(rabbit.transform.position);
                fearLevel += 1f * Time.deltaTime;
                if (fearLevel <= 100f)
                {
                    isScared = true;
                }

            }
            else
                if(rabbitClose == false)
            {
                transform.LookAt(transform.position);
            }
           
        }
        else
        {
            if(distance >= stats.range)
            {
                isScared = false;
                rabbitClose = false;
                fearLevel -= 1f * Time.deltaTime;
                
            }
        }
    }

    private void OnTriggerEnter(Collider hSpot)
    {
        if(hSpot == GameObject.FindGameObjectWithTag("Hiding"))
        {
            isHiding = true;
        }
    }

    private void OnTriggerExit(Collider hSpot)
    {
        if (hSpot == GameObject.FindGameObjectWithTag("Hiding"))
        {
            isHiding = false;
        }
    }
}

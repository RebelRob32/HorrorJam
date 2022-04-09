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
    public bool isFollowing;

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

        
    }

    public void Update()
    {
        closestHidingSpot = GetHidingSpots();

        if(fearLevel <= 0f)
        {
            fearLevel = stats.fear;
        }

        if(fearLevel >= 100f)
        {
            fearLevel = 100f;
        }

        FearSystem();

        if(rabbit.GetComponent<RabbitController>().inRange == true)
        {
            transform.LookAt(rabbit.transform);
        }
        else
        {
            return;
        }
    }

    public void FixedUpdate()
    {
        FollowRabbit();
        Hide();
        RunAway();
    }

    public void FollowRabbit()
    {
        if (isFollowing == true)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, rabbit.transform.position, Time.deltaTime * stats.speed);
        }
        else
            return;
        
    }

    public void Hide()
    {
        if(rabbit.GetComponent<RabbitController>().isHiding == true)
        {
           transform.position = Vector3.MoveTowards(transform.position, closestHidingSpot.transform.position, Time.deltaTime * stats.speed * 2);
            transform.LookAt(transform.position, closestHidingSpot.transform.position);
        }
        
    }

    public void RunAway()
    {
        if(fearLevel >= 90f)
        {
            StartCoroutine(EscapeRabbit());
        }
    }

    IEnumerator EscapeRabbit()
    {
        Hide();
        yield return new WaitForSeconds(5);
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
            if(isHiding == true)
            {
                isFollowing = false;
                
            }
           
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

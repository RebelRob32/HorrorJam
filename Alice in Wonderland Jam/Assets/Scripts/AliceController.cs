using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceController : MonoBehaviour
{
    public AliceStats stats;
    public Potions sizePotions;
    public GameObject rabbit;
    public GameObject[] hidingSpots;
    public GameObject largePotion;
    public GameObject smallPotion;
    public GameObject smallKey;
    public GameObject[] doors;
    public GameObject doorLock;
    

    public Transform closestHidingSpot;
    public float fearLevel;
    public Vector3 hSpot;
    

    public bool rabbitClose;
    public bool isScared;
    public bool isHiding;
    public bool isFollowing;
    public bool isBig;
    public bool hasKey;
    public bool isLocked;

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
        doors = GameObject.FindGameObjectsWithTag("Door");

        if(doors == null)
        {
            return;
        }

        if(doorLock == null)
        {
            return;
        }

        if(doorLock != null)
        {
            isLocked = true;
        }

        if(largePotion == null)
        {
            return;
        }

        if(smallPotion == null)
        {
            return;
        }

        
    }

    public void Update()
    {
        closestHidingSpot = GetHidingSpots();
        hSpot = closestHidingSpot.transform.position;

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

        if(isLocked == false)
        {
            foreach (GameObject door in doors)
            {
                bool isActive = door.activeSelf;
                door.SetActive(false);
            }
        }
    }

    public void FixedUpdate()
    {
        FollowRabbit();
        Hide();
        RunAway();

        if(rabbit.GetComponent<RabbitController>().isSending == true)
        {
            GoToPoint();
        }

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
        if(rabbit.GetComponent<RabbitController>().hideAlice == true)
        {
           transform.position = Vector3.MoveTowards(transform.position, closestHidingSpot.transform.position, Time.deltaTime * stats.speed * 2);
          
            if(rabbit.GetComponent<RabbitController>().isCalling == true)
            {
                FollowRabbit();
            }
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

   public void GoToPoint()
    {
        if(rabbit.GetComponent<RabbitController>().isSending == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, rabbit.GetComponent<RabbitController>().closestObject.transform.position, Time.deltaTime * stats.speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hiding")
        {
            isHiding = true;
            isFollowing = false;
        }

        if(other.tag == "KeyHole" && hasKey == true)
        {
            isLocked = false;
        }

        if(other.name == "Key")
        {
            bool isActive = smallKey.activeSelf;
            smallKey.SetActive(false);
        }

        if(other.name == "BigPotion")
        {
            transform.localScale = new Vector3(transform.localScale.x, 5F, transform.localScale.y);
            bool isActive = largePotion.activeSelf;
            largePotion.SetActive(false);
        }

        if(other.name == "SmallPotion")
        {
            transform.localScale = new Vector3(transform.localScale.x, -5f, transform.localScale.y);
            bool isActive = smallPotion.activeSelf;
            smallPotion.SetActive(false);
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hiding")
        {
            isHiding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Hiding")
        {
            isHiding = false;
        }
    }
}

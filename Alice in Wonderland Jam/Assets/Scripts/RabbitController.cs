using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    public GameManager gm;
    public RabbitStats stats;
    public CharacterController controller;
    public Vector3 direction;

    public GameObject radarSphere;

    public GameObject[] enemies;
    public GameObject sendPoint;
    public Transform closestEnemy;

    public GameObject alice;
    public GameObject calloutText;
    public GameObject sootheText;
    public Transform followCam;
    public Transform startPoint;

    private Vector3 inputs = Vector3.zero;
    public float callout;
    public float sootheCharges;

    public bool inRange;
    public bool enemInRange;
    public bool enemDistracted;
    public bool isCalling;
    public bool isClose;
    public bool isHiding;
    public bool isSending;
    public bool outOfCharges;


    public Animator anim;

    #region WireSphere
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, callout);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stats.closeRange);
    }
    #endregion

    public void Awake()
    {
        callout = stats.callRange;
        sootheCharges = 3f;
        controller = GetComponent<CharacterController>();
        alice = GameObject.FindGameObjectWithTag("Alice");
        gm = GameObject.FindObjectOfType<GameManager>();
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        closestEnemy = null;

        gameObject.transform.position = startPoint.transform.position;
        
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        closestEnemy = GetEnemiesInRange();
        checkRange();
        CallOut();
        Soothe();
        DistractEnemy();
    }

    public void FixedUpdate()
    {
       
        PlayerMove();
    }

    public void PlayerMove()
    {

        inputs = Vector3.zero;
        inputs.x = Input.GetAxisRaw("Horizontal");
        inputs.z = Input.GetAxisRaw("Vertical");

        var inputDir = Quaternion.AngleAxis(followCam.rotation.eulerAngles.y, Vector3.up) * inputs;

        if (inputDir.magnitude >= 0.1f)
        {
            inputDir = inputDir.normalized;

            anim.SetBool("WALKING", true);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDir, Vector3.up), stats.rotateSpeed * Time.deltaTime);

            controller.SimpleMove(inputDir.normalized * stats.speed * 2f * Time.deltaTime);
        }
        else
        {
            anim.SetBool("WALKING", false);
        }
    }

    public void checkRange()
    {
        float distance = Vector3.Distance(transform.position, alice.transform.position);

        if(distance <= callout)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
        if(distance <= stats.closeRange)
        {
            isClose = true;
        }
        else
        {
            isClose = false;
        }

    }

    public Transform GetEnemiesInRange()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject enemy in enemies)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = enemy.transform;
            }
        }
        return trans;
    }

    public void DistractEnemy()
    {
        if(closestEnemy != null)
        {
            float enemyDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

            if(enemyDistance <= stats.callRange)
            {
                enemInRange = true;
            }
            else
            {
                enemInRange = false;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(enemInRange == true)
                {
                    Debug.Log("Distraction!");
                    StartCoroutine(DistractTime());
                    
                }
            }
        }
        else
        {
            return;
        }
    }

    public void CallOut()
    {   
        
        float distance = Vector3.Distance(transform.position, alice.transform.position);

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(distance <= callout)
            {
                Debug.Log("Alice!");
                StartCoroutine(CallRangeDecrease());
                StartCoroutine(CallTextDuration());
                alice.GetComponent<AliceController>().fearLevel += 5f;
                alice.GetComponent<AliceController>().isFollowing = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(distance <= callout)
            {
                Debug.Log("Hide!");
                StartCoroutine(HideAlice());
                alice.GetComponent<AliceController>().fearLevel += 10f;
            }
        }

        if(Input.GetKey(KeyCode.V))
        {
            Debug.Log("Action!");
            //interact with potions, key and lock
            
        }
    }

    public void Soothe()
    {
        if(Input.GetKeyDown(KeyCode.Q) && inRange == true)
        {
            alice.GetComponent<AliceController>().fearLevel -= 15f;
            sootheCharges -= 1f;
            StartCoroutine(SootheTextTime());
            if(sootheCharges <= 0f && inRange == true)
            {
                outOfCharges = false;
                if(outOfCharges == false)
                {
                    return;
                }
            }
        }
    }

    IEnumerator CallRangeDecrease()
    {
        isCalling = true;
        

        if(isCalling == true)
        {
            bool isActive = radarSphere.activeSelf;
            radarSphere.SetActive(true);
            yield return new WaitForSeconds(1);
            radarSphere.SetActive(false);
            callout = 0.5f;
        }
        yield return new WaitForSeconds(5);
        isCalling = false;
    }

    IEnumerator CallTextDuration()
    {
        bool isActive = calloutText.activeSelf;
        calloutText.SetActive(true);
        calloutText.transform.position = this.transform.position + new Vector3(.5f, .5f, 0);
        yield return new WaitForSeconds(1.5f);
        callout = stats.callRange;
        calloutText.SetActive(false);
    }

    IEnumerator SootheTextTime()
    {
        bool isActive = sootheText.activeSelf;
        sootheText.SetActive(true);
        sootheText.transform.position = this.transform.position + new Vector3(.5f, .5f, 0);
        yield return new WaitForSeconds(2);
        sootheText.SetActive(false);
    }

    IEnumerator HideAlice()
    {
        isHiding = true;
        StartCoroutine(CastSphere());

        yield return new WaitForSeconds(5);
        isHiding = false;
    }

   

    IEnumerator CastSphere()
    {
        bool isActive = radarSphere.activeSelf;
        radarSphere.SetActive(true);
        yield return new WaitForSeconds(1);
        radarSphere.SetActive(false);
    }

    IEnumerator DistractTime()
    {
        StartCoroutine(CastSphere());

        yield return new WaitForSeconds(5);

        
    }

   
}

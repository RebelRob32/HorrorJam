using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    public GameManager gm;
    public RabbitStats stats;
    public CharacterController controller;
    public Vector3 direction;
 

    public GameObject alice;
    public GameObject calloutText;
    public Transform followCam;
    public Transform startPoint;

    private Vector3 inputs = Vector3.zero;
    public float callout;

    public bool inRange;
    public bool isCalling;
    public bool isClose;
    public bool isHiding;

    #region WireSphere
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, callout);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.closeRange);
    }
    #endregion

    public void Awake()
    {
        callout = stats.callRange;
        controller = GetComponent<CharacterController>();
        alice = GameObject.FindGameObjectWithTag("Alice");
        gm = GameObject.FindObjectOfType<GameManager>();
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;

        gameObject.transform.position = startPoint.transform.position;
        
    }

    public void Update()
    {
        checkRange();
        CallOut();
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

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDir, Vector3.up), stats.rotateSpeed * Time.deltaTime);

            controller.SimpleMove(inputDir.normalized * stats.speed * 2f * Time.deltaTime);
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
            return;
        }
        if(distance <= stats.closeRange)
        {
            isClose = true;
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
                alice.GetComponent<AliceController>().fearLevel += 5f;
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
    }

    IEnumerator CallRangeDecrease()
    {
        isCalling = true;
        if(isCalling == true)
        {
            callout = 0.5f;
        }
        bool isActive = calloutText.activeSelf;
        calloutText.SetActive(true);
        calloutText.transform.position = this.transform.position + new Vector3(.5f, .5f, 0);
        yield return new WaitForSeconds(5);
        isCalling = false;
        callout = stats.callRange;
        calloutText.SetActive(false);
    }

    IEnumerator HideAlice()
    {
        isHiding = true;
        yield return new WaitForSeconds(5);
        isHiding = false;
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col == GameObject.FindGameObjectWithTag("Trigger") && isClose == true && alice.GetComponent<AliceController>().fearLevel <= 50f)
        {
            gm.MoveToWonderland();
        }
    }



}

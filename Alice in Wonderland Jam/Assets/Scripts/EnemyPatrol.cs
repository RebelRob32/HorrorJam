using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject alice;
    public EnemyStats stats;
    public GameManager gm;

    public Transform[] waypoints;
    public int curPoint = 0;

    public void Update()
    {
        alice = GameObject.FindGameObjectWithTag("Alice");
        gm = GameObject.FindObjectOfType<GameManager>();

        if(alice == null)
        {
            return;
        }
    }


    public void FixedUpdate()
    {
        Patrol();
    }


    public void Patrol()
    {
        if (alice != null)
        {
            float distance = Vector3.Distance(transform.position, alice.transform.position);

            if (distance >= stats.range)
            {
                Transform wp = waypoints[curPoint];

                if (Vector3.Distance(transform.position, wp.position) < 0.1f)
                {
                    curPoint = (curPoint + 1) % waypoints.Length;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, wp.position, stats.speed * Time.deltaTime);
                    transform.LookAt(wp.position);
                }
            }

            if(distance <= stats.range)
            {
                transform.position = Vector3.MoveTowards(transform.position, alice.transform.position, stats.speed * 1.5f * Time.deltaTime);
            }
        }
        else
        {
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, stats.range);
        Gizmos.color = Color.yellow;
    }

    private void OnCollisionEnter(Collision col)
    {
     if(col.gameObject.tag == "Alice")
        {
            gm.LoseGame();
        }
    }


}

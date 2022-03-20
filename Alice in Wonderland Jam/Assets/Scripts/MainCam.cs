using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    public float turnSpeed = 5.0f;
    public GameObject player;
    public bool upgrading = false;

    public Transform playerTransform;
    public Vector3 offset;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Rabbit");
    }

    void Start()
    {
        playerTransform = player.transform;
    }

    private void Update()
    {
        if (Input.GetMouseButton(2))
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        }
    }

    void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        if (upgrading)
        {
            transform.position = new Vector3(0, 22, -37);
            transform.eulerAngles = new Vector3(15, 0, 0);
        }
        else
        {
            transform.position = playerTransform.position + offset;
            transform.LookAt(playerTransform.position);
        }
    }

}

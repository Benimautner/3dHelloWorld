using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController controller;
    public Rigidbody playerBody;
    public Camera camera;
    private Vector3 movement;
    public float mouseMultiplicator;
    private bool onFloor;
    private Vector3 dir;
    void Start()
    {
        //controller = gameObject.GetComponent<PlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKey("space") && onFloor)
        {
            playerBody.AddForce(0, 100, 0);
        }
        
        dir = Vector3.zero;

        if(Input.GetKey("w"))
        {
            dir = camera.transform.forward / 5;
            dir.y = 0;
        }
        if (Input.GetKey("s"))
        {
            dir = -camera.transform.forward / 5;
            dir.y = 0;
        }
        if(Input.GetKey("a"))
        {
            playerBody.position += -camera.transform.right / 5;
        }
        if (Input.GetKey("d"))
        {
            playerBody.position += camera.transform.right / 5;
            movement = Vector3.right;
        }
        playerBody.position += dir;


        if (Input.GetKey("e"))
        {
            Application.Quit();
        }


    }


    private void OnCollisionEnter(Collision other)
    {
        onFloor = true;
    }

    private void OnCollisionExit(Collision other)
    {
        onFloor = false;
    }
}

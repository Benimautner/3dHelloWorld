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
    private Vector3 _movement;
    public float mouseMultiplicator;
    private bool _onFloor;
    private Vector3 _dir;
    void Start()
    {
        //controller = gameObject.GetComponent<PlayerController>();
        playerBody.position = new Vector3(0, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKey("space") && _onFloor)
        {
            playerBody.AddForce(0, 100, 0);
        }
        
        _dir = Vector3.zero;

        if(Input.GetKey("w"))
        {
            _dir = camera.transform.forward / 5;
            _dir.y = 0;
        }
        if (Input.GetKey("s"))
        {
            _dir = -camera.transform.forward / 5;
            _dir.y = 0;
        }
        if(Input.GetKey("a"))
        {
            playerBody.position += -camera.transform.right / 5;
        }
        if (Input.GetKey("d"))
        {
            playerBody.position += camera.transform.right / 5;
            _movement = Vector3.right;
        }
        playerBody.position += _dir;


        if (Input.GetKey("e"))
        {
            Application.Quit();
        }
        


    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), ((int) (1.0f / Time.smoothDeltaTime)).ToString());
    }


    private void OnCollisionEnter(Collision other)
    {
        _onFloor = true;
    }

    private void OnCollisionExit(Collision other)
    {
        _onFloor = false;
    }

    private void OnCollisionStay(Collision other)
    {
        throw new NotImplementedException();
    }
}

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
    [SerializeField] public PlayerController controller;
    [SerializeField] public CharacterController characterController;
    [SerializeField] public Rigidbody playerBody;
    [SerializeField] public float mouseMultiplicator;
    [SerializeField] public Camera camera;
    private const float speedCoeff = 12;
    private bool _onFloor;
    private Vector3 _dir = Vector3.zero;
    void Start()
    {
        characterController.Move(new Vector3(0, 10, 0));
        //playerBody.position = new Vector3(0, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camFwd = camera.transform.forward;
        camFwd.y = 0;

        _dir.x = 0;
        _dir.z = 0;

        if(Input.GetKey("w"))
        {
            _dir += camFwd;
        }
        if (Input.GetKey("s"))
        {
            _dir -= camFwd;
        }
        if(Input.GetKey("a"))
        {
            _dir += -camera.transform.right;
        }
        if (Input.GetKey("d"))
        {
            _dir += camera.transform.right;
        }

        print(characterController.isGrounded);
        if (Input.GetKey("space") && characterController.isGrounded)
        {
            _dir.y = Mathf.Sqrt(-Physics.gravity.y / 2);
        }

        _dir += Time.deltaTime * Physics.gravity / 2;

        characterController.Move(speedCoeff * Time.deltaTime * _dir);
        

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
}

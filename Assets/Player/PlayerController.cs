﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float SpeedCoeff = 12;

    // Start is called before the first frame update
    [SerializeField] public PlayerController controller;
    [SerializeField] public CharacterController characterController;
    [SerializeField] public Rigidbody playerBody;
    [SerializeField] public float mouseMultiplicator;
    [SerializeField] public new Camera camera;
    [SerializeField] public bool gravityEnabled;
    private Vector3 _dir = Vector3.zero;

    private void Start()
    {
        characterController.Move(new Vector3(0, 50, 0));
        //playerBody.position = new Vector3(0, 10, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        var camFwd = camera.transform.forward;
        camFwd.y = 0;

        _dir.x = 0;
        _dir.z = 0;

        if (Input.GetKey("w")) _dir += camFwd;
        if (Input.GetKey("s")) _dir -= camFwd;
        if (Input.GetKey("a")) _dir += -camera.transform.right;
        if (Input.GetKey("d")) _dir += camera.transform.right;

        if (Input.GetKey("space") && characterController.isGrounded) _dir.y = Mathf.Sqrt(-Physics.gravity.y / 3);

        if (gravityEnabled) _dir += Time.deltaTime * Physics.gravity / 2;

        characterController.Move(SpeedCoeff * Time.deltaTime * _dir);

        if (Input.GetKey("e")) Application.Quit();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 1000, 1000), ((int) (1.0f / Time.smoothDeltaTime)).ToString());
    }
}